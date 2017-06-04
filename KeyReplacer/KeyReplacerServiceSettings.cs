using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyReplacer
{
    public class KeyReplacerServiceSettings
    {
        public List<Keymap> Keymaps { get; set; } 
    }
    public class Keymap
    {
        public string From { get; set; }
        public string To { get; set; }
        public int Probability { get; set; }
    }
}
