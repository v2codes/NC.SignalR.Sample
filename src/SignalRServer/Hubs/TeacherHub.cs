using Microsoft.AspNetCore.SignalR;
using SignalRServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRServer.Hubs
{
    /// <summary>
    /// 教师指令hub
    /// </summary>
    public partial class ChatHub : Hub<ISignalrClient>
    {

        ///// <summary>
        ///// 接受客户端消息
        ///// </summary>
        ///// <param name="commandType"></param>
        ///// <param name="teacherCode"></param>
        ///// <returns></returns>
        //public async Task SendMessage(string commandType, string teacherCode)
        //{
        //    //WriteMessage(commandType, teacherCode);
        //    //MessageHistory.SaveMessage(Context.ConnectionId, teacherCode);
        //    switch (commandType)
        //    {
        //        case CommandType.Start:
        //        case CommandType.Stop:
        //            {
        //                await Clients.Group(teacherCode).SendToReceiveMessage(commandType, teacherCode);
        //                break;
        //            }
        //        case CommandType.Pause:
        //            {
        //                await Clients.Group(teacherCode).SendToReceiveMessage(commandType, teacherCode);
        //                break;
        //            }
        //        default:
        //            throw new ArgumentException("c");
        //    }
        //    await Clients.Group(teacherCode).SendAsync("ReceiveMessage", commandType, teacherCode);

        //    //await Clients.All.SendAsync("ReceiveMessage", commandType, teacherCode);
        //}

        private async Task SendToPart(string commandType, List<string> connectionIds)
        {
            //await Clients.Clients(connectionIds).SendAsync("ReceiveMessage", commandType);
            await Clients.Clients(connectionIds).ReceiveMessage(commandType, null);
        }

        private async Task SendToUser(string commandType, string connectionId)
        {
            //await Clients.Client(connectionId).SendAsync(EventType.ReceiveMessage, commandType);
            await Clients.Client(connectionId).ReceiveMessage(commandType, null);
        }


    }
}
