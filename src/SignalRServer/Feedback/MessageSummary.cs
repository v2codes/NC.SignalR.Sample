using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SignalRServer.Feedback
{
    /// <summary>
    /// 消息记录概要
    /// @Created by Leo 2020/7/13 18:07:38
    /// </summary>
    public class MessageSummary
    {
        /// <summary>
        /// 消息ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 事件类型（方法名）
        /// </summary>
        public string EventType { get; set; }

        /// <summary>
        /// 指令类型
        /// </summary>
        public string CommandType { get; set; }

        /// <summary>
        /// 发送方，记录ConnectionId
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// 接收方，记录ConnectionId
        /// </summary>
        public List<MessageReceiver> Receivers { get; set; }

        /// <summary>
        /// 输入/输出数据
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTimeOffset SendTime { get; set; }

        /// <summary>
        /// 反馈状态
        /// </summary>
        public bool FeedbackState { get; set; }

        /// <summary>
        /// 通知状态
        /// </summary>
        public bool NoticeState { get; set; }

        /// <summary>
        /// 消息反馈时间
        /// </summary>
        public DateTimeOffset? FeedbackTime { get; set; }

        public MessageSummary()
        {
            this.Id = Guid.NewGuid();
            this.SendTime = DateTimeOffset.Now;
            this.FeedbackState = false;
            this.NoticeState = false;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
