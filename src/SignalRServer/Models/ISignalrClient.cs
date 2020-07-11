using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRServer.Models
{
    /// <summary>
    /// 强类型Hub，定义客户端方法名
    /// </summary>
    public interface ISignalrClient
    {
        /// <summary>
        /// 发送注册成功消息
        /// </summary>
        /// <param name="clientSummary">注册信息</param>
        /// <returns></returns>
        Task RegisterSuccess(ClientSummary clientSummary);

        ///// <summary>
        ///// 通知教师机学生机接入成功
        ///// </summary>
        ///// <param name="command">指令</param>
        ///// <param name="data">数据</param>
        ///// <returns></returns>
        //Task StudentConnected(string command, object data);

        /// <summary>
        /// 发送普通消息
        /// </summary>
        /// <param name="command">指令</param>
        /// <param name="data">数据</param>
        /// <returns></returns>
        Task ReceiveMessage(string command, object data);

    }
}
