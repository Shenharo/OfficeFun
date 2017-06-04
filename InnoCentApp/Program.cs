using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsInput;

namespace InnoCentApp
{
    class InterceptKeys

    {

        public static void Main()
        {
            var service= new KeyReplacer.KeyReplacerService(new KeyReplacer.KeyReplacerServiceSettings
                ()
            {
                Keymaps = new List<KeyReplacer.Keymap>
                {
                   new KeyReplacer.Keymap(){From="a" , To="b", Probability=50},
                   new KeyReplacer.Keymap(){From=";" , To=".,", Probability=80},
                }
            });
            Application.Run();
            service.Dispose();

        }
    }

}
