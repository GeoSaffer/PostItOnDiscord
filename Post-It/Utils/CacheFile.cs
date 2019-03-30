using System;
using System.IO;

namespace Post_It.Utils
{
    public static class CacheFile
    {
        private static string Timestamp => $"{DateTime.Now:yyyyMMddHHmmss}";

        public static string Create(string filePath)
        {
            if (!Directory.Exists("Cache")) Directory.CreateDirectory("Cache");
            var extenstion = Path.GetExtension(filePath);
            var fileName = $"{Timestamp}{extenstion}";
            File.Copy(filePath, $@"Cache\{fileName}", true);
            return fileName;
        }
    }
}
