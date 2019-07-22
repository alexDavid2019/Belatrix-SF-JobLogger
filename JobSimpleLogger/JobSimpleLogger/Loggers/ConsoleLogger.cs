using JobSimpleLogger.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobSimpleLogger.Loggers
{
    internal class ConsoleLogger : ILogger
    {
        public void LogMessage(string message, SeverityLevel severityLevel)
        {
            switch (severityLevel)
            {
                case SeverityLevel.FATAL:
                case SeverityLevel.ERROR:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case SeverityLevel.WARNING:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case SeverityLevel.DEBUG:
                case SeverityLevel.TRACE:
                case SeverityLevel.INFO:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                default:
                    return;
            }

            Console.WriteLine( message);

            //Sets the foreground and background console colors to their defaults.
            Console.ResetColor();
        }
    }
}
