using System.IO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.Text;
using System.Timers;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace DesktopBackgroundChanger
{

    public class RandomImageProvider
    {
        Timer timer;

        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        public static extern int SystemParametersInfo(UAction uAction, int uParam, StringBuilder lpvParam, int fuWinIni);

        private  List<string> _topics;

        internal void init(TimeSpan timeSpan, IEnumerable<string> topics)
        {
            _topics = topics.Select(x=> HttpUtility.UrlEncode(x)).ToList();
            timer = new Timer();
            timer.Elapsed += Timer_Elapsed;
            timer.Interval = timeSpan.TotalMilliseconds;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                ChangeToRandomBackground();
            }
            catch (Exception ex)
            { }
        }
        private int modCount=0;
        public void ChangeToRandomBackground()
        {
            string html = GetHtmlCode();
            List<string> urls = GetUrls(html);
            var rnd = new Random();

            int randomUrl = rnd.Next(0, urls.Count - 1);

            string luckyUrl = urls[randomUrl];

            byte[] image = GetImage(luckyUrl);
            string file = AppContext.BaseDirectory+$"temp{(modCount)}.png";
            modCount = (modCount + 1) % 2;
         
            File.WriteAllBytes(file, image);
            SetBackgroud(file);




        }
        public enum UAction
        {
            /// <summary>
            /// set the desktop background image
            /// </summary>
            SPI_SETDESKWALLPAPER = 0x0014,
            /// <summary>
            /// set the desktop background image
            /// </summary>
            SPI_GETDESKWALLPAPER = 0x0073,
        }

        private static int SetBackgroud(string fileName)
        {
            int result = 0;
            if (File.Exists(fileName))
            {
                StringBuilder s = new StringBuilder(fileName);
                result = SystemParametersInfo(UAction.SPI_SETDESKWALLPAPER, 0, s, 0x2);
            }
            return result;
        }



        /// <summary>
        /// set the option of registry
        /// </summary>
        /// <param name="optionsName">the name of registry</param>
        /// <param name="optionsData">set the data of registry</param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool SetOptions(string optionsName, string optionsData, ref string msg)
        {
            bool returnBool = true;
            RegistryKey classesRoot = Registry.CurrentUser;
            RegistryKey registryKey = classesRoot.OpenSubKey(@"Control Panel\Desktop", true);
            try
            {
                if (registryKey != null)
                {
                    registryKey.SetValue(optionsName.ToUpper(), optionsData);
                }
                else
                {
                    returnBool = false;
                }
            }
            catch
            {
                returnBool = false;
                msg = "Error when read the registry";
            }
            finally
            {
                classesRoot.Close();
                registryKey.Close();
            }
            return returnBool;
        }



        private string GetHtmlCode()
        {
            var rnd = new Random();

            int topic = rnd.Next(0, _topics.Count);

            string url = @"https://www.flickr.com/search/?text=" + _topics[topic]+"& dimension_search_mode = min & height = 1024 & width = 1024 & orientation = landscape & media = photos";
            string data = "";

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Accept = @"text/html, application/xhtml+xml, *//*";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";

            var response = (HttpWebResponse)request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                if (dataStream == null)
                    return "";
                using (var sr = new StreamReader(dataStream))
                {
                    data = sr.ReadToEnd();
                }
            }
            return data;
        }

        private List<string> GetUrls(string html)
        {
            var urls = new List<string>();
            int ndx = html.IndexOf("main search-photos-results", StringComparison.Ordinal);
            ndx = html.IndexOf("url(", ndx, StringComparison.Ordinal);

            while (ndx >= 0)
            {
                ndx = ndx + 4;
                int ndx2 = html.IndexOf(")", ndx, StringComparison.Ordinal);
                string url = "https:" + html.Substring(ndx, ndx2 - ndx);
                urls.Add(url);
                ndx = html.IndexOf("url(", ndx, StringComparison.Ordinal);
            }
            return urls;
        }

        private byte[] GetImage(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            var response = (HttpWebResponse)request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                if (dataStream == null)
                    return null;
                using (var sr = new BinaryReader(dataStream))
                {
                    byte[] bytes = sr.ReadBytes(100000000);

                    return bytes;
                }
            }

            return null;
        }


    }
}
