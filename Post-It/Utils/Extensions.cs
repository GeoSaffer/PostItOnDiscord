using System.Globalization;

namespace Post_It.Utils
{
    public static class Extensions
    {
        public static uint FromHexString(this string color)
        {
            return uint.Parse(color.Replace("#FF", ""), NumberStyles.HexNumber);
        }
    }
}
