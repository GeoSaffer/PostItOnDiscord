using Newtonsoft.Json;
using System.IO;

namespace Post_It.Utils
{
    public static class JsonFile
    {
        public static T Load<T>(string filename)
        {
            using (var r = new StreamReader(filename))
            {
                string json = r.ReadToEnd();
                var deserializedObject = JsonConvert.DeserializeObject<T>(json);
                return deserializedObject;
            }
        }

        public static void Create<T>(string filePath, object obj)
        {
            using (StreamWriter sw = File.CreateText(filePath))
            {
                var jonString = JsonConvert.SerializeObject((T)obj,Formatting.Indented);
                sw.Write(jonString);
            }
        }
    }
}
