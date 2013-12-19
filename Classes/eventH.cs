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
        private currentCall c;
        public void handle()
        {
            switch (this.action)
            {
                case "callstart":  //Phone starts ringing
                    c = JsonConvert.DeserializeObject<currentCall>(this.data.ToString());
                    c.action = this.action;
                    this.incommingCall(c);
                break;
                case "callpickup": //Phone is picked up by user
                Debug.WriteLine(c.id + "");
                break;
                case "callend":  //Ongoing call has ended

                break;
            }
        }

        //Handles incomming calls
        private void incommingCall(currentCall c)
        {
            //set title
            Program.f.setStatus("Binnenkomend gesprek");  
            if (c.info != null && c.info.Count == 3)
            {
                //There's some extra info
            }
            else
            {
                //No extra info
            }
            Program.f.changeState("incomming");
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
