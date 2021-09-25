using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clicker
{
    public partial class CpsTest : Form
    {
        bool asked = false;

        public Timer timer = new Timer();
        public List<int> _aveClickList = new List<int>();
        public int _clicks;
        public int _total;
        public CpsTest()
        {
            InitializeComponent();

            timer.Interval = 1000;
            timer.Tick += new EventHandler(timer_Tick);

            Timer timer2 = new Timer();
            timer2.Interval = 10;
            timer2.Tick += new EventHandler(Timer2_Tick);
            timer2.Enabled = true;

            Timer rcCheck = new Timer();
            rcCheck.Interval = 10;
            rcCheck.Tick += RcCheck_Tick;
            rcCheck.Enabled = true;
        }

        private void RcCheck_Tick(object sender, EventArgs e)
        {
            if(Properties.Settings.Default.rightClick)
            {
                if(!asked)
                {
                    asked = true;
                    DialogResult result = MessageBox.Show("Right Click option was enabled\nCPS button wont work with right click\nWould you like to continue?", "Warning", MessageBoxButtons.YesNo);
                    if (result == DialogResult.No)
                    {
                        this.Close();
                    }
                }
            }
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            label1.Text = "Cps: " + _clicks;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            _aveClickList.Add(_clicks);
            try
            {
                label3.Text = "Ave CPS: " + _aveClickList.Average().ToString().Substring(0, 4);
            }
            catch
            {
                label3.Text = "Ave CPS: " + _aveClickList.Average().ToString();
            }
            _clicks = 0;
        }

        private void CpsTest_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
            _total++;
            _clicks++;
            label2.Text = "Total CPS: " + _total;
        }
    }
}
