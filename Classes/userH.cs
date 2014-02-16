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

        public static void keyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                //Modifier key used
                if (e.KeyCode == Keys.Enter)
                {

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
            }
        }

        private static bool IsKeyADigit(Keys key)
        {
            return (key >= Keys.D0 && key <= Keys.D9) || (key >= Keys.NumPad0 && key <= Keys.NumPad9);
        }
    }
}
