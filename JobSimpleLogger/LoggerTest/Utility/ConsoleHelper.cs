using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LoggerTest.Utility
{
    public class ConsoleHelper
    {
        private static TextWriter defaultConsoleOut;
        private static TextWriter stringWriter;
        private static StringBuilder consoleOutput;

        public static void InitTestConsole()
        {
            defaultConsoleOut = Console.Out;
            consoleOutput = new StringBuilder();
            stringWriter = new StringWriter(consoleOutput);
            Console.SetOut(stringWriter);
        }

        public static void ResetTestConsole()
        {
            stringWriter.Flush();
            consoleOutput.Clear();
            Console.Clear();

            stringWriter.Dispose();
            Console.SetOut(defaultConsoleOut);
        }

        public static string GetConsoleOutput()
        {
            return consoleOutput.ToString();
        }
    }
}
