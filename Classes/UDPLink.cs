using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json;

namespace OTM_Client
{
    public class UDPlink
    {
        private int sPort, cPort;
        private string sHost;

        private IPEndPoint RemoteEndPoint;
        private Socket server;

        private bool running = true;

        public UDPlink(string h, int sp, int cp)
        {
            //Set Settings
            this.sPort = sp;
            this.sHost = h;
            this.cPort = cp;

            //Init listener & sender
            this.initServer();
        }

        private void initServer()
        {
            //Starting listener in seperated thread.
            ThreadStart start = () =>
            {
                StartListener(this.cPort);
            };
            Thread receiveThread = new Thread(start);
            receiveThread.Start();

            //Setting up sender
            RemoteEndPoint = new IPEndPoint(
                                        IPAddress.Parse(this.sHost),
                                        this.sPort);
            server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        }

        private void send(string m)
        {
            var data = Encoding.ASCII.GetBytes(m);
            server.SendTo(data, data.Length, SocketFlags.None, RemoteEndPoint);
        }

        public static void StartListener(int port)
        {
            
            UdpClient client = new UdpClient();
 
            client.ExclusiveAddressUse = false;
            IPEndPoint localEp = new IPEndPoint(IPAddress.Any, port);
 
            client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            client.ExclusiveAddressUse = false;
 
            client.Client.Bind(localEp);
 
            //IPAddress multicastaddress = IPAddress.Parse("239.0.0.222");
            //client.JoinMulticastGroup(multicastaddress);
             
 
            while (true)
            {
                Byte[] data = client.Receive(ref localEp);
                
                string strData = Encoding.ASCII.GetString(data);

                eventH e = JsonConvert.DeserializeObject<eventH>(strData);
                e.handle();

            }
           
        }

    }
}
