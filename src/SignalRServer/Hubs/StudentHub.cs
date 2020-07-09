using Microsoft.AspNetCore.SignalR;
using SignalRServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRServer.Hubs
{
    /// <summary>
    /// 学生指令hub
    /// </summary>
    public partial class ChatHub : Hub
    {
        /// <summary>
        /// 状态上报
        /// </summary>
        /// <returns></returns>
        public async Task Status(string teacherCode)
        {
            await Clients.Client(teacherCode).SendToReceiveMessage(CommandType.Status, Context.ConnectionId);
        }
    }
}
