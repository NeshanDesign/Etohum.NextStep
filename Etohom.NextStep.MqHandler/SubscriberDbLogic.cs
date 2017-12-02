using System;
using System.CodeDom;
using Etohum.NextStep.Common.Dto;
using Etohum.NextStep.Common.Utils;
using Etohum.NextStep.Data.Model;
using Etohum.NextStep.Data.Repositoies;

namespace Etohom.NextStep.MqHandler
{
    //Todo: complete it
    internal class SubscriberDbLogic
    {
        private RepositoryBase<int, Subscriber> subscriberRepository;
        private RepositoryBase<int, SubscriptionHistory> subHistoryRepository;

        public SubscriberDbLogic()
        {
            // it is the duty of IOC, don't instantiate such a way, it will break the SRP rule.
            subscriberRepository = new SubscribeRepository();
            subHistoryRepository = new SubscribeHistoryRepository();
        }

        public void CreateSubscriber(SubscriberInfo subscriberInfo, Subscriber subscriber)
        {
            if (subscriber == null)
            {
                var mappedSub = subscriberInfo.Get<SubscriberInfo, Subscriber>();
                subscriberRepository.Add(mappedSub);
                subscriberRepository.SaveChanges();
            }
            else if(subscriber.IsRemoved)
            {
                ToggleRemove(subscriber, false);
            }
        }

        public void UnSubscribe(UnSubscriberInfo unSubscriberInfo, Subscriber subscriber)
        {
            if (subscriber != null)
            {
                ToggleRemove(subscriber, true);
            }
        }

        private void ToggleRemove(Subscriber subscriber, bool remove)
        {
            subscriber.IsRemoved = remove;
            subscriber.ModifiedDate = DateTime.Now;
            subscriberRepository.Update(subscriber);

            subHistoryRepository.Add(new SubscriptionHistory()
            {
                SubscriberId = subscriber.Id,
                Action = remove ? SubscriptionAction.UnSubscribe : SubscriptionAction.Subscribe
            });

            subscriberRepository.SaveChanges();
            subHistoryRepository.SaveChanges();

        }

        public Subscriber GetSubscriber(string  email)
        {
            return subscriberRepository.First(pred => pred.EmailAddress.Equals(email, StringComparison.InvariantCultureIgnoreCase));
        }

    }
}