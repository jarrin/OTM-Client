using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace OTM_Client
{
    class userH
    {
        private frm_main f;
        private UDPlink u;
        public userH(frm_main f, UDPlink u)
        {
            this.f = f;
            this.u = u;
        }
        public void keyDown(object sender, KeyEventArgs e)
        {
            
            if (e.Control)
            {
                //Modifier key used
                if (e.KeyCode == Keys.Return)
                {
                    Debug.WriteLine("CTRL+Enter");
                    switch (f.formState)
                    {
                        case "dialing":
                            eventH eh = new eventH();
                            eh.action = "makecall";
                            eh.data = this.f.getNumber();
                            eh.hookUDP(u);
                            eh.hookForm(this.f);
                            eh.handle();
                            break;
                    }
                }
                else if (e.KeyCode == Keys.Back)
                {
                    if (this.f.formState == "dialing")
                    {
                        this.f.changeState("full");
                    }
                }



            }
            else if (userH.IsKeyADigit(e.KeyData))
            {
                int val = 0;
                if (e.KeyValue >= 48 && e.KeyValue <= 57)
                {
                    val = e.KeyValue - 48;
                }
                else if(e.KeyValue >= 96 && e.KeyValue <= 105)
                {
                    val = e.KeyValue - 96;
                }
                Debug.WriteLine(val);
                f.addNumber(val);
            }
            else if(e.KeyData == Keys.Back)
            {
                f.removeNumber();
            }
            else if (e.KeyData == Keys.Return)
            {
                e.Handled = true;
                switch (f.formState)
                {
                    case "dialing":
                        eventH eh = new eventH();
                        eh.action = "makecall";
                        eh.data = this.f.getNumber();
                        eh.hookUDP(u);
                        eh.hookForm(this.f);
                        eh.handle();
                        break;
                }
            }
        }
        private static bool IsKeyADigit(Keys key)
        {
            return (key >= Keys.D0 && key <= Keys.D9) || (key >= Keys.NumPad0 && key <= Keys.NumPad9);
        }
    }
}
