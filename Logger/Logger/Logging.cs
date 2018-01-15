using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    class Logging
    {

        private string serverPath = @"log/";

        public Logging()
        {

        }

        private string unixDate(DateTime datum)
        {
            string mesiac = datum.Month.ToString();
            string den = datum.Day.ToString();
            string rok = datum.Year.ToString();

            return rok + "-" + mesiac + "-" + den;
        }

        private SortedList openFile()
        {
            StreamWriter sfw = null;
            string complFile = "";
            string fileName = "";

            if (serverPath != "end")
            {
                DateTime dt = DateTime.Today;

                string shortDate = this.unixDate(dt);
                //string path = @"..\App_Data\";

                complFile = this.serverPath + shortDate + ".log";
                fileName = shortDate + ".log";

                /*if (!File.Exists(complFile))
                {
                    File.Create(complFile);
                } */

                int counter = 0;

                while (FileInUse(@complFile) && counter < 10)
                {
                    System.Threading.Thread.Sleep(100);
                    counter++;
                }
                if (counter < 10)
                {
                    sfw = new StreamWriter(@complFile, true);
                }
                else
                {
                    sfw = null;
                }
            }

            SortedList result = new SortedList();
            result.Add("sfw", sfw);
            result.Add("fileName", fileName);

            return result;
        }

        public void logData(object data, string error, string idf)
        {
            string logIp = "localhost";

            string dt = DateTime.Today.ToShortDateString();
            string dh = DateTime.Now.ToLongTimeString();
            StringBuilder sb = new StringBuilder();
            SortedList errorDt = new SortedList();

            Boolean sendMail = false;

            string strToWrite = "";

            if (error.Length > 0)
            {
                sb.AppendFormat("ERROR   {0} {1} -- {2}, IP:{3} ERROR:\r\n {4} ", dt, dh, idf, logIp, error);
                sb.AppendFormat("Stack trace: {0} \r\n", Environment.StackTrace.ToString());
                sb.AppendLine("\r\n-----------------------------------------------------------END OF ERROR\r\n");

                sendMail = true;

                errorDt.Add("stack", Environment.StackTrace.ToString());
                errorDt.Add("error", error);
            }
            //sw.WriteLine(sb.ToString());
            // sb.Length = 0;

            if (data.GetType() == typeof(NameValueCollection))
            {
                NameValueCollection sl = (NameValueCollection)data;
                sb.AppendFormat("{0} {1} -- {2} --IP:{3} ---- NameValueCollection:\r\n", dt, dh, idf, logIp);

                foreach (string key in sl)
                {
                    sb.AppendFormat("['{0}'] = {1} \r\n", key, sl[key].ToString());
                }

                sb.AppendLine("\r\n-----------------------------------------------------------END OF  NameValueCollection\r\n");
                strToWrite = sb.ToString();
            }

            if (data.GetType() == typeof(SortedList))
            {
                SortedList sl = (SortedList)data;
                sb.AppendFormat("{0} {1} -- {2} --IP:{3} ---- SortedList:\r\n", dt, dh, idf, logIp);

                foreach (DictionaryEntry row in sl)
                {
                    sb.AppendFormat("       ['{0}'] = {1} \r\n", row.Key.ToString(), row.Value.ToString());
                }
                sb.AppendLine("\r\n-----------------------------------------------------------END OF  SortedList\r\n");
                strToWrite = sb.ToString();

                if (sendMail) errorDt.Add("data", strToWrite);

            }

            if (data.GetType() == typeof(Dictionary<int, Hashtable>))
            {
                Dictionary<int, Hashtable> table = (Dictionary<int, Hashtable>)data;
                sb.AppendFormat("{0} {1} -- {2} --IP:{3} -- Dictionary<int, Hashtable>:\r\n", dt, dh, idf, logIp);
                int cnt = table.Count;
                for (int row = 0; row < cnt; row++)
                {
                    foreach (DictionaryEntry riad in table[row])
                    {
                        sb.AppendFormat("        [{0}]['{1}'] = {2} \r\n", row, riad.Key.ToString(), riad.Value.ToString());
                    }
                }
                sb.AppendLine("\r\n-----------------------------------------------------------END OF  Dictionary<int, Hashtable>\r\n");

                strToWrite = sb.ToString();

                if (sendMail) errorDt.Add("data", strToWrite);
            }

            if (data.GetType() == typeof(Dictionary<int, SortedList>))
            {
                Dictionary<int, SortedList> table = (Dictionary<int, SortedList>)data;
                sb.AppendFormat("{0} {1} -- {2} -- IP:{3} -- Dictionary<int, SortedList>:\r\n", dt, dh, idf, logIp);
                int cnt = table.Count;
                for (int row = 0; row < cnt; row++)
                {
                    foreach (DictionaryEntry riad in table[row])
                    {
                        sb.AppendFormat("        [{0}]['{1}'] = {2} \r\n", row, riad.Key.ToString(), riad.Value.ToString());
                    }
                }
                sb.AppendLine("\r\n-----------------------------------------------------------END OF  Dictionary<int, SortedList>\r\n");
                // strToWrite = sb.ToString();

                strToWrite = sb.ToString();

                if (sendMail) errorDt.Add("data", strToWrite);
            }


            if (data.GetType() == typeof(string))
            {
                sb.AppendFormat("{0} {1} -- {2} -- IP:{3} -- string data:\r\n", dt, dh, idf, logIp);
                sb.AppendFormat("        string = {0} \r\n", data.ToString());
                sb.AppendLine("\r\n-----------------------------------------------------------END OF  string\r\n");


                strToWrite = sb.ToString();


                if (sendMail) errorDt.Add("data", strToWrite);
            }

            StreamWriter sw = null;

            try
            {
                SortedList res = this.openFile();
                sw = (StreamWriter)res["sfw"];
                string fileName = res["fileName"].ToString();

                if (sw != null)
                {

                    sw.WriteLine(strToWrite);
                    //sw.Flush();
                    sw.Close();
                    // this.registerTempFile(fileName, 7);
                    sw.Dispose();

                    //reg.registerTempFile(sw.)
                }


                // if (sendMail) this.sendMail(errorDt);

            }
            catch (Exception ex)
            {
                if (this.serverPath != "end")
                {

                    string edt = DateTime.Today.ToShortDateString();
                    string edh = DateTime.Now.ToLongTimeString();

                    string elogIp = "localhost";

                    DateTime eDt = DateTime.Today;

                    string shortDate = this.unixDate(eDt);
                    string fileName = this.serverPath + @"\App_Data\global_error_" + shortDate + ".log";
                    string shortName = "global_error_" + shortDate + ".log";
                    StringBuilder esb = new StringBuilder();
                    esb.AppendFormat("\r\n Global error..........{0} {1} {2}", elogIp, edt, edh);
                    esb.Append("_________________________________________________________________________\r\n");
                    esb.AppendFormat("{0}\r\n", ex.ToString());
                    esb.Append("_________________________________________________________________________\r\n");


                    StreamWriter sfw = new StreamWriter(fileName, true);
                    sfw.WriteLine(esb.ToString());
                    sfw.Close();
                    sfw.Dispose();
                    //this.registerTempFile(shortName, 7);


                }

                //sw.Flush();
                //sw.Close();
            }

        }

        static bool FileInUse(string path)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                {

                    if (fs.CanWrite)
                    {
                        fs.Dispose();
                        fs.Close();
                    }
                }
                return false;
            }
            catch (IOException ex)
            {
                return true;
            }
        }

    }
}
