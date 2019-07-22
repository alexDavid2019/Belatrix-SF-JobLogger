using JobSimpleLogger.Enums;
using JobSimpleLogger.Exceptions;
using JobSimpleLogger.Loggers;
using JobSimpleLogger.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JobSimpleLogger
{
    public class AsyncLogger
    {
        private readonly int timeout;
        private readonly CancellationTokenSource cts;
        private readonly CancellationToken token;
        private readonly BlockingCollection<LogEventInfo> queue;
        private readonly MessageTransmitter messageTransmitter;
        
        public AsyncLogger(MessageBuilder messageBuilder, LogType logType, int timeout)
        {
            this.messageTransmitter = new MessageTransmitter(logType);

            this.timeout = timeout;

            cts = new CancellationTokenSource();
            token = cts.Token;

            queue = new BlockingCollection<LogEventInfo>();

            Task.Run(() => ProcessQueueAsync(messageBuilder));

        }

        public void  LogMessage(string message)
        {
            Log(new LogEventInfo( message, SeverityLevel.INFO));
        }
        
        public void Log(LogEventInfo eventInfo)
        {
            Enqueue(eventInfo, timeout);
        }

        private void ProcessQueueAsync(MessageBuilder messageBuilder)
        {
            ProcessQueueAsync(messageBuilder, 
                new TaskCompletionSource<object>()).ContinueWith(t =>
                {
                    Console.Error.WriteLine(t.Exception?.GetBaseException());

                    ProcessQueueAsync(messageBuilder);

                }, token, TaskContinuationOptions.ExecuteSynchronously 
                | TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.Current);
        }

        private Task ProcessQueueAsync(MessageBuilder messageBuilder, TaskCompletionSource<object> tcs)
        {
            if (token.IsCancellationRequested)
                return tcs.CanceledTask();

            try
            {
                var messageEventInfo = queue.Take(token);
                                
                var logEventMsgSet = new LogEventMsgSet(messageEventInfo, 
                                                    messageBuilder, 
                                                    messageTransmitter);

                logEventMsgSet
                    .Build()
                    .SendAsync(token)
                    .ContinueWith(t =>
                    {
                        if (t.IsCanceled)
                        {
                            Console.Error.WriteLine("Task canceled");
                            tcs.SetCanceled();
                            return;
                        }

                        if (t.Exception != null) // t.IsFaulted is true
                            Console.Error.WriteLine(t.Exception.GetBaseException().StackTrace, "Task faulted");
                        else
                            Console.WriteLine("Successfully sent message '{0}'", logEventMsgSet);

                        ProcessQueueAsync(messageBuilder, tcs);

                    }, token, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Current);

                return tcs.Task;
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine(exception.Message);
                return tcs.FailedTask(exception);
            }
        }

        private void Enqueue(LogEventInfo messageEventInfo, int timeout)
        {
            if (queue.TryAdd(messageEventInfo, timeout, token))
            {
                Console.WriteLine("Item added");
            }
            else
            {
                Console.WriteLine("Item does not added");
            }
        }

        public void Dispose()
        {
            cts.Cancel();
            cts.Dispose();
            queue.Dispose();
            messageTransmitter.Dispose();
        }

    }
}
