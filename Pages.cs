using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using StickyNote.Services;

namespace StickyNote
{
    public partial class Pages : Form
    {
        public Pages()
        {
            InitializeComponent();
        }

        private readonly RegistryKey _key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
        private readonly ContextMenu _menu = new ContextMenu();
        private readonly IResizeService _resize = new ResizeService();
        private readonly IPageService _pageService = new PageService();
        private bool _b1 = true, _b2 = true;

        public override Size MinimumSize
        {
            get => base.MinimumSize;
            // ReSharper disable once ValueParameterNotUsed
            set => base.MinimumSize = new Size(179, 51);
        }

        protected override void WndProc(ref Message m)
        {
            var x = (int)(m.LParam.ToInt64() & 0xFFFF);
            var y = (int)((m.LParam.ToInt64() & 0xFFFF0000) >> 16);
            var pt = PointToClient(new Point(x, y));

            if (m.Msg == 0x84)
            {
                switch (_resize.GetMousePosition(pt, this))
                {
                    case "l": m.Result = (IntPtr)10; return;  // the Mouse on Left Form
                    case "r": m.Result = (IntPtr)11; return;  // the Mouse on Right Form
                    case "a": m.Result = (IntPtr)12; return;
                    case "la": m.Result = (IntPtr)13; return;
                    case "ra": m.Result = (IntPtr)14; return;
                    case "u": m.Result = (IntPtr)15; return;
                    case "lu": m.Result = (IntPtr)16; return;
                    case "ru": m.Result = (IntPtr)17; return; // the Mouse on Right_Under Form
                    case "": m.Result = pt.Y < 32 /*mouse on title Bar*/ ? (IntPtr)2 : (IntPtr)1; return;
                }
            }
            base.WndProc(ref m);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                _key.SetValue("YapiskanNot", "\"" + Application.ExecutablePath + "\"");
            }
            catch
            {
                // ignored
            }
            if (!Directory.Exists("c:\\YapiskanNot"))
            {
                var di = Directory.CreateDirectory("c:\\YapiskanNot");
                di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            }
            if (Directory.Exists("C:\\YapiskanNot\\" + Text) == false)
            {
                Directory.CreateDirectory("C:\\YapiskanNot\\" + Text);
            }
            if (File.Exists("C:\\YapiskanNot\\" + Text + "\\not"))
            {
                var lines = File.ReadAllLines("C:\\YapiskanNot\\" + Text + "\\not");
                foreach (var line in lines) { richTextBox1.Text += line + Environment.NewLine; }
            }
            if (!File.Exists("C:\\YapiskanNot\\" + Text + "\\not"))
            {
                var file2 = new StreamWriter("C:\\YapiskanNot\\" + Text + "\\not");
                file2.Close();
            }
            if (File.Exists("c:\\YapiskanNot\\" + Text + "\\notpen"))
            {
                var value1 = File.ReadAllText("c:\\YapiskanNot\\" + Text + "\\notpen");
                var cvt = new FontConverter();
                var f = cvt.ConvertFromString(value1) as Font;
                // ReSharper disable once AssignNullToNotNullAttribute
                richTextBox1.Font = f;
            }
            if (File.Exists("c:\\YapiskanNot\\" + Text + "\\notforeground"))
            {
                var value2 = File.ReadAllText("c:\\YapiskanNot\\" + Text + "\\notforeground");
                richTextBox1.ForeColor = Color.FromArgb(int.Parse(value2));
            }
            if (File.Exists("c:\\YapiskanNot\\" + Text + "\\notbackground"))
            {
                var value3 = File.ReadAllText("c:\\YapiskanNot\\" + Text + "\\notbackground");
                richTextBox1.BackColor = Color.FromArgb(int.Parse(value3));
                BackColor = Color.FromArgb(int.Parse(value3));
            }
            if (File.Exists("C:\\YapiskanNot\\" + Text + "\\notlocation"))
            {
                var s = File.ReadAllLines("C:\\YapiskanNot\\" + Text + "\\notlocation");
                if (s.Length == 0) { _b1 = false; }
                if (_b1)
                {
                    var cord = new string[s.Length];
                    for (var i = 0; i < s.Length; i++)
                    {
                        cord[i] = s[i];
                    }
                    Location = new Point(int.Parse(cord[0]), int.Parse(cord[1]));
                }
            }
            if (File.Exists("C:\\YapiskanNot\\" + Text + "\\notsize"))
            {
                var s = File.ReadAllLines("C:\\YapiskanNot\\" + Text + "\\notsize");
                var cord = new string[s.Length];
                if (s.Length == 0) { _b2 = false; }
                if (_b2)
                {
                    for (var i = 0; i < s.Length; i++)
                    {
                        cord[i] = s[i];
                    }
                    Size = new Size(int.Parse(cord[0]), int.Parse(cord[1]));
                }
            }
            timer1.Interval = 1;
            timer1.Start();
            _menu.MenuItems.Add(0, new MenuItem("Add Note", AddNote));
            _menu.MenuItems.Add(1, new MenuItem("Font Color", ColorBoxForeground));
            _menu.MenuItems.Add(2, new MenuItem("Font Type", PenProperties));
            _menu.MenuItems.Add(3, new MenuItem("Page Design", ColorBoxBackground));
            _menu.MenuItems.Add(4, new MenuItem("Remove Note", Delete));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try { File.WriteAllText(@"C:\\YapiskanNot\\" + Text + "\\not", richTextBox1.Text); } catch { Application.ExitThread(); }
            richTextBox1.Width = Width; richTextBox1.Height = Height;
            var x = Location.X.ToString();
            var y = Location.Y.ToString();
            _pageService.SavePageLocationInfo(Text, x, y);
            var h = Size.Height.ToString();
            var w = Size.Width.ToString();
            _pageService.SavePageSizeInfo(Text, h, w);
        }

        private void Delete(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            _pageService.DeletePage(Text);
            Close();
        }

        private void AddNote(object sender, EventArgs e)
        {
            var p = new PageService();
            p.CreatePage();
        }

        private void ColorBoxForeground(object sender, EventArgs e)
        {
            var cd = new ColorDialog();
            cd.ShowDialog();
            richTextBox1.ForeColor = cd.Color;
            _pageService.SaveForegroundColorInfo(Text, cd);
        }

        private void PenProperties(object sender, EventArgs e)
        {
            var fd = new FontDialog();
            if (fd.ShowDialog() != DialogResult.OK) return;
            richTextBox1.Font = fd.Font;
            _pageService.SavePenColor(fd, Text);
        }

        private void ColorBoxBackground(object sender, EventArgs e)
        {
            var cd = new ColorDialog();
            cd.ShowDialog();
            richTextBox1.BackColor = cd.Color;
            BackColor = cd.Color;
            _pageService.SaveBackgroundInfo(Text, cd);
        }

        private void settingsBox_Click(object sender, EventArgs e)
        {
            _menu.Show(settingsBox, settingsBox.PointToClient(Cursor.Position));
        }

        private bool _dragging;
        private Point _offset;

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            _dragging = false;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            _dragging = true; _offset = e.Location;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_dragging) return;
            var currentScreenPos = PointToScreen(e.Location);
            Location = new Point(currentScreenPos.X - _offset.X, currentScreenPos.Y - _offset.Y);
        }

        private void Pages_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Stop();
        }
    }
}