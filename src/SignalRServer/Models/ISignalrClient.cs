using SignalRServer.Dtos;
using SignalRServer.Feedback;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SignalRServer.Models
{
    /// <summary>
    /// 强类型Hub，定义客户端方法名
    /// @Created by Leo 2020/7/13 16:54:33
    /// </summary>
    public interface ISignalrClient
    {
        /// <summary>
        /// 发送注册成功消息
        /// </summary>
        /// <param name="clientSummary">注册信息</param>
        /// <param name="messageId">通知消息ID</param>
        /// <returns></returns>
        Task RegisterSuccess(ClientSummary clientSummary);

        /// <summary>
        /// 发送普通消息
        /// </summary>
        /// <param name="commandType">指令</param>
        /// <param name="data">发送数据</param>
        /// <returns></returns>
        Task ReceiveMessage(string commandType, ICmdParams data);

        /// <summary>
        /// 发送异常信息
        /// </summary>
        /// <param name="errorMessage">异常信息</param>
        /// <returns></returns>
        Task ReceiveServerError(string errorMessage);

        /// <summary>
        /// 超时未反馈通知
        /// 发出指令消息后，服务端在规定时间内（默认3s）未收到接收方发送"Feedback"时，通知该指令发送方
        /// </summary>
        /// <param name="clients"></param>
        /// <returns></returns>
        Task Timeout(List<ClientSummary> clients);

        Task Timeout(MessageSummary messageSummary);

        /// <summary>
        /// 通知客户端主动断开连接
        /// </summary>
        /// <returns></returns>
        Task CloseConnection();
    }
}
