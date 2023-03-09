using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace ModpacksCH.API.Model
{
    public class ModpackFile
    {
        public bool ClientOnly { get; set; }
        public int ID { get; set; }
        public List<object> Mirrors { get; set; }
        public string Name { get; set; }
        public bool Optional { get; set; }
        public string Path { get; set; }
        public bool ServerOnly { get; set; }
        public string SHA1 { get; set; }
        public int Size { get; set; }
        public List<Tag> Tags { get; set; }

        [JsonConverter(typeof(StringEnumConverter), typeof(CamelCaseNamingStrategy))]
        public ModpackFileType Type { get; set; }

        public long Updated { get; set; }
        public string Url { get; set; }
        public string Version { get; set; }
        public CF CurseForge { get; set; }
        public class CF
        {
            public int Project { get; set; }
            public int File { get; set; }
        }
    }
}