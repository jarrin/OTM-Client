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

    public class Pipe
    {
        public string pipeName;
        private static readonly int BufferSize = 256;
        public frm_main form {get; set; }
        public Pipe(string pn)
        {
            this.pipeName = pn;
        }


        public void createPipeServer()
        {
            ThreadStart start = () =>
            {
                startServer(this.pipeName, this.form);
            };
            Thread receiveThread = new Thread(start);
            receiveThread.Start();

        }
        private static void startServer(string pn, frm_main f)
        { 
        
            Decoder decoder = Encoding.Default.GetDecoder();
            Byte[] bytes = new Byte[BufferSize];
            char[] chars = new char[BufferSize];
            int numBytes = 0;
            StringBuilder msg = new StringBuilder();
             NamedPipeServerStream pipeServer;
            try
            {
                pipeServer = new NamedPipeServerStream(pn, PipeDirection.In, 1,
                                                       PipeTransmissionMode.Message,
                                                       PipeOptions.Asynchronous);
                while (true)
                {
                    pipeServer.WaitForConnection();

                    do
                    {
                        msg.Length = 0;
                        do
                        {
                            numBytes = pipeServer.Read(bytes, 0, BufferSize);
                            if (numBytes > 0)
                            {
                                int numChars = decoder.GetCharCount(bytes, 0, numBytes);
                                decoder.GetChars(bytes, 0, numBytes, chars, 0, false);
                                msg.Append(chars, 0, numChars);
                            }
                        } while (numBytes > 0 && !pipeServer.IsMessageComplete);
                        decoder.Reset();
                        if (numBytes > 0)
                        {
                            //MESSAGE O
                            string json = msg.ToString();
                            JSONobject a = JsonConvert.DeserializeObject<JSONobject>(json);
                            switch (a.action)
                            {
                                case "dial":
                                    Debug.WriteLine("Call command from namedPipe...");                                   
                                    f.changeState("dialing");
                                    f.setFocus();
                                    f.setNumber(a.data.telnr);
                                    break;
                            }
       
                            //ownerInvoker.Invoke(msg.ToString());
                        }
                    } while (numBytes != 0);
                    pipeServer.Disconnect();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void createPipeClient(string s)
        {
            using (NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", this.pipeName, PipeDirection.Out, PipeOptions.Asynchronous))
            {
                try
                {
                    pipeClient.Connect(2000);
                }
                catch
                {
                    MessageBox.Show("The Pipe server must be started in order to send data to it.");
                    return;
                }
                using (StreamWriter sw = new StreamWriter(pipeClient))
                {
                    sw.WriteLine(s);
                }
            }
        }
        public void send(string s)
        {
            this.createPipeClient(s);
            
        }

    }
}
