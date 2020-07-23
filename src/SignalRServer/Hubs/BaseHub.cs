using System;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Autofac;
using SignalRServer.Dtos;
using SignalRServer.Models;
using SignalRServer.Benchmark;
using SignalRServer.Feedback;
using SignalRServer.Handlers;
using SignalRServer.Storage;

namespace SignalRServer.Hubs
{
    /// <summary>
    /// 基础消息处理
    /// @Created by Leo 2020/7/13 17:13:43
    /// </summary>
    public partial class ProxyHub : Hub<ISignalrClient>
    {
        private readonly ConnectionCounter _counter;
        private readonly ClientStorage _clientStorage;
        private readonly MessageHistory _messageHistory;
        private readonly IComponentContext _componentContext;
        private readonly ILogger<ProxyHub> _logger;

        public ProxyHub(ConnectionCounter counter,
                        ClientStorage clientStorage,
                        MessageHistory messageHistory,
                        IComponentContext context,
                        ILogger<ProxyHub> logger)
        {
            _counter = counter;
            _clientStorage = clientStorage;
            _messageHistory = messageHistory;
            _componentContext = context;
            _logger = logger;
        }

        /// <summary>
        /// 接收客户端消息
        /// 根据不同 commandType 获取不同命令处理程序实例，执行进一步处理
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="jsondata"></param>
        /// <returns></returns>
        public async Task SendMessage(string commandType, string jsondata)
        {
            var _commandHandler = _componentContext.ResolveNamed<ICommandHandler>(commandType);
            await _commandHandler.Process(Context, jsondata);
        }

        /// <summary>
        /// 客户端接入回调
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            _counter.Connected();
            // TODO，连接成功后，注册之前，要不要处理？？
            _clientStorage.UpdateConnectedState(Context.ConnectionId, true);
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

            _clientStorage.UpdateConnectedState(Context.ConnectionId, false);

            return base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// 消息接收反馈
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public async Task Feedback(Guid messageId)
        {
            Thread.Sleep(5000);
            await _messageHistory.Feedback(messageId, Context.ConnectionId);
        }

        /// <summary>
        /// 用于负载测试
        /// </summary>
        /// <param name="payload"></param>
        public void SendPayload(string payload)
        {
            _counter.Receive(payload);
        }

        ///// <summary>
        ///// 监听当前连接情况  // TODO
        ///// </summary>
        //private void MonitorConnections(ClientStorage storage)
        //{
        //    Task.Run(() =>
        //    {
        //        while (true)
        //        {
        //            var stuCount = storage.StudentList.Values.Select(p => p.Count).Sum();
        //            Console.WriteLine($"Connections：教师机数量:{storage.TeacherList.Count} 学生机数量 {stuCount}");
        //            Thread.Sleep(1000);
        //        }
        //    });
        //}
    }
}
