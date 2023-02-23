using System.Collections.Generic;
using System.Linq;
using SharpYaml.Serialization;

namespace Blaze.Bundle.Data
{
    /// unity ab 装配文件读取
    public class ManifestDesc
    {
        public ManifestDesc(string content)
        {
            // File.ReadAllText(Path)
            var deserializer = new Serializer();
            var obj = deserializer.Deserialize(content) as Dictionary<object, object>;
            CRC =  obj["CRC"].ToString();
            Assets =((List<object>) obj["Assets"]).ToList().ConvertAll(delegate(object input)
            {
                return (string) input;
            });
        }

        public string CRC;
        public List<string> Assets;
    }

}