using System;
using System.Dynamic;
using System.Threading.Tasks;
using Etohom.NextStep.MqHandler.Mapping;

namespace Etohom.NextStep.MqHandler
{
    class Program
    {
        static void Main(string[] args)
        {
            // Bootstraping & Initializing the app, do nothing before this lines
            AutoMapperConfiguration.Configure();
            // end of Boostraping

            // you can use an IOC to inject specific impl, here we have just one impl of the interface
            // for msmq, you can implement it for wbsphere or any other queue systems
            IQueueHandler queueHandler = new MsmqHandler(); 
            do
            {
                // we create 2 threads to handle each queue through its own context
                Task.Run(() => queueHandler.Subscribe());
                Task.Run(() => queueHandler.Unsubscribe());
            } while (Console.ReadLine().ToLower() != "exit"); 
            // threads might stay alive in OS on app exit, cancellation token needs to be set
        }
    }
}
