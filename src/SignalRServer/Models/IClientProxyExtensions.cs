using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SignalRServer.Models
{
    /// <summary>
    /// IClientProxy扩展
    /// @Created by Leo 2020/7/14 18:07:45
    /// </summary>
    public static class IClientProxyExtensions
    {
        /// <summary>
        /// 扩展方法，推送异常消息
        /// </summary>
        /// <param name="clients"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static async Task SendErrorMessage(this IClientProxy clients, string errorMessage)
        {
            await clients.SendAsync("ReceiveServerError", errorMessage);
        }
    }
}
