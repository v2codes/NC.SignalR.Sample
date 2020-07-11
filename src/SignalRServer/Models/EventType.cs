using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRServer.Models
{
    /// <summary>
    /// 前端消息
    /// </summary>
    public class EventType
    {
        /// <summary>
        /// 接收消息
        /// </summary>
        public const string ReceiveMessage = "ReceiveMessage";

        /// <summary>
        /// 注册成功
        /// </summary>
        public const string RegisterSuccess = "RegisterSuccess";

        /// <summary>
        /// 服务端异常信息
        /// </summary>
        public const string ServerErrorMessage = "ServerErrorMessage";

    }
}
