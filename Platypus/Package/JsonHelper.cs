using LitJson;

namespace Platypus.Package
{
    public class JsonHelper
    {
        public static T toObject<T>(string path)
        {
            string json = readFile(path);
            T jsonObject = JsonMapper.ToObject<T>(json);
            return jsonObject;
        }

        public static string toJson<T>(T jsonObject)
        {
            return JsonMapper.ToJson(jsonObject);
        }

        public static string readFile(string path)
        {
            string json = "";
            using (System.IO.StreamReader sr = new System.IO.StreamReader(path))
            {
                string str;
                while ((str = sr.ReadLine()) != null)
                {
                    json += str;
                }
            }
            return json;
        }

        public static void saveFile(string path, string content)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, true))
            {
                file.Write(content);
            }
        }

    }
}
