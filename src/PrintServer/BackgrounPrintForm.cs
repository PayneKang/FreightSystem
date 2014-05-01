using System;
using System.Windows.Forms;

namespace PrintServer
{
    public partial class BackgrounPrintForm : Form
    {
        public BackgrounPrintForm()
        {
            InitializeComponent();
        }
        private PortListener listener = new PortListener(88);
        private void Form1_Load(object sender, EventArgs e)
        {
            this.listener.StartListener();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.listener.StopListener();
            this.notifyIcon1.Visible = false;
        }

        private void BackgrounPrintForm_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                this.ShowInTaskbar = false;
                this.notifyIcon1.Visible = true;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.ShowInTaskbar = true;
        }

        private void btnHide_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.ShowInTaskbar = false;
            this.notifyIcon1.Visible = true;
        }
    }
}
