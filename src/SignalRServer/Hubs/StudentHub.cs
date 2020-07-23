using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SignalRServer.Dtos;
using SignalRServer.Models;
using SignalRServer.Feedback;
using Autofac;
using SignalRServer.Handlers;

namespace SignalRServer.Hubs
{
    /// <summary>
    /// 基础消息处理
    /// @Created by Leo 2020/7/13 19:59:30
    /// </summary>
    public partial class ProxyHub : Hub<ISignalrClient>
    {

        /// <summary>
        /// 状态上报
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        public async Task Status(string jsondata)
        {
            ICommandHandler _commandHandler = _componentContext.ResolveNamed<ICommandHandler>("Status");
            await _commandHandler.Process(Context, jsondata);
        }

        /// <summary>
        /// 学生机注册
        /// </summary>
        /// <param name="registerInfo">注册信息</param>
        /// <returns></returns>
        public async Task RegisterStudent(RegisterCmdParams registerInfo)
        {
            if (registerInfo.Identity != IdentityType.Student)
            {
                throw new ArgumentException("注册信息 Identity 错误");
            }

            // 检查教师机连接是否正常
            var teacher = _clientStorage.TeacherClients.FirstOrDefault(p => p.TeacherCode == registerInfo.TeacherCode);
            if (teacher == null || !teacher.IsConnected)
            {
                var msg = $"教师机连接异常[code={registerInfo.TeacherCode}]！";
                await Clients.Client(teacher.ConnectionId).ReceiveServerError(msg);
                _logger.LogError(msg);
                throw new Exception(msg);
            }

            var ip = Context.GetHttpContext().Connection.RemoteIpAddress.ToString();
            var clientSummary = new StudentClientSummary
            {
                Identity = registerInfo.Identity,
                ConnectionId = Context.ConnectionId,
                IsConnected = true,
                RemoteIpAddress = ip,
                TeacherCode = registerInfo.TeacherCode,
            };

            // 保存学生机信息
            _clientStorage.AddStudent(clientSummary);

            // 将学生机加入对应的教师机分组
            await Groups.AddToGroupAsync(Context.ConnectionId, clientSummary.TeacherCode);

            // 通知教师机有学生机接入，保存消息记录
            var receivers = new List<MessageReceiver>
            {
                new MessageReceiver(){ConnectionId=teacher.ConnectionId}
            };
            var messageSummary = _messageHistory.SaveMessage(Context.ConnectionId, receivers, "ReceiveMessage", CommandType.StudentConnected, clientSummary);

            clientSummary.MessageId = messageSummary.Id;
            await Clients.Client(teacher.ConnectionId).ReceiveMessage(CommandType.StudentConnected, clientSummary);

            // 通知学生机注册成功，保存消息记录
            var receivers2 = new List<MessageReceiver>
            {
                new MessageReceiver(){ConnectionId=Context.ConnectionId}
            };
            var messageSummary2 = _messageHistory.SaveMessage(Context.ConnectionId, receivers2, "RegisterSuccess", "RegisterSuccess", registerInfo);

            clientSummary.MessageId = messageSummary2.Id;
            await Clients.Caller.RegisterSuccess(clientSummary);
        }
    }
}
