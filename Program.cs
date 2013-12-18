using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace OTM_Client
{
    static class Program
    {
        public static frm_main f;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Program.f = new frm_main();
            Application.Run(Program.f);
        }
    }
}
