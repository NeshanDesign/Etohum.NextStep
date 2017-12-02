using System;
using System.IO;
using System.Text;
using Etohum.NextStep.Common.Convertor;
using Etohum.NextStep.Common.Dto;
using Msmq = System.Messaging;
using Etohum.NextStep.Common.Utils;

namespace Etohum.NextStep.MQ
{
    public interface IQueueProvider
    {
        bool Suscribe(SubscriberInfo subscriber);
        bool Unsubscribe(UnSubscriberInfo unSubscriber);
    }

    public class MsmqProvider : IQueueProvider
    {
        /// <summary>
        /// it is something like a template method, if it gets more complicated this design will be more usefull than this simple design
        /// </summary>
        abstract class QueueFactory
        {
            /// <summary>
            /// only path of queues is different, every thing is the same
            /// </summary>
            public abstract string QueuePath { get; }
            public bool SendMessage<T>(T msgBody)
            {
                try
                {
                    using (var queue = new Msmq.MessageQueue(QueuePath))
                    {
                        var message = new Msmq.Message();
                        var obj = JsonConvertor.SerializeObject(msgBody);// we use json for more optimized queue size, default is xml
                        message.BodyStream = new MemoryStream(Encoding.UTF8.GetBytes(obj)); // convert json to array of bytes & put it into memorystream for using in queue
                        queue.Send(message);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Error(ex);
                }
                return false; // if it fails in any reason, the result is false
            }
        }

        private class SubscriberImpl : QueueFactory
        {
            public override string QueuePath => @".\private$\etohum.subscribe";
        }
        private class UnSubscriberImpl : QueueFactory
        {
            public override string QueuePath => @".\private$\etohum.unsubscribe";
        }

        public bool Suscribe(SubscriberInfo subscriber)
        {
            return new SubscriberImpl().SendMessage(subscriber);
        }
        public bool Unsubscribe(UnSubscriberInfo unSubscriber)
        {
            return new UnSubscriberImpl().SendMessage(unSubscriber);
        }
    }
}
