using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace OTM_Client
{
    public class eventH
    {
        public string action { get; set; }
        public object data { get; set; }
        private Data c;
        private UDPlink udp;
        private frm_main form;
        private string homenumbers = @"(^\+[0-9]{2}|^\+[0-9]{2}\(0\)|^\(\+[0-9]{2}\)\(0\)|^00[0-9]{2}|^0)([0-9]{9}$|[0-9\-\s]{10}$)";
        private string mobilenumbers = @"(((\\+31|0|0031)6){1}[1-9]{1}[0-9]{7})";
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
                case "makecall": //Incomming command to make call from user (crtl + enter pressed)
                   this.makeCall();
                break;
            }
        }
        public void hookUDP(UDPlink u)
        {
            this.udp = u;
        }
        public void hookForm(frm_main f)
        {
            this.form = f;
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
            j.data.telnr = this.formatNumber(this.data + "");
            


            this.form.setTextLine(2, j.data.telnr);
            this.form.changeState("ringing");
            //this.udp.sendCMD(JsonConvert.SerializeObject(j));
        }
        private string formatNumber(string s)
        {
            if(s.StartsWith("06"))
            {
                return "+31 (" + s.Substring(0, 2) + ") " + s.Substring(2, 3) + " " + s.Substring(5, 2) + " " + s.Substring(7, 3);
            }
            else if(s.StartsWith("0"))
            {
                return s;
            }
            return s;
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
