using JobSimpleLogger.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JobSimpleLogger.Loggers
{
    internal class FileLogger : ILogger
    {
        public void LogMessage(string message, SeverityLevel severityLevel)
        {
            //string directory = System.Configuration.ConfigurationSettings.AppSettings.Get("LogFileDirectory");

            string directory = System.Configuration.ConfigurationManager.AppSettings["LogFileDirectory"];
            string fileName = System.Configuration.ConfigurationManager.AppSettings["LogFileName"];
            string fileNameDateFormat = System.Configuration.ConfigurationManager.AppSettings["LogFileNameDateFormat"];
            string lineformat = System.Configuration.ConfigurationManager.AppSettings["LogFileLineFormat"];

            string path = directory + fileName + DateTime.Now.ToString(fileNameDateFormat) + ".txt";
            string logMessage = string.Format(lineformat, DateTime.Now.ToString(), severityLevel, message);

            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path)) { }
            }

            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(logMessage);
            }

        }
    }
}
