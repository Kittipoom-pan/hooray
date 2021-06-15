using Hooray.Core.ViewModels;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace Hooray.Core.Manager
{
    public class Log 
    {
        public Log()
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US", false);

            string format = ConfigurationModel.LoremLogPath;

            string filename = DateTime.Today.ToString("ddMMyyyy");

            string path = string.Format(format, filename);

            if (!File.Exists(path))
            {
                FileStream stream = new FileStream(path, FileMode.Create);
                stream.Close();
            }
        }

        public void WriteExceptionLog(Exception ex, string servicename)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US", false);

            string format = ConfigurationModel.LoremLogPath;

            string filename = DateTime.Today.ToString("ddMMyyyy");

            string path = string.Format(format, filename);

            using (StreamWriter writer = new StreamWriter(path, true))
            {
                string description = ex.ToString();

                string[] split = description.Split('\\');

                string timestamp = DateTime.Now.ToLongTimeString();

                writer.WriteLine(string.Format("{0} --> {1} {2} {3} --> {4}", timestamp, split[split.Count() - 1], ex.TargetSite.Name, ex.Message, servicename));
            }
        }

        public void WriteMessageLog(string message)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US", false);

            string format = ConfigurationModel.LoremLogPath;

            string filename = DateTime.Today.ToString("ddMMyyyy");

            string path = string.Format(format, filename);

            try
            {
                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    string timestamp = DateTime.Now.ToLongTimeString();

                    writer.WriteLine(string.Format("{0} --> {1}", timestamp, message));
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
