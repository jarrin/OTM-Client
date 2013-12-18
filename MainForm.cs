using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;


namespace OTM_Client
{
    public partial class frm_main : Form
    {

        //Variables
        private Timer mpCheckTimer; //Timer for Mouse Position Checker
        private string formState; //State: Minimal, info and full
        private bool mouseInForm; //Is the mouse inside this form?


        //Own classes
        private UDPlink Con;
        private registry Reg;
        public error ErrorH;


        //Settings object.
        Dictionary<string, string> settings = new Dictionary<string, string>
        {
            {"serverPort", null},
            {"serverHost", null},
            {"clientPort", null}
        };    

        public frm_main()
        {
            InitializeComponent();
            
            this.changeState("minimal");
            ErrorH = new error();
            Reg = new registry(ErrorH);

            //Load settings from registry.
            this.settings["serverPort"] = Reg.get("serverPort");
            this.settings["serverHost"] = Reg.get("serverHost");
            this.settings["clientPort"] = Reg.get("clientPort");

            if (this.settings["serverPort"] == null || this.settings["serverHost"] == null || this.settings["clientPort"] == null)
            {
                ErrorH.handle("Er is helaas een fout opgetreden. Kan de applicatie niet opstarten. \n (Registry keys hebben null-waarde)", 9);
            }
            else
            {
                Con = new UDPlink(this.settings["serverHost"], Convert.ToInt32(this.settings["serverPort"]), Convert.ToInt32(this.settings["clientPort"]));
            }
            //Enable Timer
            mpCheckTimer = new Timer();
            mpCheckTimer.Interval = 200;
            mpCheckTimer.Tick += MousePosCheck;
            mpCheckTimer.Enabled = true;
            
        }


        //Checks if mouse pointer is inside Form(this)
        private void MousePosCheck(object sender, EventArgs e)
        {
            if (this.DesktopBounds.Contains(Cursor.Position))
            {
                if (this.formState == "info" || this.formState == "minimal") //If form WAS minimized
                {
                    this.mouseInForm = true; //Mouse is inside
                    this.changeState("full"); //Then change it to it's full state
                }
            }
            else
            {
                this.mouseInForm = false; //Mouse is outside
            }
        }

        //Changes form size & position
        public void changeState(string state)
        {
            Rectangle workingArea = Screen.GetWorkingArea(this);;
            switch (state)
            {
                case "minimal":
                    this.Height = 30;
                    this.Location = new Point(workingArea.Right - Size.Width, 
                                              workingArea.Bottom - Size.Height);
                    this.formState = "minimal";
                break;
                case "full":
                    this.Height = 329;
                    this.Location = new Point(workingArea.Right - Size.Width, 
                                              workingArea.Bottom - Size.Height);
                    this.formState = "full";
                break;
            }
        }


        public void setStatus(string t)
        {
            this.lbl_status.Invoke((MethodInvoker)(() => this.lbl_status.Text = t));
        }





        private void frm_main_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pnl_default_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btn_2_Click(object sender, EventArgs e)
        {

        }

        private void btn_6_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void lbl_status_Click(object sender, EventArgs e)
        {

        }
    }
}
