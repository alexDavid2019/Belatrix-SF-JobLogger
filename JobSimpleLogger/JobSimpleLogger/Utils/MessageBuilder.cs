using JobSimpleLogger.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace JobSimpleLogger.Utils
{
    public class MessageBuilder
    {
        private const string TimestampFormat = "{0:yyyy-MM-ddTHH:mm:ss.ffffffK}";
        private static readonly byte[] SpaceBytes = { 0x20 };

        public Dictionary<SeverityLevel, string> BuildLogEntries(string logEvent, SeverityLevel severityLevel)
        {
            Dictionary<SeverityLevel, string> result = new Dictionary<SeverityLevel, string>();
            StringBuilder buffer = new StringBuilder();
            try
            {
                var timestamp = string.Format(CultureInfo.InvariantCulture, TimestampFormat, DateTime.Now);
                buffer.Append(String.Format("{0} ({1}) - {2}", timestamp, severityLevel, logEvent));
                if (!result.ContainsKey(severityLevel))
                {
                    result.Add(severityLevel, buffer.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
            return result;
        }

    }
}
