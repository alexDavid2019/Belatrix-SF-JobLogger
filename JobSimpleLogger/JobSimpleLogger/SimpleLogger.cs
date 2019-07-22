using JobSimpleLogger.Enums;
using JobSimpleLogger.Loggers;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobSimpleLogger
{
    public class SimpleLogger
    {
        private LogType logType;
        private SeverityLevel severityLevel;
        private ILogger logger;

        public SimpleLogger(LogType logType)
        {
            this.logType = logType;
            SetLogType();
        }

        private void SetLogType()
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

        public void LogMessage(string message)
        {
            if (message != null)
            {
                message.Trim();
                if (message.Length == 0)
                {
                    return;
                }
            }
            else
            {
                return;
            }
            Log(new LogEventInfo(message, SeverityLevel.INFO));

        }


        public void Log(LogEventInfo eventInfo)
        {
            logger.LogMessage(eventInfo.LogMessageEvent, eventInfo.SeverityLevel);
        }

    }
}
