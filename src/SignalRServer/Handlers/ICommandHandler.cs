using SignalRServer.Dtos;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SignalRServer.Handlers
{
    /// <summary>
    /// 指令处理接口程序
    /// @Created by Leo 2020/7/13 20:07:00
    /// </summary>
    public interface ICommandHandler
    {
        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="data">传入数据</param>
        Task Process(HubCallerContext Context, string data);
    }
}
