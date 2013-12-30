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
        public static string pipeName;
        private static NamedPipeServerStream pipeServer;
        private static readonly int BufferSize = 256;



        public static void createPipeServer()
        {
            Decoder decoder = Encoding.Default.GetDecoder();
            Byte[] bytes = new Byte[BufferSize];
            char[] chars = new char[BufferSize];
            int numBytes = 0;
            StringBuilder msg = new StringBuilder();

            try
            {
                pipeServer = new NamedPipeServerStream(pipeName, PipeDirection.In, 1,
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
                            Debug.WriteLine(msg.ToString());
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
        public static void createPipeClient()
        {
            using (NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", pipeName, PipeDirection.Out, PipeOptions.Asynchronous))
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
                    sw.WriteLine("testZender");
                }
            }
        }
    }
}
