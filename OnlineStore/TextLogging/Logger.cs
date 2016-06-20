using System;
using System.Configuration;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.TextLogging
{
    public class QALog
    {
        static System.Collections.Specialized.NameValueCollection appSettings = ConfigurationManager.AppSettings;
        static string LogFile = System.IO.Path.Combine(appSettings["LogDirectory"] == null ? Environment.CurrentDirectory : appSettings["LogDirectory"],
    appSettings["LogPrefix"] == null ? "NUnitTest" : appSettings["LogPrefix"] + string.Format("_{0:yyyyMMddHHmmss}.log", DateTime.Now));

        // Logging definition start
        public static void QATextLog(string str)
        {
            QATextLog(str, null);
            return;
        }

        public static void QATextLog(string str, params object[] para)
        {
            string str2 = string.Format("[{0:yyyy-MM-dd HH:mm:ss.ffff}] ", DateTime.Now) + str;
            Console.WriteLine(str2, para);
            if (para != null)
            {
                Debug.WriteLine(str2, para);
                try
                {
                    System.IO.File.AppendAllText(LogFile, string.Format(str2, para) + Environment.NewLine);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
            else
            {
                Debug.WriteLine(str2);
                try
                {
                    System.IO.File.AppendAllText(LogFile, str2 + Environment.NewLine);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
        return;
        }
        // Logging definition end
    }
}
