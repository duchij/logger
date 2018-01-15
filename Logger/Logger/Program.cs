using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    class Program
    {
        static void Main(string[] args)
        {

            ProcessPasswordFile ppf = new ProcessPasswordFile("input.txt","output.txt");
            ppf.process();
            Console.ReadKey();

           // lgf()


        }
    }
}
