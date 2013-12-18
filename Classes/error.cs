using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;//Mandatory
using System.Windows.Forms; 


namespace OTM_Client
{
    class error
    {
        private EventLog log;
        private Form f;
        public error()
        {
  
        }

        public void handle(string Message, int level)
        {
            MessageBox.Show(Message);
            switch (level)
            {
                case 9:
                    //this.f.Close();
                break;
            }
        }
    }
}
