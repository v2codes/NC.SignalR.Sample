using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SignalRServer.Dtos;
using SignalRServer.Hubs;
using SignalRServer.Models;
using SignalRServer.Storage;

namespace SignalRServer.Handlers
{
    /// <summary>
    /// Status 命令处理
    /// @Created by Leo 2020/7/14 18:46:34
    /// </summary>
    [InjectNamed(CommandType.Status)]
    public class StatusHandler : ICommandHandler
    {
        // hub 上下文
        private readonly IHubContext<ProxyHub, ISignalrClient> _hubContext;
        private readonly ClientStorage _storage;
        private readonly ILogger<StartHandler> _logger;
        public StatusHandler(IHubContext<ProxyHub, ISignalrClient> hubCotnext, ClientStorage storage, ILogger<StartHandler> logger)
        {
            _hubContext = hubCotnext;
            _storage = storage;
            _logger = logger;
        }

        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task Process(HubCallerContext Context, string data)
        {
            var connectionId = Context.ConnectionId;

            var param = JsonConvert.DeserializeObject<StatusCmdParams>(data);

            var teacher = _storage.TeacherClients.FirstOrDefault(p => p.TeacherCode == param.TeacherCode);
            await _hubContext.Clients.Client(teacher.ConnectionId).ReceiveMessage(CommandType.Status, param);
        }
    }
}
