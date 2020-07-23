using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SignalRServer.Dtos;
using SignalRServer.Hubs;
using SignalRServer.Models;
using SignalRServer.Storage;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace SignalRServer.Handlers
{
    /// <summary>
    /// AllowLogin 命令处理
    /// @Created by Leo 2020/7/21 17:33:29
    /// </summary>
    [InjectNamed(CommandType.AllowLogin)]
    public class AllowLoginHandler : ICommandHandler
    {
        // hub 上下文
        private readonly IHubContext<ProxyHub, ISignalrClient> _hubContext;
        private readonly ClientStorage _clientStorage;
        private readonly ILogger<StartHandler> _logger;
        public AllowLoginHandler(IHubContext<ProxyHub, ISignalrClient> hubCotnext, ClientStorage clientStorage, ILogger<StartHandler> logger)
        {
            this._hubContext = hubCotnext;
            this._clientStorage = clientStorage;
            this._logger = logger;
        }

        /// <summary>
        /// 消息处理                                         
        /// </summary>
        /// <param name="context"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task Process(HubCallerContext context, string jsondata)
        {
            var param = JsonConvert.DeserializeObject<AllowLoginCmdParams>(jsondata);

            var teacherInfo = _clientStorage.TeacherClients.FirstOrDefault(p => p.ConnectionId == context.ConnectionId);

            if (teacherInfo == null || !teacherInfo.IsConnected)
            {
                throw new Exception("教师机连接状态异常！");
            }
            await _hubContext.Clients.Group(teacherInfo.TeacherCode).ReceiveMessage(CommandType.AllowLogin, param);
        }
    }
}
