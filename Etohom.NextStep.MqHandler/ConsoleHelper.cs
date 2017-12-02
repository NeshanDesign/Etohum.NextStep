using System;

namespace Etohom.NextStep.MqHandler
{
    public class ConsoleHelper
    {
        private static readonly object Locker = new object();


        public static void DoConsoleJob(ConsoleColor color, Action action)
        {
            lock (Locker)
            {
                Console.ForegroundColor = color;
                action();
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        public static void WriteLine(ConsoleColor color, string text)
        {
            lock (Locker)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(text);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
    }
}