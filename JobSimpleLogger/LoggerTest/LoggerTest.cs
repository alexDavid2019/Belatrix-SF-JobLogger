using System;
using JobSimpleLogger;
using JobSimpleLogger.Utils;
using JobSimpleLogger.Enums;
using LoggerTest.Utility;
using NUnit;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LoggerTests
{
    [TestClass]
    public class LoggerTest
    {

        [TestInitialize]
        public void TestInit()
        {
            ConsoleHelper.InitTestConsole();
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            ConsoleHelper.ResetTestConsole();
        }


        [TestMethod]
        public void ConsoleLogCreated()
        {
            string message = "Test Console Message 001";
            //AsyncLogger mngLogger = new AsyncLogger( new MessageBuilder(), LogType.CONSOLE, 100);
            SimpleLogger mngLogger = new SimpleLogger(LogType.CONSOLE);

            mngLogger.LogMessage(message);

            string consoleOutput = ConsoleHelper.GetConsoleOutput();

            Assert.IsTrue(consoleOutput.Contains(message));
        }
        
        [TestMethod]
        public void FileLogCreated()
        {
            //Delete Current Log File
            var fileHelper = new FileHelper();
            fileHelper.DeleteCurrentLogFile();

            string message = "Test File Message 001";
            //AsyncLogger mngLogger = new AsyncLogger(new MessageBuilder(), LogType.FILE, 100);
            SimpleLogger mngLogger = new SimpleLogger(LogType.FILE);

            mngLogger.Log(new LogEventInfo(message, SeverityLevel.ERROR));

            //Existe Log File?
            Assert.IsTrue(fileHelper.ExistLogFile());
        }

        [TestMethod]
        public void DataBaseLogCreated()
        {
            //Empty Log Table
            var dataHelper = new DataHelper();
            dataHelper.EmptyLogTable();

            string message = "Test DataBase Message 001";

            //AsyncLogger mngLogger = new AsyncLogger(new MessageBuilder(), LogType.DATABASE, 100);
            SimpleLogger mngLogger = new SimpleLogger(LogType.DATABASE);

            mngLogger.Log(new LogEventInfo(message, SeverityLevel.WARNING));

            //IsLogEmpty
            Assert.IsFalse(dataHelper.IsLogEmpty());
        }
        
    }

}
