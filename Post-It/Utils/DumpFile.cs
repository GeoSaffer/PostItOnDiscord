using System;
using System.IO;

namespace Post_It.Utils
{
    public static class DumpFile
    {
        public static void Create(Exception e)
        {
            if (!Directory.Exists("Logs")) Directory.CreateDirectory("Logs");
            var timestamp = $"{DateTime.Now:yyyy-MM-dd_HHmmss}";
            using (StreamWriter sw = File.CreateText($"Logs\\Exception_{timestamp}.log"))
            {
                sw.WriteLine(timestamp);
                sw.WriteLine("");
                sw.WriteLine("");
                sw.WriteLine($"OS VErsion: {Environment.OSVersion}");
                sw.WriteLine($"");
                sw.WriteLine($"64Bit OS: {Environment.Is64BitOperatingSystem}");
                sw.WriteLine($"");
                sw.WriteLine($"CPU Cores available: {Environment.ProcessorCount}");
                sw.WriteLine("");
                sw.WriteLine($"64Bit Process: {Environment.Is64BitProcess}");
                sw.WriteLine($"");
                sw.WriteLine($"Machine Name: {Environment.MachineName}");
                sw.WriteLine($"");
                sw.WriteLine($"Current Directory: {Environment.CurrentDirectory}");
                sw.WriteLine($"");
                sw.WriteLine($"Command Line: {Environment.CommandLine}");
                sw.WriteLine($"");
                sw.WriteLine($"Exit Code: {Environment.ExitCode}");
                sw.WriteLine($"");
                sw.WriteLine($"System Directory: {Environment.SystemDirectory}");
                sw.WriteLine($"");
                sw.WriteLine($"Environment Version: {Environment.Version}");
                sw.WriteLine($"");
                sw.WriteLine($"Environment StackTrace: {Environment.StackTrace}");
                sw.WriteLine($"");
                sw.WriteLine($"");
                sw.WriteLine($"");
                sw.WriteLine($"");
                sw.WriteLine($"");
                sw.WriteLine($"Exception Message: \n{e.Message}");
                sw.WriteLine($"");
                sw.WriteLine($"Inner Exception: \n{e.InnerException}");
                sw.WriteLine($"");
                sw.WriteLine($"Exception StackTrace: \n{e.StackTrace}");

            }
        }
    }
}
