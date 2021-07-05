using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace timedShutdown
{
    public partial class timedShutdown : Form
    {
        public timedShutdown()
        {
            InitializeComponent();
        }

        //Command Variables
        string startCommand = "/C shutdown -s -t ";
        string stopCommand = "/C shutdown -a";
        //Converted Variables
        int hours = 0, minutes = 0, seconds = 0, theTime = 0;
        //Not Converted Variables
        int hoursNC = 0, minutesNC = 0, secondsNC = 0;

        private void close_MouseEnter(object sender, EventArgs e)
        {
            close.ForeColor = Color.OrangeRed;
        }
        private void close_MouseLeave(object sender, EventArgs e)
        {
            close.ForeColor = System.Drawing.ColorTranslator.FromHtml("#6d7373");
        }

        private void timedShutdown_Load(object sender, EventArgs e)
        {
            stopTimedShutdown();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            startTimedShutdown();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            stopTimedShutdown();
        }

        private void startTimedShutdown()
        {
            hours = ((Convert.ToInt32(hoursText.Text)) * 60) * 60;
            minutes = ((Convert.ToInt32(minutesText.Text)) * 60);
            seconds = (Convert.ToInt32(secondsText.Text));
            hoursNC = Convert.ToInt32(hoursText.Text);
            minutesNC = Convert.ToInt32(minutesText.Text);
            secondsNC = Convert.ToInt32(secondsText.Text);
            theTime = hours + minutes + seconds;
            if (theTime > 0)
            {
                string runCode = startCommand + theTime.ToString();
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = runCode;
                process.StartInfo = startInfo;
                try
                {
                    process.Start();
                }
                catch (InvalidCastException a)
                {
                    MessageBox.Show(a.ToString() + "\n Please contact with me with this error. https://github.com/jaeger-dvlp");
                }
                stopButton.Enabled = true;
                startButton.Enabled = false;
                timer1.Enabled = true;
                statusActive();
            }
            else
            {
                MessageBox.Show("Time can't set as 0.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void stopTimedShutdown()
        {
            string runCode = stopCommand;
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = runCode;
            process.StartInfo = startInfo;
            process.Start();
            stopButton.Enabled = false;
            startButton.Enabled = true;
            timer1.Enabled = false;
            remainTimeLbl.Text = "🕒 00:00:00";
            statusInactive();
        }

        private void statusActive()
        {
            statusLbl.Text = "Active";
            statusLbl.ForeColor = Color.Lime;
        }
        private void statusInactive()
        {
            statusLbl.Text = "Inactive";
            statusLbl.ForeColor = Color.Red;
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void timedShutdown_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (secondsNC == 0)
            {
                secondsNC = 59;
                if (minutesNC == 0)
                {
                    minutesNC = 59;
                    if (hoursNC != 0)
                    {
                        hoursNC--;
                    }
                }
                else
                {
                    minutesNC--;
                }

            }
            else
            {
                secondsNC--;
            }
            if(seconds==0 && minutesNC==0 && hoursNC == 0)
            {
                MessageBox.Show("Shutting Down..", "Time's Up!", MessageBoxButtons.OK);
                remainTimeLbl.Text = "🕒 00:00:00";
                timer1.Enabled = false;
            }
            remainTimeLbl.Text = "🕒 " + hoursNC.ToString() + ":" + minutesNC.ToString() + ":" + secondsNC.ToString();
        }

        private void label10_MouseEnter(object sender, EventArgs e)
        {
            label10.ForeColor = SystemColors.ControlDark;
        }

        private void label10_MouseLeave(object sender, EventArgs e)
        {
            label10.ForeColor = SystemColors.ControlDarkDark;
        }

        private void label10_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/jaeger-dvlp");
        }

        private void hoursText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

        }
        private void secondsText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

        }

        private void minutesText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

        }

        private void close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
