using System;
using System.Collections.Generic;
using System.Text;

namespace JobSimpleLogger.Exceptions
{
    public class LoggerException : Exception
    {
        public LoggerException(string message) : base(message) { }
    }
}
