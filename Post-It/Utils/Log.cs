using System;
using System.IO;

namespace Post_It.Utils
{
    public static class Log
    {
        private static string Timestamp => $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}";
        private static string Date => $"{DateTime.Now:yyyy-MM-dd}";

        public static void Info(string msg)
        {
            UpdateLogFile($"[{Timestamp}] [INFO] :  {msg}");
        }

        public static void Warning(string msg)
        {
            UpdateLogFile($"[{Timestamp}] [WARNING] :  {msg}");
        }

        public static void Error(string msg)
        {
            UpdateLogFile($"[{Timestamp}] [ERROR] :  {msg}");
        }

        public static void Exception(string msg)
        {
            UpdateLogFile($"[{Timestamp}] [EXCEPTION] :  {msg}");
        }

        private static void UpdateLogFile(string data)
        {
            data = StripCharacters(data);
            if (!Directory.Exists("Logs")) Directory.CreateDirectory("Logs");
            var path = $"Logs\\{Date}.log";
            if (File.Exists(path))
            {
                using (var sw = File.AppendText($"Logs\\{Date}.log"))
                {
                    sw.WriteLine(data);
                }
            }
            else
            {
                using (var sw = File.CreateText($"Logs\\{Date}.log"))
                {
                    sw.WriteLine(data);
                }
            }
        }

        private static string StripCharacters(string data)
        {
            data = data.Replace("\r", "");
            data = data.Replace("\n", "");
            return data;
        }

    }
}
