using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OTM_Client
{
    public partial class frm_settings : Form
    {
        public frm_settings()
        {
            InitializeComponent();
        }

        private void frm_settings_Load(object sender, EventArgs e)
        {
            if (Program.f.settings["clientPort"] != null) this.txt_client_port.Text = Program.f.settings["clientPort"];
            if (Program.f.settings["serverHost"] != null) this.txt_host_addr.Text = Program.f.settings["serverHost"];
            if (Program.f.settings["serverPort"] != null) this.txt_host_port.Text = Program.f.settings["serverPort"];

            Rectangle workingArea = Screen.GetWorkingArea(this);

            this.Location = new Point(workingArea.Right - Size.Width - Program.f.Size.Width, workingArea.Bottom - Size.Height);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.txt_client_port.Text == ""
            || this.txt_host_addr.Text == ""
            || this.txt_host_port.Text == ""
            || this.txt_client_port.Text == "")
            {
                MessageBox.Show("Alle velden zijn verplicht!");
            }
            else
            {
                Program.f.settings["clientPort"] = this.txt_client_port.Text;
                Program.f.settings["serverHost"] = this.txt_host_addr.Text;
                Program.f.settings["serverPort"] = this.txt_host_port.Text;
                Program.f.updateReg();
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Program.f.closeApplication();
        }
    }
}
