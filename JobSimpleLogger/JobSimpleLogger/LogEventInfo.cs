using JobSimpleLogger.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobSimpleLogger
{
    public class LogEventInfo
    {
        public SeverityLevel SeverityLevel { get; }

        public LogEventInfo(string logEvent, SeverityLevel level)
        {
            LogMessageEvent = logEvent;
            SeverityLevel = level;
        }
               
        public string LogMessageEvent { get; }
     }

}
