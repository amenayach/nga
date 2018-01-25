using Newtonsoft.Json;
using System.IO;

namespace nga
{
    public class Config
    {
        private const string ConfigPath = "nga.json";

        public string AppName { get; set; }

        public static Config Get()
        {
            var filepath = GetFilePath();

            if (File.Exists(filepath))
            {
                var text = File.ReadAllText(filepath);

                if (!string.IsNullOrEmpty(text))
                {
                    return JsonConvert.DeserializeObject<Config>(text);
                }
            }

            return new Config();
        }

        public static bool Save(Config config)
        {
            var filepath = GetFilePath();

            if (config != null)
            {
                File.WriteAllText(filepath, JsonConvert.SerializeObject(config));

                return true;
            }

            return false;
        }

        private static string GetFilePath()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), ConfigPath);
        }
    }
}
