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
                case "callpickup": //Phone is picked up by user
                    currentCall c = JsonConvert.DeserializeObject<currentCall>(this.data.ToString());
                    c.action = this.action;
                    //c.handle(this);
                    Program.f.setStatus("Aan het bellen");
                break;
                case "callend":  //Ongoing call has ended

                break;
            }
        }
    }

    public class currentCall
    {

        public string action { get; set; }

        public int id { get; set; }
        public string telnr { get; set; }

        public List<string> info { get; set; }
    }

}
