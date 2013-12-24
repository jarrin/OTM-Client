using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Diagnostics;

namespace OTM_Client
{

    class registry
    {
        RegistryKey root;
        public registry()
        {
            this.root = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\\OTM", true);

            if (this.root == null)
            {
                Debug.WriteLine("Er is helaas een fout opgetreden. Kan de applicatie niet opstarten. \n (Registry keys niet gevonden)");
            }
        }
        public string get(string n)
        {
            var val = this.root.GetValue(n).ToString();
            return val;
        }
    }
}
