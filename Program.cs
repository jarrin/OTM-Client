using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace OTM_Client
{
    static class Program
    {
        public static frm_main f;
        static string appGuid = "{8F6F0AC4-B9A1-45fd-A8CF-72F04E6BDE8F}";
        public const int WM_COPYDATA = 0x004a;


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            using (Mutex mutex = new Mutex(false, appGuid))
            {
                //if (!mutex.WaitOne(0, false))
                //{
                    registry Reg = new registry();
                    string pipeName = Reg.get("namedPipeName");
                    pipes Client = new pipes(pipeName, true);

                    //-- Send object
                    JSONobject temp = new JSONobject();
                    temp.action = "makecall";
                    Data d = new Data();
                    d.telnr = "0654545454";
                    temp.data = d;
                    //--

                    Client.send("testxd");
                    //Client.dispose();

                    Application.Exit();
                    return;
               // }
               // Program.f = new frm_main();
             //   Application.Run(f);
            }
        }
      
    }
}
