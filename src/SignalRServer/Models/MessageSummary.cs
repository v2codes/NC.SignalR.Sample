using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRServer.Models
{
    /// <summary>
    /// 消息记录概要
    /// </summary>
    public class MessageSummary
    {

        /// <summary>
        /// 消息ID
        /// </summary>
        public Guid MessageId { get; set; }

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
        public List<ReceiverSummary> Receivers { get; set; }

        /// <summary>
        /// 输入/输出数据
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime? SendTime { get; set; }

        /// <summary>
        /// 反馈状态
        /// </summary>
        public bool? State { get; set; }

        /// <summary>
        /// 消息反馈时间
        /// </summary>
        public DateTime? FeedbackTime { get; set; }

        public MessageSummary()
        {
            this.MessageId = Guid.NewGuid();
            this.SendTime = DateTime.Now;
            this.State = false;
        }
    }

    /// <summary>                  
    /// 接收方信息
    /// </summary>
    public class ReceiverSummary
    {
        /// <summary>
        /// 连接ID
        /// </summary>
        public string ConnectionId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public bool? State { get; set; }

        /// <summary>
        /// 消息反馈时间
        /// </summary>
        public DateTime? FeedbackTime { get; set; }

        public ReceiverSummary()
        {
            this.State = false;
        }
    }
}
