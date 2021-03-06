﻿using System;
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
        public string formState; //State: Minimal, info and full
        private bool mouseInForm; //Is the mouse inside this form?


        //Own classes
        private UDPlink Con;
        private registry Reg;
        private error ErrorH;
        private userH userH;
        private hotkey hk;
        private Pipe P;

        public Rectangle workingArea;

        //Settings object.
        public Dictionary<string, string> settings = new Dictionary<string, string>
        {
            {"serverPort", null},
            {"serverHost", null},
            {"clientPort", null}
        };    

        public frm_main()
        {
            Reg = new registry();
            Reg.setUriReg("OTM");

            this.settings["serverPort"] = Reg.get("serverPort");
            this.settings["serverHost"] = Reg.get("serverHost");
            this.settings["clientPort"] = Reg.get("clientPort");
            InitializeComponent();
            this.setTextLine(1, "Kies een nummer...");
            this.setTextLine(2, "");
            this.setTextLine(3, "");
            this.setTextLine(4, "");
            this.changeState("minimal");
            ErrorH = new error();

            hk = new hotkey(Constants.ALT + Constants.SHIFT, Keys.O, this);
            hk.Register();
            //Load settings from registry.


           
 
            //Enable Timer
            mpCheckTimer = new Timer();
            mpCheckTimer.Interval = 200;
            mpCheckTimer.Tick += MousePosCheck;
            mpCheckTimer.Enabled = true;



            
        }
        private void HandleHotkey()
        {
            MessageBox.Show("Hotkey pressed!");
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
                    mouseInForm = true; //Mouse is inside
                    this.changeState("full"); //Then change it to it's full state
                }
            }
            else
            {
                mouseInForm = false; //Mouse is outside

            }
        }

        //Updates reg with new this.Settings
        public void updateReg()
        {
            this.Reg.set("serverPort", this.settings["serverPort"]);
            this.Reg.set("serverHost", this.settings["serverHost"]);
            this.Reg.set("clientPort", this.settings["clientPort"]);
        }
        //Changes form size & position
        public void changeState(string state)
        {


            int h = 9;
            string fs = "";
            Debug.WriteLine(state);
            switch (state)
            {
                default:
                case "minimal":
                    h = 30;
                    fs = "minimal";
                    break;
                case "incomming":
                    h = 150;
                    fs = state;
                    break;
                case "full":
                case "dialing":
                    h = 329;
                    fs = state;
                    break;
                case "ringing":
                    h = 150;
                    fs = state;
                   
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
                        this.Invoke((MethodInvoker)(() => this.brn_minimize.Visible = true));
                        this.Invoke((MethodInvoker)(() => this.pnl_dialer.Visible = false));
                        this.Invoke((MethodInvoker)(() => this.pnl_incomming.Visible = true));
                        this.Invoke((MethodInvoker)(() => this.btn_pickUp.Visible = true));
                        this.Invoke((MethodInvoker)(() => this.btn_settings.Image = global::OTM_Client.Properties.Resources.framework_settings));
                        break;
                    case "dialing":
                        this.Invoke((MethodInvoker)(() => this.brn_minimize.Visible = true));
                        this.Invoke((MethodInvoker)(() => this.pnl_incomming.Visible = false));
                        this.Invoke((MethodInvoker)(() => this.setTextLine(1, "")));
                        this.Invoke((MethodInvoker)(() => this.setTextLine(4, "CTRL+ENTER om te bellen")));
                        this.Invoke((MethodInvoker)(() => this.btn_settings.Image = global::OTM_Client.Properties.Resources.hangup));
                        break;
                    case "ringing":
                        this.Invoke((MethodInvoker)(() => this.brn_minimize.Visible = false));
                        this.Invoke((MethodInvoker)(() => this.pnl_incomming.Visible = true));
                        this.Invoke((MethodInvoker)(() => this.btn_pickUp.Visible = false));
                        this.Invoke((MethodInvoker)(() => this.setTextLine(1, "Gaat over..")));
                        this.Invoke((MethodInvoker)(() => this.setTextLine(4, "")));
                        this.Invoke((MethodInvoker)(() => this.btn_settings.Image = global::OTM_Client.Properties.Resources.hangup));
                        break;
                    case "full":
                        this.Invoke((MethodInvoker)(() => this.brn_minimize.Visible = true));
                        this.Invoke((MethodInvoker)(() => this.pnl_incomming.Visible = false));
                        
                        this.Invoke((MethodInvoker)(() => this.btn_settings.Image = global::OTM_Client.Properties.Resources.framework_settings));
                        this.setTextLine(1, "Kies een nummer...");
                        this.setTextLine(4, "");
                        break;
                    case "minimal":
                        this.Invoke((MethodInvoker)(() => this.brn_minimize.Visible = false));
                        break;
                }

            }
            else
            {
                workingArea = Screen.GetWorkingArea(this);
                this.Height = h;
                this.formState = fs;
                this.Location = new Point(workingArea.Right - Size.Width, workingArea.Bottom - Size.Height);
                switch (state)
                {
                    case "incomming":
                        this.brn_minimize.Visible = true;
                        this.pnl_dialer.Visible = false;
                        this.pnl_incomming.Visible = true;
                        this.btn_pickUp.Visible = true;
                        this.btn_settings.Image = global::OTM_Client.Properties.Resources.framework_settings;
                        break;
                    case "dialing": //dialing a number
                        this.brn_minimize.Visible = true;
                        this.setTextLine(1, "");
                        this.setTextLine(4, "CTRL+ENTER om te bellen");
                        this.pnl_incomming.Visible = false;
                        this.btn_settings.Image = global::OTM_Client.Properties.Resources.hangup;
                        break;
                    case "ringing": //Phone is ringing to a number
                        this.brn_minimize.Visible = false;
                        this.pnl_incomming.Visible = true;
                        this.btn_pickUp.Visible = false;
                        this.setTextLine(1, "Gaat over...");
                        this.setTextLine(4, "");
                        this.btn_settings.Image = global::OTM_Client.Properties.Resources.hangup;
                        break;
                    case "full":
                        this.brn_minimize.Visible = true;
                        this.pnl_incomming.Visible = false;
                        this.btn_settings.Image = global::OTM_Client.Properties.Resources.framework_settings;
                        this.setTextLine(1, "Kies een nummer...");
                        this.setTextLine(4, "");
                        break;
                    case "minimal":
                        this.brn_minimize.Visible = false;
                        break;

                }
            }
            
            
           
        }
        public void setFocus()
        {
            if (this.InvokeRequired)
                this.Invoke((MethodInvoker)(() => this.BringToFront()));
            else
                this.BringToFront();
        }
        public void setStatus(string t)
        {
            if (this.InvokeRequired)
                this.lbl_status.Invoke((MethodInvoker)(() => this.lbl_status.Text = t));
            else
                this.lbl_status.Text = t;
        }

        public void setNumber(string n)
        {
            if (this.InvokeRequired)
            {
                this.lbl_1.Invoke((MethodInvoker)(() => this.lbl_1.Text = n));
            }
            else
            {
                this.lbl_1.Text = n;
            }
            
        }
        public void addNumber(int n)
        {
            if (this.formState != "dialing")
            {
                this.changeState("dialing");
                this.lbl_1.Text = "";
            }
            this.lbl_1.Text += n;
            if (this.lbl_1.Text.Length >= 20)
            {
                this.lbl_1.Font = new Font(this.lbl_1.Font.Name, 10);
            }
            else
            {
                this.lbl_1.Font = new Font(this.lbl_1.Font.Name, 14);
            }
            this.lbl_1.Focus();

        }
        public void removeNumber()
        {
            if (this.lbl_1.Text != "Kies een nummer...")
            {
                if (this.lbl_1.Text.Length > 0) this.lbl_1.Text = this.lbl_1.Text.Remove(this.lbl_1.Text.Length - 1);
                if (this.lbl_1.Text.Length == 0) this.changeState("full");
                if (this.lbl_1.Text.Length >= 20)
                {
                    this.lbl_1.Font = new Font(this.lbl_1.Font.Name, 10);
                }
                else
                {
                    this.lbl_1.Font = new Font(this.lbl_1.Font.Name, 14);
                }
            }

        }
        public string getNumber()
        {
            return this.lbl_1.Text;
        }

        public void setTextLine(int line, string s)
        {
            if (this.InvokeRequired)
                switch (line)
                {
                    case 1:
                        this.lbl_1.Invoke((MethodInvoker)(() => this.lbl_1.Text = s));
                        break;
                    case 2:
                        this.lbl_2.Invoke((MethodInvoker)(() => this.lbl_2.Text = s));
                        break;
                    case 3:
                        this.lbl_3.Invoke((MethodInvoker)(() => this.lbl_3.Text = s));
                        break;
                    case 4:
                        this.lbl_4.Invoke((MethodInvoker)(() => this.lbl_4.Text = s));
                        break;

                }
            else
            {
                switch (line)
                {
                    case 1:
                        this.lbl_1.Text = s;
                        break;
                    case 2:
                        this.lbl_2.Text = s;
                        break;
                    case 3:
                        this.lbl_3.Text = s;
                        break;
                    case 4:
                        this.lbl_4.Text = s;
                        break;


                }
            }
        }


        private void frm_main_Load(object sender, EventArgs e)
        {
            Boolean success = frm_main.RegisterHotKey(this.Handle, this.GetType().GetHashCode(), 0x0000, 0x42);//Set hotkey as 'b'Boolean success = Form1.RegisterHotKey(this.Handle, this.GetType().GetHashCode(), 0x0000, 0x42);//Set hotkey as 'b'
            this.lbl_1.Focus();
            if(this.settings["serverPort"] == null || this.settings["serverHost"] == null || this.settings["clientPort"] == null)
            {
                this.changeState("full");
                frm_settings f = new frm_settings();
                f.Show();
                MessageBox.Show("Er ontbreken instellingen!");
            }
            else
            {
                Con = new UDPlink(this.settings["serverHost"], Convert.ToInt32(this.settings["serverPort"]), Convert.ToInt32(this.settings["clientPort"]));
                Con.subscribeMatchMaker();
                userH = new userH(this, Con);
                P = new Pipe("OTMPipe");
                P.form = this;
                P.createPipeServer();

                this.KeyPreview = true;
                this.KeyDown += new KeyEventHandler(userH.keyDown);
            }

        }
        public void closeApplication()
        {
            Environment.Exit(0);
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

        private void brn_settings_Click(object sender, EventArgs e)
        {
            if (this.formState == "full")
            {
                frm_settings f = new frm_settings();
                f.Show();
            }
            else if(this.formState == "dialing")
            {
                this.changeState("full");
            }
        }

        /// <summary>
        /// Numbers
        /// </summary>
        private void btn_0_Click(object sender, EventArgs e)
        {
            addNumber(0);
            this.Con.subscribeMatchMaker();
        }
        private void btn_1_Click(object sender, EventArgs e)
        {
            addNumber(1);
        }
        private void btn_2_Click(object sender, EventArgs e)
        {
            addNumber(3);
        }
        private void btn_3_Click(object sender, EventArgs e)
        {
            addNumber(3);
        }
        private void btn_4_Click(object sender, EventArgs e)
        {
            addNumber(4);
        }
        private void btn_5_Click(object sender, EventArgs e)
        {
            addNumber(5);
        }
        private void btn_6_Click(object sender, EventArgs e)
        {
            addNumber(6);
        }
        private void btn_7_Click(object sender, EventArgs e)
        {
            addNumber(7);
        }
        private void btn_8_Click(object sender, EventArgs e)
        {
            addNumber(8);
        }
        private void btn_9_Click(object sender, EventArgs e)
        {
            addNumber(9);
        }

        private void menu_close_Click(object sender, EventArgs e)
        {
            this.notifyIcon.Visible = false;
            this.closeApplication();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.changeState("minimal");
        }

        private void button11_Click_1(object sender, EventArgs e)
        {
            switch (this.formState)
            {
                case "dialing":
                    eventH eh = new eventH();
                    eh.action = "makecall";
                    eh.data = this.getNumber();
                    eh.hookUDP(this.Con);
                    eh.hookForm(this);
                    eh.handle();
                    break;
            }
        }
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
        //[DllImport("user32.dll")]
        //public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312)
            {
                MessageBox.Show("Catched");//You can replace this statement with your desired response to the Hotkey.
            }
            base.WndProc(ref m);
        }
    }

}
