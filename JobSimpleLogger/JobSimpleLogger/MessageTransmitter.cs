using JobSimpleLogger.Enums;
using JobSimpleLogger.Loggers;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace JobSimpleLogger
{
    internal class MessageTransmitter
    {
        protected static readonly TimeSpan ZeroSeconds = TimeSpan.FromSeconds(0);

        private volatile bool neverCalledInit;
        private volatile bool isReady;
        private readonly TimeSpan newInitDelay;
        private ILogger logger;
        private LogType logType;

        public MessageTransmitter(LogType type)
        {
            neverCalledInit = true;
            isReady = false;
            logType = type;
            SetInstanceLogger();
        }
        
        private void SetInstanceLogger()
        {
            switch (logType)
            {
                case LogType.CONSOLE:
                    logger = new ConsoleLogger();
                    break;
                case LogType.FILE:
                    logger = new FileLogger();
                    break;
                case LogType.DATABASE:
                    logger = new DataBaseLogger();
                    break;
                default:
                    break;
            }
        }

        public Task SendMessageAsync(Dictionary<SeverityLevel, string> logEntries, CancellationToken token)
        {
            if (token.IsCancellationRequested)
                return Task.FromResult<object>(null);

            return PrepareForSendAsync(token)
                .Then(_ => SendAsync(logEntries, token), token)
                .Unwrap()
                .ContinueWith(t =>
                {
                    if (t.Exception == null) // t.IsFaulted is false
                        return Task.FromResult<object>(null);

                    Console.Error.WriteLine(t.Exception?.GetBaseException().StackTrace, "SendAsync failed");

                    TidyUp();

                    return SendMessageAsync(logEntries, token); // Failures impact on the log entry queue
                }, token, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Current).Unwrap();
        }

        public void Dispose()
        {
            TidyUp();
        }

        private Task PrepareForSendAsync(CancellationToken token)
        {
            if (isReady)
                return Task.FromResult<object>(null);

            var delay = neverCalledInit ? ZeroSeconds : newInitDelay;

            neverCalledInit = false;

            return Task
                .Delay(delay, token)
                .Then(_ => Init(), token)
                .Unwrap()
                .Then(_ => isReady = true, token);
        }

        private void TidyUp()
        {
            try
            {
                if (isReady)
                    Terminate();
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine(exception.StackTrace, "Terminate failed");
            }
            finally
            {
                isReady = false;
            }
        }

        private Task Init()
        {
            return Task.FromResult<object>(null);
        }

        private Task SendAsync(Dictionary<SeverityLevel, string> logEntries, CancellationToken token)
        {
            if (token != null && token.IsCancellationRequested)
                return Task.FromResult<object>(null);

            foreach (KeyValuePair<SeverityLevel, string> entry in logEntries)
            {
                logger.LogMessage( entry.Value, entry.Key);
            }
            return Task.FromResult<String>("OK");
        }

        private void Terminate()
        {
            
        }
    }

}
