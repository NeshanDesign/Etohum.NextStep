using System;
using System.IO;
using Etohum.NextStep.Common.Convertor;
using Etohum.NextStep.Common.Dto;

namespace Etohom.NextStep.MqHandler
{
    public interface IQueueHandler
    {
        void Subscribe();
        void Unsubscribe();
    }

    public class MsmqHandler : IQueueHandler
    {
        public void Subscribe()
        {
            DoJob(true);
        }

        public void Unsubscribe()
        {
            DoJob(false);
        }

        private static void DoJob(bool isSubscription)
        {
            try
            {
                while (true)
                {
                    var dto = isSubscription
                        ? ReadMessage<SubscriberInfo>(@".\private$\etohum.subscribe", ConsoleColor.Cyan)
                        : ReadMessage<UnSubscriberInfo>(@".\private$\etohum.unsubscribe", ConsoleColor.Yellow);
                    if (dto != null)
                    {
                        if (isSubscription) new SubscriptionWorkflow().Subscribe((SubscriberInfo)dto);
                        else new SubscriptionWorkflow().Unsubscribe(dto);
                    }
                }
            }
            catch (Exception ex)
            {
                ConsoleHelper.DoConsoleJob(ConsoleColor.Red, () =>
                {
                    Console.WriteLine(ex.Message + "\n\t" + ex.StackTrace);
                });
            }
        }
        private static T ReadMessage<T>(string queueAddress, ConsoleColor color = ConsoleColor.Gray) where T : UnSubscriberInfo
        {
            T result = default(T);
            using (var mq = new System.Messaging.MessageQueue(queueAddress))
            {
                ConsoleHelper.WriteLine(color, $"Waiting for new {(typeof(T) == typeof(SubscriberInfo) ? "Subscriber" : "Unsubscriber")}...");

                var message = mq.Receive();
                if (message != null)
                {
                    var msgBodyStream = new StreamReader(message.BodyStream);
                    var msgBody = JsonConvertor.ReadObject<T>(msgBodyStream);

                    if (typeof(T) == typeof(SubscriberInfo))
                    {
                        var subscriber = (SubscriberInfo)(object)msgBody;// dirty casting
                        ConsoleHelper.WriteLine(color, "New Subscriber with below info:");
                        ConsoleHelper.WriteLine(color, $"\t\tFirstName: {subscriber.FirstName}");
                        ConsoleHelper.WriteLine(color, $"\t\tLastName: {subscriber.LastName}");
                        ConsoleHelper.WriteLine(color, $"\t\tEmailAddress: {subscriber.EmailAddress}");
                    }
                    else
                    {
                        var unsubscriber = (UnSubscriberInfo)msgBody;
                        ConsoleHelper.WriteLine(color, "New UnSubscriber with below info:");
                        ConsoleHelper.WriteLine(color, $"\t\tEmailAddress: {unsubscriber.EmailAddress}");
                    }

                    result = msgBody;
                }
            }
            return result;
        }
    }
}