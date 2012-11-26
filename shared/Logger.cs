using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace shared
{
    public class Logger
    {
        private static Logger instance;
        public static Logger GetInstance()
        {
            if (instance == null)
            {
                var name = Assembly.GetExecutingAssembly().FullName;
                instance = new Logger(String.Format(@"C:\temp\log_{0}.txt", name));
            }

            return instance;
        }

        private readonly string Path;
        private readonly Mutex mutex = new Mutex(); 
        private Logger(string path)
        {
            this.Path = path;
        }

        public void Write(string message)
        {
            if (mutex.WaitOne())
            {
                var writer = new StreamWriter(Path, true);
                writer.WriteLine(DateTime.Now.ToString() + " - " + message);
                writer.Close();

                mutex.ReleaseMutex();
            }
        }

        public void Write(string message, params object[] args)
        {
            if (mutex.WaitOne())
            {
                var writer = new StreamWriter(Path, true);
                writer.WriteLine(DateTime.Now.ToString() + " - " + String.Format(message, args));
                writer.Close();

                mutex.ReleaseMutex();
            }
        }

        public void Write(Exception ex)
        {
            Write(ex, ex.Message, new object[] { });
        }
        
        public void Write(Exception ex, string message, params object[] args)
        {
            if (mutex.WaitOne())
            {
                var writer = new StreamWriter(Path, true);
                writer.WriteLine(DateTime.Now.ToString() + " - " + String.Format(message, args));
                writer.WriteLine(ex.ToString());
                writer.Close();

                mutex.ReleaseMutex();
            }
        }
    }
}
