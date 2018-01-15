using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

using System.Resources;


namespace Logger
{
   

    class ProcessPasswordFile
    {

        private string fileName = null;
        private string outFileName = null;

        private char delimiter;

        private StreamReader sr;
        private StreamWriter sw;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessPasswordFile"/> class.
        /// </summary>
        /// <param name="name">Input fileName</param>
        /// <param name="output">Output fileName.</param>
        /// <param name="delimiter">Delimiter between name and password. Default is space</param>
        public ProcessPasswordFile(string name,string output, char delimiter=' ')
        {

            this.init(name);

            this.fileName = name;


            this.sr = new StreamReader(@name);
            this.sw = new StreamWriter(@output);
            this.outFileName = output;

            this.delimiter = delimiter;
           

           // this.process();

        }
        // checkneme ci subor existuje ak nie vytvorime z resource file :), ktory 
        private void init(string name)
        {
           // System.Resources.ResourceManager RM = new System.Resources.ResourceManager(@"data.resx", System.Reflection.Assembly.GetExecutingAssembly());

            if (!File.Exists(@name)){

                string data1 = data.testData;

                StreamWriter sw = new StreamWriter(@name);
                sw.Write(data1);
                sw.Flush();
                sw.Dispose();

               // string data = Properties.

            }

           if (!Directory.Exists(@"log")){

                Directory.CreateDirectory(@"log");

            }
        }

        public void process()
        {
            Log log = new Log();

            logToFile lgf = new logToFile(log.logToFile);
            logToConsole lgc = new logToConsole(log.logToConsole);

           // StringBuilder sb = new StringBuilder(); //mohli by sme to robit aj cez stringBuilder :)
            string line = null;
            try
            {

                while ((line = this.sr.ReadLine()) != null)
                {

                    lgc("Line to process", line);
                    lgf(line, "", "Line to process");

                    string computedLine = this.processLine(line);
                    lgc("Compute Line:", computedLine);
                    lgf(computedLine, "", "Line to save");
                    //sb.AppendLine(computedLine); // mohli by sme robit tak ye vsetko ulozime so stringbuildera a nasledne ulozime
                    //alebo priamo
                    lgc("Writing data to file:", this.outFileName);
                    sw.WriteLine(computedLine);
                }
                    
            }
            catch (PasswordProcessException ex)
            {

                lgc("Error in process of password", ex.Message);
                lgf(line, "Error in processing file", "PasswordProcess");
                // lgc("Error in")
            }
            finally // to by sa malo spravit vzdy, kvoli handlerom.....
            {
                // sw.
                    
                sr.Dispose();
                sw.Flush(); //flushneme data aby to bolo ciste
                sw.Dispose();
            }

        }

        private string processLine(string line)
        {

            string[] tmp = line.Split(this.delimiter);// no ak ich mame oddelen
            byte[] data = UTF8Encoding.UTF8.GetBytes(tmp[1]);
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();

            byte[] hash = provider.ComputeHash(data);

            return  string.Format("{0} {1}",tmp[0],Convert.ToBase64String(hash));


        }
    }
}
