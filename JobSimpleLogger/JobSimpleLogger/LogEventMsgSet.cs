using JobSimpleLogger.Enums;
using JobSimpleLogger.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JobSimpleLogger
{
    internal class LogEventMsgSet
    {
        private LogEventInfo messageLogEvent;
        private readonly MessageBuilder messageBuilder;
        private readonly MessageTransmitter messageTransmitter;
        private int currentMessage;
        private Dictionary<SeverityLevel, string> logEntries;

        public LogEventMsgSet(LogEventInfo dataLogEvent, 
                                MessageBuilder messageBuilder, 
                                MessageTransmitter messageTransmitter)
        {
            this.messageLogEvent = dataLogEvent;
            this.messageBuilder = messageBuilder;
            this.messageTransmitter = messageTransmitter;
            currentMessage = 0;
        }

        public LogEventMsgSet Build()
        {
            logEntries = messageBuilder.BuildLogEntries(messageLogEvent.LogMessageEvent, messageLogEvent.SeverityLevel);
            return this;
        }

        public Task SendAsync(CancellationToken token)
        {
            return SendAsync(token, new TaskCompletionSource<object>());
        }

        private Task SendAsync(CancellationToken token, TaskCompletionSource<object> tcs)
        {
            if (token.IsCancellationRequested)
                return tcs.CanceledTask();

            
            try
            {
                messageTransmitter
                    .SendMessageAsync(logEntries, token)
                    .ContinueWith(t =>
                    {
                        if (t.IsCanceled)
                        {
                            tcs.SetCanceled();
                            return;
                        }
                        if (t.Exception != null)
                        {
                            Console.Error.WriteLine(t.Exception.GetBaseException());
                            tcs.SetException(t.Exception);
                            return;
                        }
                        SendAsync(token, tcs);
                    }, token, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Current);

                return tcs.Task;
            }
            catch (Exception exception)
            {
                return tcs.FailedTask(exception);
            }
        }

        public override string ToString()
        {
            return $"ToString: '{ messageLogEvent }'";
        }
    }

}
