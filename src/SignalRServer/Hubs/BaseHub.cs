using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SignalRServer.Models;
using SignalRServer.Dtos;
using SignalRServer.Benchmark;

namespace SignalRServer.Hubs
{
    /// <summary>
    /// 基础消息处理
    /// </summary>
    public partial class ChatHub : Hub<ISignalrClient>
    {
        private readonly ConnectionCounter _counter;

        static ChatHub()
        {
            //MonitorConnections();
            MonitorFeedback();
        }

        public ChatHub(ConnectionCounter counter)
        {
            _counter = counter;
        }

        /// <summary>
        /// 学生机注册
        /// </summary>
        /// <param name="registerInfo">注册信息</param>
        /// <returns></returns>
        public async Task Register(RegisterInfo registerInfo)
        {
            //WriteMessage(CommandType.RegisterTeacher, regInfo);

            var ip = Context.GetHttpContext().Connection.RemoteIpAddress.ToString();

            var clientSummary = new ClientSummary
            {
                ConnectionId = Context.ConnectionId,
                IsConnected = true,
                RemoteIpAddress = ip,
                Identity = registerInfo.Identity,
                TeacherCode = registerInfo.TeacherCode,
                Code = registerInfo.Code,
                Data = registerInfo.Data
            };

            if (registerInfo.Identity == IdentityType.Teacher)
            {
                await RegisterTeacher(clientSummary);
            }
            else if (registerInfo.Identity == IdentityType.Student)
            {
                await RegisterStudent(clientSummary);
            }
            else
            {
                throw new ArgumentException("注册信息 Identity 错误");
            }

            // 应答注册成功
            //await Clients.Caller.SendAsync(EventType.RegisterSuccess, clientSummary);
            await Clients.Caller.RegisterSuccess(clientSummary);
        }

        /// <summary>
        /// 教师机注册处理
        /// </summary>
        /// <param name="clientSummary"></param>
        /// <returns></returns>
        private async Task RegisterTeacher(ClientSummary clientSummary)
        {
            // 保存教师机信息
            Storage.TeacherList.Add(clientSummary);

            // 创建教师机分组
            await Groups.AddToGroupAsync(Context.ConnectionId, clientSummary.Code);
        }

        /// <summary>
        /// 学生机注册处理
        /// </summary>
        /// <param name="clientSummary"></param>
        /// <returns></returns>
        private async Task RegisterStudent(ClientSummary clientSummary)
        {
            // 检查教师机连接是否正常
            var teacher = Storage.TeacherList.FirstOrDefault(p => p.Code == clientSummary.TeacherCode);
            if (teacher == null || !teacher.IsConnected)
            {
                throw new ArgumentNullException($"教师机连接异常[code={clientSummary.TeacherCode}]！");
            }

            // 保存学生机信息
            Storage.AddStudent(clientSummary);

            // 将学生机加入对应的教师机分组
            await Groups.AddToGroupAsync(Context.ConnectionId, clientSummary.TeacherCode);

            // 通知教师机
            //await Clients.Client(teacher.ConnectionId).SendAsync(EventType.ReceiveMessage, CommandType.StudentConnected, clientSummary);
            await Clients.Client(teacher.ConnectionId).ReceiveMessage(CommandType.StudentConnected, clientSummary);
        }


        /// <summary>
        /// TODO...
        /// 客户端接入回调
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            _counter.Connected();

            // TODO，连接成功后，注册之前，要不要处理？？
            //Storage.UpdateConnectedState(Context.ConnectionId, true);
            return base.OnConnectedAsync();
        }

        /// <summary>
        /// 客户端断开链接回调
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override Task OnDisconnectedAsync(Exception exception)
        {
            _counter.Disconnected();

            Storage.UpdateConnectedState(Context.ConnectionId, false);
            return base.OnDisconnectedAsync(exception);
        }

        public void SendPayload(string payload)
        {
            _counter.Receive(payload);
        }

        /// <summary>
        /// 消息接收反馈
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public async Task Feedback(Guid messageId)
        {
            await MessageHistory.Feedback(messageId, Context.ConnectionId);
        }

        /// <summary>
        /// 监听当前连接情况
        /// </summary>
        private static void MonitorConnections()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    var stuCount = Storage.StudentList.Values.Select(p => p.Count).Sum();
                    Console.WriteLine($"Connections：教师机数量:{Storage.TeacherList.Count} 学生机数量 {stuCount}");
                    Thread.Sleep(1000);
                }
            });
        }

        /// <summary>
        /// 监听当前连接情况
        /// TODO：超时后续处理？？？
        /// </summary>
        private static void MonitorFeedback()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    // 3秒超时，未收到有效接收反馈的消息记录
                    var timeouts = MessageHistory.MessageRecords.Where(p => p.State != true && p.SendTime.AddSeconds(3) > DateTime.Now).ToList();
                    foreach (var message in timeouts)
                    {
                        WriteTimeoutMessage(message);
                    }
                    Thread.Sleep(1000);
                }
            });
        }

        /// <summary>
        /// 输出超时消息内容
        /// </summary>
        private static void WriteTimeoutMessage(MessageSummary message)
        {
            Console.WriteLine(message.ToString());
        }

        /// <summary>
        /// 输出消息内容
        /// </summary>
        /// <param name="command"></param>
        /// <param name="data"></param>
        private void WriteCommand(string command, object data)
        {
            Console.WriteLine(new CommandSummary(command, data).ToString());
        }
    }
}
