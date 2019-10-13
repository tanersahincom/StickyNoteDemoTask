using System;
using System.Windows.Forms;

namespace StickyNote
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
            if (System.Diagnostics.Process.GetProcessesByName("Yapiskan Not").Length > 1) { Application.ExitThread(); }
        }
    }
}