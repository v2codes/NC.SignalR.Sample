using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Autofac;
using SignalRServer.Dtos;
using SignalRServer.Handlers;
using SignalRServer.Models;
using SignalRServer.Feedback;
using Newtonsoft.Json;

namespace SignalRServer.Hubs
{
    /// <summary>
    /// 教师指令hub
    /// @Created by Leo 2020/7/13 19:59:30
    /// </summary>
    public partial class ProxyHub : Hub<ISignalrClient>
    {

        /// <summary>
        /// 开始考试指令
        /// </summary>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        public async Task Start(string jsondata)
        {
            ICommandHandler _commandHandler = _componentContext.ResolveNamed<ICommandHandler>("Start");
            await _commandHandler.Process(Context, jsondata);
        }

        /// <summary>
        /// 教师机注册
        /// TODO：教师机数量限制
        /// </summary>
        /// <param name="registerInfo">注册信息</param>
        /// <returns></returns>
        public async Task RegisterTeacher(RegisterCmdParams registerInfo)
        {
            if (registerInfo.Identity != IdentityType.Teacher)
            {
                _logger.LogError("注册信息 Identity 错误");
                throw new ArgumentException("注册信息 Identity 错误");
            }

            var ip = Context.GetHttpContext().Connection.RemoteIpAddress.ToString();
            var clientSummary = new TeacherClientSummary
            {
                Identity = registerInfo.Identity,
                ConnectionId = Context.ConnectionId,
                IsConnected = true,
                RemoteIpAddress = ip,
                TeacherCode = registerInfo.TeacherCode
            };

            // 保存教师机信息
            _clientStorage.TeacherClients.Add(clientSummary);

            // 创建教师机分组
            // await Groups.AddToGroupAsync(Context.ConnectionId, clientSummary.Code);

            // 保存消息记录、应答注册成功
            var receivers = new List<MessageReceiver>
                {
                    new MessageReceiver(){ConnectionId=Context.ConnectionId}
                };
            var messageSummary = _messageHistory.SaveMessage(Context.ConnectionId, receivers, "RegisterSuccess", "RegisterSuccess", registerInfo);

            clientSummary.MessageId = messageSummary.Id;
            await Clients.Caller.RegisterSuccess(clientSummary);
        }
    }
}
