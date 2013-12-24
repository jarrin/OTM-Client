using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Pipes;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace OTM_Client
{
    // Delegate for passing received message back to caller
    class pipes
    {
        private bool running = true;

        private string pipeName;

        private NamedPipeServerStream pipeServer;
        private NamedPipeClientStream pipeClient;

        private Thread ClientThread, ServerThread;

        private Boolean connectedOrWaiting = false;

        private string sendString;

        public pipes(string pipeName, bool clientOnly)
        {
            construct(pipeName, clientOnly);

        }
        public pipes(string pipeName)
        {
            construct(pipeName, false);
        }

        private void construct(string pipeName, bool clientOnly)
        {
            //Starting threads:
            this.pipeName = pipeName;
            if (!clientOnly)
            {
                //Startserver
                ServerThread = new System.Threading.Thread(ThreadStartServer);
                ServerThread.Start();
            }

            ClientThread = new System.Threading.Thread(ThreadStartClient);
            ClientThread.Start();
        }
        private void ThreadStartServer()
        {
            pipeServer = new NamedPipeServerStream(this.pipeName, PipeDirection.InOut, 1, PipeTransmissionMode.Message, PipeOptions.Asynchronous);

            Byte[] buffer = new Byte[65535];

            while (running)
            {
                if (!connectedOrWaiting)
                {
                    pipeServer.BeginWaitForConnection((a) => { pipeServer.EndWaitForConnection(a); }, null);

                    connectedOrWaiting = true;
                }

                if (pipeServer.IsConnected)
                {
                    Int32 count = pipeServer.Read(buffer, 0, 65535);

                    if (count > 0)
                    {
                        UTF8Encoding encoding = new UTF8Encoding();
                        String strData = encoding.GetString(buffer, 0, count);
                        MessageBox.Show(strData);
                        eventH e = JsonConvert.DeserializeObject<eventH>(strData);
                        e.handle();

                    }

                    pipeServer.Disconnect();

                    connectedOrWaiting = false;
                }
            }

            Console.WriteLine("Connection lost");
        }
        public void dispose()
        {
            running = false;
            if(ClientThread != null)
                ClientThread.Abort();
            if (ServerThread != null)
                ServerThread.Abort();    
        }
        public void send(string s)
        {
            this.sendString = s;
        }
        public void send(JSONobject j)
        {
            string temp = JsonConvert.SerializeObject(j);
            MessageBox.Show(temp);
            this.sendString = temp;
        }
        private void ThreadStartClient()
        {
            // Ensure that we only start the client after the server has created the pipe
            //ManualResetEvent SyncClientServer = (ManualResetEvent)obj;
            pipeClient = new NamedPipeClientStream(this.pipeName);
                pipeClient.Connect();
                Debug.WriteLine("thread started");

            using (StreamWriter sw = new StreamWriter(pipeClient))
            {
                sw.AutoFlush = true;
                while (running)
                {
                    Debug.WriteLine(sendString);
                    if (sendString != null)
                    {
                        sw.WriteLine(sendString);
                        MessageBox.Show("x");
                        sendString = null;

                    }
                }

            }
            
        }

    }


}
