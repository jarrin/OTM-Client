using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Diagnostics;
using System.Windows.Forms;
using System.Security.Permissions;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace OTM_Client
{

    class registry
    {
        RegistryKey root, uriRoot;
        public registry()
        {
            this.root = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\\OTM", true);
            this.uriRoot = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\\Classes", true);

            if (this.root == null)
            {
                Debug.WriteLine("Er is helaas een fout opgetreden. Kan de applicatie niet opstarten. \n (Registry keys niet gevonden)");
            }
        }
        public string get(string n)
        {
            try
            {
                var val = this.root.GetValue(n).ToString();
                return val;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Er is helaas een fout opgetreden. Kan de applicatie niet opstarten. \n (Registry keys niet gevonden)");
                return null;
            }
            
            
        }
        public void set(string key, string n)
        {
            this.root.SetValue(key, n);
        }

        public void setUriReg(string uriname)
        {
            //Check's and set's the URI (OTM:12341`)
            RegistryKey uriKey;
            uriKey = this.uriRoot.OpenSubKey(uriname, true);
            if (uriKey == null)
            {
                this.uriRoot.CreateSubKey(uriname);
                uriKey = this.uriRoot.OpenSubKey(uriname, true);

                uriKey.SetValue("", "URL:"+uriname+" Protocol");
                uriKey.SetValue("URL Protocol", "");
                uriKey = uriKey.CreateSubKey("shell");
                uriKey = uriKey.CreateSubKey("open");
                uriKey = uriKey.CreateSubKey("command");
                uriKey.SetValue("", "\""+ Application.StartupPath +"\\OTM-Client.exe\" \"%1\"");
            }

            //Opening chrome localstate file
            uriname = uriname.ToLower();
            var pathWithEnv = @"%LOCALAPPDATA%\Google\Chrome\User Data\Local State";
           
            var filePath = Environment.ExpandEnvironmentVariables(pathWithEnv);
            
            string json = File.ReadAllText(filePath);
            
            JObject o = JObject.Parse(json);
            var schemes = o["protocol_handler"]["excluded_schemes"];
            if (schemes[uriname] == null)
            {
                schemes[uriname] = false;
                o["protocol_handler"]["excluded_schemes"] = schemes;

                json = o.ToString();
                File.WriteAllText(filePath, json);
            }  
        }
    }
}
