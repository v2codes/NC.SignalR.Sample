using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using SignalRServer.Hubs;
using SignalRServer.Models;
using SignalRServer.Storage;

namespace SignalRServer.Feedback
{
    /// <summary>
    /// 消息反馈监听服务
    /// @Created by Leo 2020/7/14 16:14:25
    /// </summary>
    public class FeedbackMonitorService : IHostedService
    {
        // hub 上下文
        private readonly IHubContext<ProxyHub, ISignalrClient> _hubContext;
        // 消息历史记录
        private readonly MessageHistory _messageHistory;
        // 客户端信息
        private readonly ClientStorage _clientStorage;

        private readonly ILogger<FeedbackMonitorService> _logger;

        public FeedbackMonitorService(IHubContext<ProxyHub, ISignalrClient> hubContext,
                                      MessageHistory messageHistory,
                                      ClientStorage clientStorage,
                                      ILogger<FeedbackMonitorService> logger)
        {
            _hubContext = hubContext;
            _messageHistory = messageHistory;
            _clientStorage = clientStorage;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _ = Task.Run(async () =>
            {
                while (true)
                {
                    // 3秒超时，未收到有效接收反馈的消息记录
                    var timeoutMsgRecords = _messageHistory.MessageRecords.Where(p => !p.FeedbackState 
                                                                                   && !p.NoticeState 
                                                                                   && p.SendTime.AddSeconds(3) < DateTimeOffset.Now).ToList();
                    foreach (var messageSummary in timeoutMsgRecords)
                    {
                        // TODO通知发送者，消息超时？
                        await SendTimeout(messageSummary);
                        messageSummary.NoticeState = true;
                        _logger.LogWarning("超时未收到反馈：{0}", JsonConvert.SerializeObject(messageSummary));
                    }

                    Thread.Sleep(1000);
                }
            });
            await Task.FromResult(1);
        }

        /// <summary>
        /// 发送超时通知
        /// </summary>
        /// <param name="messageSummary"></param>
        /// <returns></returns>
        public async Task SendTimeout(MessageSummary messageSummary)//List<string> receiverConnectionIds)
        {
            // TODO ，找到教师机分组
            //var teacherGroup = 
            //var clients = _clientStorage.StudentList.Where(p=>receiverConnectionIds.Contains(p.Value))
            await _hubContext.Clients.Client(messageSummary.Sender).Timeout(messageSummary);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.FromResult(1);
        }
    }
}
