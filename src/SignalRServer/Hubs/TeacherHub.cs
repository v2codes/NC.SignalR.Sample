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
    public partial class ChatHub : Hub
    {
        /// <summary>
        /// 开始考试
        /// </summary>
        /// <returns></returns>
        public async Task Start(string teacherCode)
        {
            _counter.Receive(teacherCode);
            await Clients.Group(teacherCode).SendAsync(EventType.Start,CommandType.Start, teacherCode);
        }

        /// <summary>
        /// 暂停考试
        /// </summary>
        /// <returns></returns>
        public async Task Pause(string teacherCode)
        {
            await Clients.Group(teacherCode).SendToReceiveMessage(CommandType.Pause, teacherCode);
        }

        /// <summary>
        /// 结束考试
        /// </summary>
        /// <returns></returns>
        public async Task Stop(string teacherCode)
        {
            await Clients.Group(teacherCode).SendToReceiveMessage(CommandType.Stop, teacherCode);
        }

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
            await Clients.Clients(connectionIds).SendAsync("ReceiveMessage", commandType);
        }

        private async Task SendToUser(string commandType, string connectionId)
        {
            await Clients.Client(connectionId).SendAsync(EventType.ReceiveMessage, commandType);
        }


    }
}
