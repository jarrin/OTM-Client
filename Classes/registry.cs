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
        error errorH;
        public registry(error e)
        {
            this.errorH = e;
            this.root = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\\OTM", true);

            if (this.root == null)
            {
                errorH.handle("Er is helaas een fout opgetreden. Kan de applicatie niet opstarten. \n (Registry keys niet gevonden)", 9);
            }
        }
        public string get(string n)
        {
            var val = this.root.GetValue(n).ToString();
            return val;
        }
    }
}
