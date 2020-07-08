using Microsoft.AspNetCore.SignalR;
using SignalRServer.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace SignalRServer.Hubs
{
    public class ChatHub : Hub
    {
        static ChatHub()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    Console.WriteLine($"ConnectionCount：教师机数量:{Storage.TeacherList.Count} 学生机数量 {Storage.StudentList.Count()}");
                    Thread.Sleep(1000);
                }
            });
        }

        #region 注册教师机&学生机
        // 教师机注册
        public async Task RegisterTeacher(TeacherRegisterInfo regInfo)
        {
            //WriteMessage(CommandType.RegisterTeacher, regInfo);

            var clientInfo = new ClientSummary
            {
                ConnectionId = Context.ConnectionId,
                Identity = "Teacher",
                Code = regInfo.Code,
                Data = regInfo.Data
            };

            Storage.TeacherList.Add(clientInfo);

            // 创建教师机分组
            await Groups.AddToGroupAsync(Context.ConnectionId, regInfo.Code);

            // 应答收到消息
            await Clients.Caller.SendAsync("RegisterSuccess", clientInfo);
        }

        // 学生机注册
        public async Task RegisterStudent(StudentRegisterInfo regInfo)
        {
            //WriteMessage(CommandType.RegisterStudent, regInfo);

            var clientInfo = new ClientSummary
            {
                ConnectionId = Context.ConnectionId,
                Identity = "Student",
                Code = regInfo.Code,
                Data = regInfo.Data
            };

            if (Storage.StudentList.ContainsKey(regInfo.TeacherCode))
            {
                Storage.StudentList[regInfo.TeacherCode].Add(clientInfo);
            }
            else
            {
                var list = new List<ClientSummary>() { clientInfo };
                Storage.StudentList.TryAdd(regInfo.TeacherCode, list);
            }

            // 将学生机加入对应的教师机分组
            await Groups.AddToGroupAsync(Context.ConnectionId, regInfo.TeacherCode);

            // 回复学生机注册成功
            await Clients.Caller.SendAsync("RegisterSuccess", regInfo);

            // 通知教师机
            var teacher = Storage.TeacherList.FirstOrDefault(p => p.Code == regInfo.TeacherCode);
            await Clients.Client(teacher.ConnectionId).SendAsync("ReceiveMessage", "StudentRegister", clientInfo);
        }

        #endregion

        #region 接收指令
        /// <summary>
        /// 接受客户端消息
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="teacherCode"></param>
        /// <returns></returns>
        public async Task SendMessage(string commandType, string teacherCode)
        {
            //WriteMessage(commandType, teacherCode);
            var ip = Context.GetHttpContext().Connection.RemoteIpAddress;
            //MessageHistory.SaveMessage(Context.ConnectionId, teacherCode);
            switch (commandType)
            {
                case CommandType.Start:
                case CommandType.Stop:
                    {
                        await SendToAllStudents(commandType, teacherCode);
                        break;
                    }
                default:
                    break;
            }
            await Clients.Group(teacherCode).SendAsync("ReceiveMessage", commandType, teacherCode);

            //await Clients.All.SendAsync("ReceiveMessage", commandType, teacherCode);
        }

        private async Task SendToAllStudents(string commandType, string teacherCode)
        {
            await Clients.Group(teacherCode).SendAsync("ReceiveMessage", commandType, teacherCode);
        }

        private async Task SendToPartStudents(string commandType, string teacherCode)
        {

        }
        private async Task SendToUser()
        {

        }

        #endregion


        /// <summary>
        /// 消息接收反馈
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public async Task Feedback(Guid messageId)
        {
            await MessageHistory.Feedback(messageId, Context.ConnectionId);
        }

        #region  客户端接入 & 断开
        /// <summary>
        /// 客户端接入回调
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            //Clients.Caller.SendAsync("ReceiveMessage", "Server", "Connected!");
            return base.OnConnectedAsync();
        }

        /// <summary>
        /// 客户端断开链接回调
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override Task OnDisconnectedAsync(Exception exception)
        {
            var connectionId = Context.ConnectionId;
            var teacher = Storage.TeacherList.FirstOrDefault(p => p.ConnectionId == connectionId);
            if (teacher != null)
            {
                Storage.TeacherList.TryTake(out teacher);
            }
            else
            {
                foreach (var students in Storage.StudentList)
                {
                    var student = students.Value.FirstOrDefault(p => p.ConnectionId == connectionId);
                    if (student != null)
                    {
                        students.Value.Remove(student);
                        break;
                    }
                }
            }
            return base.OnDisconnectedAsync(exception);
        }
        #endregion

        ///// <summary>
        ///// 输出消息内容
        ///// </summary>
        ///// <param name="command"></param>
        ///// <param name="data"></param>
        //private void WriteMessage(string command, object data)
        //{
        //    Console.WriteLine(new CommandSummary(command, data).ToString());
        //}
    }
}
