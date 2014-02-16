using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;


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
        private error ErrorH;
       // private PipeServer pS;

        public Rectangle workingArea;

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
            Reg = new registry();

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

            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(userH.keyDown);
            
        }
        private void CreateNotifyicon()
        {/*
            this.components = new System.ComponentModel.Container();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();

            // Initialize menuItem1
            this.menuItem1.Index = 0;
            this.menuItem1.Text = "E&xit";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);

            // Initialize contextMenu1
            this.contextMenu1.MenuItems.AddRange(
                        new System.Windows.Forms.MenuItem[] { this.menuItem1 });*/

        }
        private void notifyIcon1_Click(object Sender, EventArgs e)
        {

            MessageBox.Show("clicked");
        }

        private void notifyIcon1_DoubleClick(object Sender, EventArgs e)
        {
            MessageBox.Show("Double clicked");
        }

        private void menuItem1_Click(object Sender, EventArgs e)
        {
            // Close the form, which closes the application.
            Application.Exit();
        }

        //Checks if mouse pointer is inside Form(this)
        private void MousePosCheck(object sender, EventArgs e)
        {
            if (this.DesktopBounds.Contains(Cursor.Position))
            {
                if ((this.formState == "info" || this.formState == "minimal") && this.formState != "incomming") //If form WAS minimized, and it isn't during an incomming call
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


            int h = 9;
            string fs = "";

            switch (state)
            {
                case "minimal":
                    h = 30;
                    fs = "minimal";
                break;
                case "incomming":
                    h = 150;
                    fs = "incomming";
                break;
                case "full":
                    h = 329;
                    fs = "full";
                break;
                
            }

            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)(() => this.workingArea = Screen.GetWorkingArea(this)));
                this.Invoke((MethodInvoker)(() => this.Height = h));
                this.Invoke((MethodInvoker)(() => this.formState = fs));
                this.Invoke((MethodInvoker)(() => this.Location = new Point(workingArea.Right - Size.Width, workingArea.Bottom - Size.Height)));

                switch (state)
                {
                    case "incomming":
                        this.Invoke((MethodInvoker)(() => this.pnl_dialer.Visible = false));
                        this.Invoke((MethodInvoker)(() => this.pnl_incomming.Visible = true));
                     break;
                }

            }
            else
            {
                workingArea = Screen.GetWorkingArea(this);
                this.Height = h;
                this.formState = fs;
                this.Location = new Point(workingArea.Right - Size.Width, workingArea.Bottom - Size.Height);
            }
            
            
           
        }


        public void setStatus(string t)
        {
            MessageBox.Show(t);
            if (this.InvokeRequired)
                this.lbl_status.Invoke((MethodInvoker)(() => this.lbl_status.Text = t));
            else
                this.lbl_status.Text = t;
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

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void lbl_l1_Click(object sender, EventArgs e)
        {

        }
    }
    class TextBoxWithoutCaret : TextBox
    {
        [DllImport("coredll.dll")]
        static extern bool HideCaret(IntPtr hwnd);

        [DllImport("coredll.dll")]
        static extern bool ShowCaret(IntPtr hwnd);

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            HideCaret(Handle);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            ShowCaret(Handle);
        }
    }
}
