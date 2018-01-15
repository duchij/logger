using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    class Display
    {
        public static void writeOut(string message, string text)
        {

            Console.Out.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "...." + message);
            Console.Out.WriteLine("..............................................................");
            Console.Out.WriteLine(text);
            Console.Out.WriteLine("..............................................................");
            Console.Out.WriteLine("Server ukoncite klavesou Escape.");
        }

    }
}
