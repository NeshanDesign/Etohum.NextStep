using Etohum.NextStep.Common.Dto;
using Etohum.NextStep.Common.Utils;
using Etohum.NextStep.Data.Model;

namespace Etohom.NextStep.MqHandler
{
    public class SubscriptionWorkflow
    {
        private readonly SubscriberDbLogic _db;

        public SubscriptionWorkflow()
        {
            _db = new SubscriberDbLogic();
        }
        public void Subscribe(SubscriberInfo subscriberInfo)
        {
            // check if they have alraedy subscribed or not
            // if not save subscriber  to db
            // send the subscriber a notification email

            var subscriber = _db.GetSubscriber(subscriberInfo.EmailAddress);
            _db.CreateSubscriber(subscriberInfo, subscriber);
            var message = EmailHelper.CreateMessage(subscriberInfo,"", true, subscriber != null);
            EmailHelper.SendMessageAsync(message);
        }

        public void Unsubscribe(UnSubscriberInfo unSubscriberInfo)
        {
            // check if they have alraedy subscribed or not
            // if yes, update subscriber to unsubscriber list  in db
            // send the unsubscriber a confirmation email request

            var subscriber = _db.GetSubscriber(unSubscriberInfo.EmailAddress);
            var alreadyUnSubscribedOrNotExist = (subscriber != null && subscriber.IsRemoved) || subscriber == null;
            _db.UnSubscribe(unSubscriberInfo, subscriber);
            var subscriberInfo = subscriber == null ? new SubscriberInfo() : subscriber.Get<Subscriber, SubscriberInfo>();
            var message = EmailHelper.CreateMessage(subscriberInfo, unSubscriberInfo.EmailAddress, false, alreadyUnSubscribedOrNotExist);
            EmailHelper.SendMessageAsync(message);
        }
    }
}