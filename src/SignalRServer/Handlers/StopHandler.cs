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
    /// Stop 命令处理
    /// @Created by Leo 2020/7/14 10:51:10
    /// </summary>
    [InjectNamed(CommandType.Stop)]
    public class StopHandler : ICommandHandler
    {
        private readonly IHubContext<ProxyHub, ISignalrClient> _hubContext;
        private readonly ILogger<StartHandler> _logger;
        public StopHandler(IHubContext<ProxyHub, ISignalrClient> hubCotnext, ILogger<StartHandler> logger)
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
        public async Task Process(HubCallerContext Context, string data)
        {
            var param = JsonConvert.DeserializeObject<StopCmdParams>(data);
            await _hubContext.Clients.Group(param.TeacherCode).ReceiveMessage(CommandType.Stop, param);
        }
    }
}
