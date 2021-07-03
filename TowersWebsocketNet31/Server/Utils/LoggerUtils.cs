using System;
using System.IO;
using System.Text;

namespace TowersWebsocketNet31.Server
{
    public static class LoggerUtils
    {
        public static void WriteToLogFile(string fileName, string callback)
        {
            //Console.WriteLine(Directory.GetCurrentDirectory());
            string path = "../../../Logs/" + fileName + ".twlog";
            try
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(callback);
                }	
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
        }
    }
}