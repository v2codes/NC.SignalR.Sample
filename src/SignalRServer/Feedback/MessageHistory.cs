using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRServer.Feedback
{
    /// <summary>
    /// 消息记录
    /// @Created by Leo 2020/7/13 18:06:13
    /// </summary>
    public class MessageHistory
    {
        /// <summary>
        /// 消息记录
        /// </summary>
        public ConcurrentBag<MessageSummary> MessageRecords { get; private set; }

        public MessageHistory()
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
        public MessageSummary SaveMessage(string sender, List<MessageReceiver> receivers, string eventType, string commandType, object data)
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
        public async Task Feedback(Guid messageId, string receiverConnectionId)
        {
            await Task.Run(() =>
            {
                var message = MessageRecords.FirstOrDefault(p => p.Id == messageId);
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
                    message.FeedbackState = true;
                }
                message.FeedbackTime = DateTime.Now;
            });
        }
    }
}
