using System;
using System.Collections.Generic;
using System.Text;

namespace SignalRServer.Feedback
{
    /// <summary>
    /// 接收消息客户端信息
    /// @Created by Leo 2020/7/13 18:11:25
    /// </summary>
    public class MessageReceiver
    {
        /// <summary>
        /// 连接ID
        /// </summary>
        public string ConnectionId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public bool State { get; set; }

        /// <summary>
        /// 消息反馈时间
        /// </summary>
        public DateTimeOffset? FeedbackTime { get; set; }

        public MessageReceiver()
        {
            this.State = false;
        }
    }
}
