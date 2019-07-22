using JobSimpleLogger.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobSimpleLogger
{
    public interface ILogger
    {
        void LogMessage(string message, SeverityLevel severityLevel);
    }
}
