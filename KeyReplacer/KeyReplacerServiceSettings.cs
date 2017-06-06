using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace KeyReplacer
{
    [DataContract]
    public class KeyReplacerServiceSettings
    {
        [DataMember]
        public List<Keymap> Keymaps { get; set; }
        public static KeyReplacerServiceSettings FromJson(string json)
        {
            return JsonConvert.DeserializeObject<KeyReplacerServiceSettings>(json);
        }        

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    [DataContract]
    public class Keymap
    {
        [DataMember]
        public string From { get; set; }
        [DataMember]
        public string To { get; set; }
        [DataMember]
        public int Probability { get; set; }
    }
}
