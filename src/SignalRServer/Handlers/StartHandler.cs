using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SignalRServer.Dtos;
using SignalRServer.Hubs;
using SignalRServer.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace SignalRServer.Handlers
{
    /// <summary>
    /// Start 命令处理
    /// @Created by Leo 2020/7/13 20:17:48
    /// </summary>
    [InjectNamed(CommandType.Start)]
    public class StartHandler : ICommandHandler
    {
        // hub 上下文
        private readonly IHubContext<ProxyHub, ISignalrClient> _hubContext;
        private readonly ILogger<StartHandler> _logger;
        public StartHandler(IHubContext<ProxyHub, ISignalrClient> hubCotnext, ILogger<StartHandler> logger)
        {
            _hubContext = hubCotnext;
            _logger = logger;
        }

        /// <summary>
        /// 消息处理                                         
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="teacherCode"></param>
        /// <returns></returns>
        public async Task Process(HubCallerContext Context, string jsondata)
        {
            var param = JsonConvert.DeserializeObject<StartCmdParams>(jsondata);
            await _hubContext.Clients.Group(param.TeacherCode).ReceiveMessage(CommandType.Start, param);
        }
    }
}
