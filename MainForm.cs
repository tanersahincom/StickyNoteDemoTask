using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using StickyNote.Services;

namespace StickyNote
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private readonly RegistryKey _key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);

        private readonly IPageService _pageService = new PageService();

        private void Form1_Load(object sender, EventArgs e)
        {
            _pageService.CreateMainPageIfNotExists();
            var result = _pageService.LoadPages();
            if (!result) return;
            _pageService.CreatePage();
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            Hide();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var di = new DirectoryInfo("C:\\YapiskanNot");
            var subDirs = di.GetDirectories();
            if (subDirs.Length != 1) return;
            try { _key.DeleteValue("YapiskanNot", true); }
            catch
            {
                // ignored
            }
            Application.ExitThread();
        }
    }
}