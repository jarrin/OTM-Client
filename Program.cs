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
            Program.f = new frm_main();
            using (Mutex mutex = new Mutex(false, appGuid))
            {
                if (!mutex.WaitOne(0, false))
                {
                    MessageBox.Show("An instance of the application already running");
                    return;
                }

                Application.Run(f);
            }
        }
       
        [StructLayout(LayoutKind.Sequential)]
        public struct COPYDATASTRUCT
        {
            [MarshalAs(UnmanagedType.I4)]
            public int dwData;
            [MarshalAs(UnmanagedType.I4)]
            public int cbData;
            [MarshalAs(UnmanagedType.SysInt)]
            public IntPtr lpData;


            unsafe protected override void WndProc(ref Message message)
            {
                if (message.Msg == WM_COPYDATA)
                {
                    COPYDATASTRUCT data = (COPYDATASTRUCT)
                        message.GetLParam(typeof(COPYDATASTRUCT));

                    string str = new string((char*)(data.lpData),
                        0, data.cbData / 2);

                    MessageBox.Show(str, "Data received",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                WndProc(ref message);
            }

            unsafe private void SendString()
            {
                string s = "test";
                IntPtr lpData = Marshal.StringToHGlobalUni(s);

                COPYDATASTRUCT data = new COPYDATASTRUCT();
                data.dwData = 0;
                data.cbData = s.Length * 2;
                data.lpData = lpData;

                IntPtr lpStruct = Marshal.AllocHGlobal(
                    Marshal.SizeOf(data));

                Marshal.StructureToPtr(data, lpStruct, false);

                int hTarget = 123123;

                SendMessage(hTarget, WM_COPYDATA,
                    this.Handle, lpStruct);
            }

            private void button1_Click(object sender, EventArgs e)
            {
                SendString();
            }

            public void handle(COPYDATASTRUCT c)
            {

            }

            [DllImport("User32.dll")]
            private static extern bool SendMessage(int hWnd,
                int wMsg, IntPtr wParam, IntPtr lParam);

        }
    }
}
