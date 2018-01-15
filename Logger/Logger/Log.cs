using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{

    public delegate void logToFile(object data, string error, string idf);
    public delegate void logToConsole(string message, string data);

    class Log
    {

        private Logging _log;

        public Log()
        {
            this._log = new Logging();
        }

        public void logToFile(object data, string error, string idf)
        {
            this._log.logData(data, error, idf);
        }

        public void logToConsole(string message, string data)
        {
            Display.writeOut(message, data);
        }

    }
}
