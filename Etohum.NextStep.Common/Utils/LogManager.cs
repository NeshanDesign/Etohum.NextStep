using System;
using System.Diagnostics;

namespace Etohum.NextStep.Common.Utils
{
    public enum LoggerLevel
    {
        None, Info, Warn, Debug, Error
    }
    public interface ILogger
    {
        void Log(string msg);
        void Log(string msg, LoggerLevel level);
        void Error(string msg);
        void Error(Exception exception);
        void Info(string msg);
        void Warn(string msg);
    }
    internal class Logger
    {
        private readonly ILogger _logger;
        public Logger(ILogger logger)
        {
            _logger = logger;
        }

        public void Log(string msg)
        {
            _logger.Log(msg);
        }

        public void Log(string msg, LoggerLevel level)
        {
            _logger.Log(msg, level);
        }

        public void Error(string msg)
        {
            _logger.Error(msg);
        }

        public void Error(Exception exception)
        {
            _logger.Error(exception);
        }

        public void Info(string msg)
        {
            _logger.Info(msg);
        }

        public void Warn(string msg)
        {
            _logger.Warn(msg);
        }
    }

    public class EventViewerLogger : ILogger
    {
        private readonly EventLog eventLogger;
        public EventViewerLogger()
        {
            if (!EventLog.SourceExists("Switch.Admin.Web.Daemon"))
            {
               EventLog.CreateEventSource("Switch.Admin.Web.Daemon", "DaemonLogger");
               throw new Exception("Creating new event log needs application to be closed and reopened after few seconds.");
            }
            eventLogger = new EventLog() { Source = "Switch.Admin.Web.Daemon" };
        }
        public void Log(string msg)
        {
            eventLogger.WriteEntry(msg);
        }

        public void Log(string msg, LoggerLevel level)
        {
            throw new NotImplementedException();
        }

        public void Error(string msg)
        {
            eventLogger.WriteEntry(msg, EventLogEntryType.Error);
        }

        public void Error(Exception exception)
        {
           this.Error(exception.FullTrace());
        }

        public void Info(string msg)
        {
            eventLogger.WriteEntry(msg, EventLogEntryType.Information);
        }

        public void Warn(string msg)
        {
            eventLogger.WriteEntry(msg, EventLogEntryType.Warning);
        }
    }

    public class NLogAsLogger : ILogger
    {
        private static Lazy<NLog.Logger> _logger;

        static NLogAsLogger()
        {
            _logger = new Lazy<NLog.Logger>(NLog.LogManager.GetCurrentClassLogger);
        }

        public void Log(string msg)
        {
            _logger.Value.Log(NLog.LogLevel.Debug, msg);
        }

        public void Log(string msg, LoggerLevel level)
        {
            _logger.Value.Log(NLog.LogLevel.Debug, msg);
        }

        public void Error(string msg)
        {
            _logger.Value.Error(msg);
        }

        public void Error(Exception exception)
        {
            _logger.Value.Error(exception);
        }

        public void Info(string msg)
        {
            _logger.Value.Info(msg);
        }

        public void Warn(string msg)
        {
            _logger.Value.Warn(msg);
        }
    }

    public class ConsoleAsLogger : ILogger
    {
        private static readonly object SyncObject = new object();
        public void Log(string msg)
        {
            Log(msg, LoggerLevel.Debug);
        }

        public void Log(string msg, LoggerLevel level)
        {
            ConsoleWriter($"[{DateTime.Now}]:[{level.ToString()}] {msg}", level);
        }

        public void Error(string msg)
        {
            ConsoleWriter($"[{DateTime.Now}]: [Error] {msg}", LoggerLevel.Error);
        }

        public void Error(Exception exception)
        {
            Error(exception.FullTrace());
        }

        public void Info(string msg)
        {
            ConsoleWriter(string.Format("[{0}]: [Info] {1}", DateTime.Now, msg), LoggerLevel.Info);
        }

        public void Warn(string msg)
        {
            ConsoleWriter(string.Format("[{0}]: [Warn] {1}", DateTime.Now, msg), LoggerLevel.Warn);
        }

        private static void ConsoleWriter(string msg, LoggerLevel level)
        {
            lock (SyncObject)
            {
                using (var writer = (LoggerLevel.Error == level ? Console.Error : Console.Out))
                {
                    switch (level)
                    {
                        case LoggerLevel.Error: Console.ForegroundColor = ConsoleColor.Red; break;
                        case LoggerLevel.Info: Console.ForegroundColor = ConsoleColor.Cyan; break;
                        case LoggerLevel.Warn:  Console.ForegroundColor = ConsoleColor.Yellow; break;
                        case LoggerLevel.Debug: Console.ForegroundColor = ConsoleColor.Gray;   break;
                        default:                Console.ForegroundColor = ConsoleColor.White;  break;
                    }
                    writer.WriteLine(msg);
                    writer.Flush();
                }
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
    public class LogManager
    {
        private static ILogger targetLogger;
        private readonly Logger _logger;
        static readonly Lazy<Logger> LazyLogger = new Lazy<Logger>(() => new LogManager(targetLogger)._logger);
        public static object LogLocker = new object();

        public static void Initializer(string target)
        {
            if ("file".Equals(target, StringComparison.InvariantCultureIgnoreCase)) targetLogger = new NLogAsLogger();
            else if ("event".Equals(target, StringComparison.InvariantCultureIgnoreCase)) targetLogger = new EventViewerLogger();
            else targetLogger = new ConsoleAsLogger();
        }
       
        public LogManager()
            : this(new ConsoleAsLogger()){ }

        public LogManager(ILogger targetLogger)
        {
            _logger = new Logger(targetLogger ?? new ConsoleAsLogger());
        }

        public static void Log(string msg)
        {

            lock (LogLocker)
            {
                LazyLogger.Value.Log(msg);
            }
        }

        public static void Info(string msg)
        {
            lock (LogLocker)
            {
                LazyLogger.Value.Info(msg);
            }
        }

        public static void Warn(string msg)
        {
            lock (LogLocker)
            {
                LazyLogger.Value.Warn(msg);
            }
        }
        public static void Error(string msg)
        {
            lock (LogLocker)
            {
                LazyLogger.Value.Error(msg);
            }
        }

        public static void Error(Exception exception)
        {
            lock (LogLocker)
            {
                LazyLogger.Value.Error(exception);
            }
        }
    }

}
