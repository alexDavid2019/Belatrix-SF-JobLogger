using JobSimpleLogger.Enums;
using JobSimpleLogger.Utils;
using System;
using System.Configuration;


namespace JobSimpleLogger
{
    class Program
    {
        //private static readonly AsyncLogger mngLogger = new AsyncLogger( new MessageBuilder(), LogType.FILE, 100);
        private static readonly SimpleLogger mngLogger = new SimpleLogger(LogType.FILE);

        static void Main(string[] args)
        {
            string message = "Test Message 4";

            mngLogger.LogMessage(message);

            mngLogger.Log(new LogEventInfo(message, SeverityLevel.WARNING));

            Console.WriteLine();

        }
    }
}
