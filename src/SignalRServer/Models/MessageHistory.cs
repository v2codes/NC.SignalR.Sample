using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRServer.Models
{
    /// <summary>
    /// 消息历史
    /// TODO：持久化
    /// </summary>
    public class MessageHistory
    {

        /// <summary>
        /// 消息记录
        /// </summary>
        public static ConcurrentBag<MessageSummary> MessageRecords { get; private set; }

        static MessageHistory()
        {
            MessageRecords = new ConcurrentBag<MessageSummary>();
        }

        /// <summary>
        /// 保存消息记录
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="commandType"></param>
        /// <param name="sender"></param>
        /// <param name="receivers"></param>
        /// <param name="data"></param>
        public static MessageSummary SaveMessage(string sender, List<ReceiverSummary> receivers, string eventType, string commandType, object data)
        {
            var message = new MessageSummary()
            {
                EventType = eventType,
                CommandType = commandType,
                Sender = sender,
                Receivers = receivers,
                Data = data
            };

            MessageRecords.Add(message);

            return message;
        }

        /// <summary>
        /// 消息反馈状态更新
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="receiverConnectionId"></param>
        /// <returns></returns>
        public static async Task Feedback(Guid messageId, string receiverConnectionId)
        {
            await Task.Run(() =>
            {
                var message = MessageRecords.FirstOrDefault(p => p.MessageId == messageId);
                if (message == null)
                {
                    throw new Exception($"未找到消息记录：[id={messageId}]");
                }
                var receiver = message.Receivers.FirstOrDefault(p => p.ConnectionId == receiverConnectionId);
                if (receiver == null)
                {
                    throw new Exception($"未找到消息接收者记录：[ConnectionId={receiverConnectionId}]");
                }

                receiver.State = true;
                receiver.FeedbackTime = DateTime.Now;

                var noFeedback = message.Receivers.Any(p => p.State != true);
                if (!noFeedback)
                {
                    message.State = true;
                }
                message.FeedbackTime = DateTime.Now;
            });
        }
    }
}
