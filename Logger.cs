using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderUpdaterServer
{
    internal class Logger
    {
        private Logger() { }
        public static void Log(string info)
        {
            var logFile = new StreamWriter("application.log", true);
            logFile.WriteLine($"{DateTime.Now} --- {info}");
            logFile.Close();
        }
    }
}
