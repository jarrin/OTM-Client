using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Newtonsoft.Json;

namespace OTM_Client
{
    public class eventH
    {
        public string action { get; set; }
        public object data { get; set; }

        public void handle()
        {
            Debug.WriteLine("cx");
            switch (this.action)
            {
                case "callstart":  //Phone starts ringing
                    currentCall c = JsonConvert.DeserializeObject<currentCall>(this.data.ToString());
                    c.action = this.action;
                    this.incommingCall(c);
                break;
                case "callpickup": //Phone is picked up by user

                break;
                case "callend":  //Ongoing call has ended

                break;
            }
        }

        //Handles incomming calls
        private void incommingCall(currentCall c)
        {
            Program.f.setStatus("Binnenkomend gesprek");
            if (c.info.Count == 3)
            {
            }
            else
            {
                Program.f.ErrorH.handle("Inkomend gesprek had geen 3 info lijnen.", 1);
            }
        }
    
    }

    //C# equivelant of the JSON Objects

    public class currentCall
    {

        public string action { get; set; }

        public int id { get; set; }
        public string telnr { get; set; }

        public List<string> info { get; set; }
    }

}
