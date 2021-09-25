using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clicker
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        bool clickerActivated = false;
        bool clickerEnabled = true;

        private IKeyboardMouseEvents m_GlobalHook;

        public Form1()
        {
            InitializeComponent();
            Subscribe();

            textBox3.Text = Properties.Settings.Default.startChar.ToString();
            checkBox1.Checked = Properties.Settings.Default.rightClick;
            textBox1.Text = Properties.Settings.Default.leftClickDef.ToString();
            textBox2.Text = Properties.Settings.Default.rightClickDef.ToString();

            GlobalHotKey.RegisterHotKey("Alt + " + Properties.Settings.Default.startChar, () => ClickerFunc());
            GlobalHotKey.RegisterHotKey("Alt + C", () => Application.Exit());

            textBox3.MaxLength = 1;
        }

        public void ClickerFunc()
        {
            if (clickerEnabled)
            {
                Thread thread = new Thread(new ThreadStart(AutoClicker));
                if (clickerActivated == true)
                {
                    label4.Text = "Clicker Not Running";
                    label4.ForeColor = Color.Red;
                    clickerActivated = false;
                    thread.Abort();
                }
                else
                {
                    label4.Text = "Clicker Running";
                    label4.ForeColor = Color.Lime;
                    clickerActivated = true;
                    thread.Start();
                }
            }
        }

        public void DoMouseClick(bool right)
        {
            if(right)
            {
                uint X = (uint)Cursor.Position.X;
                uint Y = (uint)Cursor.Position.Y;
                mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, X, Y, 0, 0);
            }
            else
            {
                uint X = (uint)Cursor.Position.X;
                uint Y = (uint)Cursor.Position.Y;
                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
            }
        }

        public void AutoClicker()
        {
            while (clickerActivated == true)
            {
                if(checkBox1.Checked)
                {
                    if (textBox1.Text == textBox2.Text)
                    {
                        Thread.Sleep(new Random().Next(Convert.ToInt32(textBox1.Text)));
                        DoMouseClick(true);
                    }
                    else
                    {
                        Thread.Sleep(new Random().Next(Convert.ToInt32(textBox1.Text), Convert.ToInt32(textBox2.Text)));
                        DoMouseClick(true);
                    }
                }
                else
                {
                    if (textBox1.Text == textBox2.Text)
                    {
                        Thread.Sleep(new Random().Next(Convert.ToInt32(textBox1.Text)));
                        DoMouseClick(false);
                    }
                    else
                    {
                        Thread.Sleep(new Random().Next(Convert.ToInt32(textBox1.Text), Convert.ToInt32(textBox2.Text)));
                        DoMouseClick(false);
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                DialogResult result = MessageBox.Show("CPS wont work because right click option is enabled\nWould you like to continue?", "Warning", MessageBoxButtons.YesNo);
                if(result == DialogResult.Yes)
                {
                    CpsTest cps = new CpsTest();
                    cps.Show();
                }
            }
            else
            {
                CpsTest cps = new CpsTest();
                cps.Show();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (textBox3.Text.Length > 0)
            {
                Properties.Settings.Default.startChar = textBox3.Text.ToCharArray()[0];
                Properties.Settings.Default.Save();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.rightClick = checkBox1.Checked;
            Properties.Settings.Default.Save();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.leftClickDef = Convert.ToInt32(textBox1.Text);
                Properties.Settings.Default.Save();
            }
            catch
            {
                textBox1.Text = Properties.Settings.Default.leftClickDef.ToString();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.rightClickDef = Convert.ToInt32(textBox2.Text);
                Properties.Settings.Default.Save();
            }
            catch
            {
                textBox2.Text = Properties.Settings.Default.rightClickDef.ToString();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void Subscribe()
        {
            m_GlobalHook = Hook.GlobalEvents();
            m_GlobalHook.KeyPress += GlobalHookKeyPress;
        }

        private void GlobalHookKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Properties.Settings.Default.startChar.ToString().ToLower().ToCharArray()[0])
                ClickerFunc();
        }

        public void Unsubscribe()
        {
            m_GlobalHook.KeyPress -= GlobalHookKeyPress;
            m_GlobalHook.Dispose();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            textBox3.Text = textBox3.Text.ToUpper();
            if(textBox3.Text.Length > 0)
            {
                Properties.Settings.Default.startChar = textBox3.Text.ToLower().ToCharArray()[0];
            }
        }
    }
}
