using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using Newtonsoft.Json;
namespace OTM_Client
{
    static class Program
    {
        public static frm_main f;
        static string appGuid = ((GuidAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), false).GetValue(0)).Value.ToString();
        public const int WM_COPYDATA = 0x004a;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // get application GUID as defined in AssemblyInfo.cs


            // unique id for global mutex - Global prefix means it is global to the machine
            string mutexId = string.Format("Global\\{{{0}}}", appGuid);

            using (var mutex = new Mutex(false, mutexId))
            {
                try
                {
                    if (!mutex.WaitOne(0, false))
                    {
                        //signal existing app via named pipes
                        JSONobject j = new JSONobject();
                        j.action = "dial";
                        j.data = new Data();
                        j.data.telnr = args[0].Remove(0, 4).Trim('/');
                        
                        Pipe p  = new Pipe("OTMPipe");
                        p.send(JsonConvert.SerializeObject(j));
                        Environment.Exit(0);
                    }
                    else
                    {
                        // handle protocol with this instance   
                        Program.f = new frm_main();
                        Application.Run(f);

                    }
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            }
        }
    }     
}