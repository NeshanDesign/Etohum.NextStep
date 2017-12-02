using System;
using System.Threading;
using System.Threading.Tasks;
using Etohum.NextStep.Common.Dto;

namespace Etohom.NextStep.MqHandler
{
    public class EmailHelper
    {
        public static EmailMessage CreateMessage(SubscriberInfo info, string emailAddress, bool isSubscription, bool alreadyDone = false) 
        {
            if (isSubscription)
            {
                return new EmailMessage()// i dont care about null pointer here!
                {
                    EmailAddress = info.EmailAddress,
                    Subject = "New Subscription",
                    Body = !alreadyDone
                        ? $"Hello \"{info.FirstName} {info.LastName}\"\tYour subscription is done successfully.we added you to our mailing list."
                        : $"Hello \"{info.FirstName} {info.LastName}\"\tYou have been already added to our mailing list."
                };
            }
            else
            {
                return new EmailMessage()
                {
                    EmailAddress = info.EmailAddress,
                    Subject = "UnSubscription",
                    Body = !alreadyDone
                        ? $"dear:{info.FirstName} {info.LastName}\n\tIf your are determined to unsubscribe please confirm <here>"
                        : $"dear:{emailAddress}\n\tyou have not subscribed to our system, if you still receive from us please contact administrator."
                };
            }
        }

        public static async void SendMessageAsync(EmailMessage message)
        {
            await Task.Run(() =>
            {
                // wait for one second, simply it is a small chuunk of time for handshaking
                Thread.Sleep(1000); 
                ConsoleHelper.DoConsoleJob(ConsoleColor.Green, () =>
                {
                    Console.WriteLine(message);
                });
            });
        }
    }
}