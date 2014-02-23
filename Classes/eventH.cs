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
        private Data c;
        private UDPlink udp;
        public void handle()
        {
            Debug.WriteLine("asd");
            switch (this.action)
            {
                case "callstart":  //Phone starts ringing
                    c = JsonConvert.DeserializeObject<Data>(this.data.ToString());
                    this.incommingCall();
                break;
                case "callpickup": //Phone is picked up by user

                break;
                case "callend":  //Ongoing call has ended

                break;
                case "makecall": //Incomming command to make call
                   this.makeCall();
                break;
            }
        }
        public void hookUDP(UDPlink u)
        {
            this.udp = u;
        }

        //Handles incomming calls
        private void incommingCall()
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

        private void makeCall()
        {
            Debug.WriteLine("Making call to " + this.data);
            JSONobject j = new JSONobject();
            j.action = "dial";
            j.data = new Data();
            j.data.telnr = this.data + "";

            this.udp.sendCMD(JsonConvert.SerializeObject(j));
        }
    
    }

    //C# equivelant of the JSON Objects

    public class JSONobject
    {
        public string action { get; set; }
        public Data data { get; set; }
    }
    public class Data
    {
        public string telnr { get; set; }

        public List<string> info { get; set; }
    }
}
