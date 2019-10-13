using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

namespace StickyNote.Services
{
    internal class PageService : IPageService
    {
        public void CreatePage()
        {
            var rnd = new Random();
            var b = false;
            var name = "not";
            var di = new DirectoryInfo("C:\\YapiskanNot");
            var subDirs = di.GetDirectories();
            for (; ; )
            {
                var randomNumber = rnd.Next(2, 999999);
                name += randomNumber.ToString();
                foreach (var item in subDirs)
                {
                    if (item.ToString() != name) continue;
                    b = true;
                    randomNumber = rnd.Next(2, 999999);
                    name += randomNumber.ToString();
                }

                if (b) continue;
                var page = new Pages { Text = name };
                page.Show();
                break;
            }
        }

        public void CreateMainPageIfNotExists()
        {
            var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
            if (!Directory.Exists("c:\\YapiskanNot") || !Directory.Exists("c:\\YapiskanNot//not1"))
            {
                var di = Directory.CreateDirectory("c:\\YapiskanNot");
                di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                Directory.CreateDirectory("c:\\YapiskanNot\\not1");
            }

            // ReSharper disable once PossibleNullReferenceException
            try
            {
                key.SetValue("YapiskanNot", "\"" + Application.ExecutablePath + "\"");
            }
            catch
            {
                // ignored
            }
        }

        public bool LoadPages()
        {
            var result = true;
            var di = new DirectoryInfo("C:\\YapiskanNot");
            var subDirs = di.GetDirectories();
            foreach (var item in subDirs)
            {
                if (item.Name == "not1")
                {
                    continue;
                }

                var page = new Pages { Text = item.ToString() };
                page.Show();
                result = false;
            }

            return result;
        }

        public void SavePageLocationInfo(string text, string x, string y)
        {
            File.Delete("C:\\YapiskanNot\\" + text + "\\notlocation");
            var sw = new StreamWriter("C:\\YapiskanNot\\" + text + "\\notlocation");
            sw.WriteLine(x);
            sw.WriteLine(y);
            sw.Close();
        }

        public void SavePageSizeInfo(string text, string h, string w)
        {
            File.Delete("C:\\YapiskanNot\\" + text + "\\notsize");
            var sw1 = new StreamWriter("C:\\YapiskanNot\\" + text + "\\notsize");
            sw1.WriteLine(w);
            sw1.WriteLine(h);
            sw1.Close();
        }

        public void SaveForegroundColorInfo(string text, ColorDialog colorDialog)
        {
            File.Delete("c:\\YapiskanNot\\" + text + "\\notforeground");
            var sw = new StreamWriter("c:\\YapiskanNot\\" + text + "\\notforeground");
            sw.WriteLine(colorDialog.Color.ToArgb());
            sw.Close();
        }

        public void DeletePage(string text)
        {
            var filePaths = Directory.GetFiles("C:\\YapiskanNot\\" + text);
            foreach (var item in filePaths) { File.Delete(item); }
            Directory.Delete("C:\\YapiskanNot\\" + text);
        }

        public void SavePenColor(FontDialog fd, string text)
        {
            var cvt = new FontConverter();
            var s = cvt.ConvertToString(fd.Font);
            File.Delete("c:\\YapiskanNot\\" + text + "\\notpen");
            var sw = new StreamWriter("c:\\YapiskanNot\\" + text + "\\notpen");
            sw.WriteLine(s);
            sw.Close();
        }

        public void SaveBackgroundInfo(string text, ColorDialog cd)
        {
            File.Delete("c:\\YapiskanNot\\" + text + "\\notbackground");
            var sw = new StreamWriter("c:\\YapiskanNot\\" + text + "\\notbackground");
            sw.WriteLine(cd.Color.ToArgb());
            sw.Close();
        }
    }
}