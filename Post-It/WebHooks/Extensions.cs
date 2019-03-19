using System.Drawing;
using System.Globalization;
using System.Net;

namespace Post_It.WebHooks
{
    public static class Extensions
    {
        public static int ToRgb(this Color color)
        {
            return int.Parse(ColorTranslator.ToHtml(Color.FromArgb(color.ToArgb())).Replace("#", ""), NumberStyles.HexNumber);
        }
        public static int FromHexString(this string color)
        {
            return int.Parse(color.Replace("#FF", ""), NumberStyles.HexNumber);
        }

        public static bool CheckImageFromUrlExist(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "HEAD";

            bool exists;
            try
            {
                request.GetResponse();
                exists = true;
            }
            catch
            {
                exists = false;
            }

            return exists;
        }
    }
}
