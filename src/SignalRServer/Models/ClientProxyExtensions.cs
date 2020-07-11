using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRServer.Models
{
    /// <summary>
    /// IClientProxy扩展方法
    /// </summary>
    public static class IClientProxyExtensions
    {
        /// <summary>
        /// 扩展方法
        /// 发送 ReceiveMessage 事件消息
        /// </summary>
        /// <param name="clients"></param>
        /// <param name="commandType"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task SendToReceiveMessage(this IClientProxy clients, string commandType, object data)
        {
            await clients.SendAsync(EventType.ReceiveMessage, commandType, data);
        }

        /// <summary>
        /// 发送消息并保存消息记录
        /// </summary>
        /// <param name="Clients"></param>
        /// <param name="commandType"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task SendAndSaveAsync(this IClientProxy Clients,string sender, string commandType, object data)
        {
            //MessageHistory.SaveMessage(sender,, commandType, data);
            await Clients.SendAsync(EventType.ReceiveMessage, commandType, data);
        }
    }
}
