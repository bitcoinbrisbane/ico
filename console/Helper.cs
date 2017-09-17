using System;
using System.IO;
using System.Text;

namespace console
{
    public abstract class Helper
    {
        public static DateTime FromUnixTime(long unixTime)
        {
            return epoch.AddSeconds(unixTime);
        }
        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        protected static string GetABIFromFile(String path)
        {
            var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                String text = streamReader.ReadToEnd();
                return text;
            }
        }

        protected static string GetBytesFromFile(String path)
        {
            var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                String text = streamReader.ReadToEnd();
                return "0x" + text;
            }
        }
        
        protected static string GetBytesFromFile(String path, String contractname)
        {
            var fileStream = new FileStream(String.Format("{0}/{1}.bin",path, contractname) , FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                String text = streamReader.ReadToEnd();
                return "0x" + text;
            }
        }
    }
}