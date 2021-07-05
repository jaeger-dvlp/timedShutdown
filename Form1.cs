namespace timedShutdown
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// Defines the <see cref="timedShutdown" />.
    /// </summary>
    public partial class timedShutdown : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="timedShutdown"/> class.
        /// </summary>
        public timedShutdown()
        {
            InitializeComponent();
        }

        //Command Variables
        /// <summary>
        /// Defines the startCommand.
        /// </summary>
        internal string startCommand = "/C shutdown -s -t ";

        /// <summary>
        /// Defines the stopCommand.
        /// </summary>
        internal string stopCommand = "/C shutdown -a";

        //Converted Variables
        /// <summary>
        /// Defines the hours, minutes, seconds, theTime.
        /// </summary>
        internal int hours = 0, minutes = 0, seconds = 0, theTime = 0;

        //Not Converted Variables
        /// <summary>
        /// Defines the hoursNC, minutesNC, secondsNC.
        /// </summary>
        internal int hoursNC = 0, minutesNC = 0, secondsNC = 0;

        /// <summary>
        /// The close_MouseEnter.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void close_MouseEnter(object sender, EventArgs e)
        {
            close.ForeColor = Color.OrangeRed;
        }

        /// <summary>
        /// The close_MouseLeave.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void close_MouseLeave(object sender, EventArgs e)
        {
            close.ForeColor = System.Drawing.ColorTranslator.FromHtml("#6d7373");
        }

        /// <summary>
        /// The timedShutdown_Load.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void timedShutdown_Load(object sender, EventArgs e)
        {
            stopTimedShutdown();
        }

        /// <summary>
        /// The startButton_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void startButton_Click(object sender, EventArgs e)
        {
            startTimedShutdown();
        }

        /// <summary>
        /// The stopButton_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void stopButton_Click(object sender, EventArgs e)
        {
            stopTimedShutdown();
        }

        /// <summary>
        /// The startTimedShutdown.
        /// </summary>
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

        /// <summary>
        /// The stopTimedShutdown.
        /// </summary>
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

        /// <summary>
        /// The statusActive.
        /// </summary>
        private void statusActive()
        {
            statusLbl.Text = "Active";
            statusLbl.ForeColor = Color.Lime;
        }

        /// <summary>
        /// The statusInactive.
        /// </summary>
        private void statusInactive()
        {
            statusLbl.Text = "Inactive";
            statusLbl.ForeColor = Color.Red;
        }

        /// <summary>
        /// Defines the WM_NCLBUTTONDOWN.
        /// </summary>
        public const int WM_NCLBUTTONDOWN = 0xA1;

        /// <summary>
        /// Defines the HT_CAPTION.
        /// </summary>
        public const int HT_CAPTION = 0x2;

        /// <summary>
        /// The SendMessage.
        /// </summary>
        /// <param name="hWnd">The hWnd<see cref="IntPtr"/>.</param>
        /// <param name="Msg">The Msg<see cref="int"/>.</param>
        /// <param name="wParam">The wParam<see cref="int"/>.</param>
        /// <param name="lParam">The lParam<see cref="int"/>.</param>
        /// <returns>The <see cref="int"/>.</returns>
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        /// <summary>
        /// The ReleaseCapture.
        /// </summary>
        /// <returns>The <see cref="bool"/>.</returns>
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        /// <summary>
        /// The timedShutdown_MouseDown.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="System.Windows.Forms.MouseEventArgs"/>.</param>
        private void timedShutdown_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        /// <summary>
        /// The timer1_Tick.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
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
            if (seconds == 0 && minutesNC == 0 && hoursNC == 0)
            {
                MessageBox.Show("Shutting Down..", "Time's Up!", MessageBoxButtons.OK);
                remainTimeLbl.Text = "🕒 00:00:00";
                timer1.Enabled = false;
            }
            remainTimeLbl.Text = "🕒 " + hoursNC.ToString() + ":" + minutesNC.ToString() + ":" + secondsNC.ToString();
        }

        /// <summary>
        /// The label10_MouseEnter.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void label10_MouseEnter(object sender, EventArgs e)
        {
            label10.ForeColor = SystemColors.ControlDark;
        }

        /// <summary>
        /// The label10_MouseLeave.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void label10_MouseLeave(object sender, EventArgs e)
        {
            label10.ForeColor = SystemColors.ControlDarkDark;
        }

        /// <summary>
        /// The label10_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void label10_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/jaeger-dvlp");
        }

        /// <summary>
        /// The hoursText_KeyPress.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="KeyPressEventArgs"/>.</param>
        private void hoursText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// The secondsText_KeyPress.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="KeyPressEventArgs"/>.</param>
        private void secondsText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// The minutesText_KeyPress.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="KeyPressEventArgs"/>.</param>
        private void minutesText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// The close_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// The minimize_Click.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="e">The e<see cref="EventArgs"/>.</param>
        private void minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
