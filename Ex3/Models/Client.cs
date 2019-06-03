using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace Ex3.Models
{
    public class Client
    {
        public bool IsConnected { get; set; }
        Socket soc;

        // instance for singleton pattern
        private static Client instance = null;

        private Client()
        {
            this.IsConnected = false;
        }

        /* instance method for singleton pattern */
        public static Client Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Client();
                }
                return instance;
            }
        }

        /* open server */
        public void ConnectToServer(string IP, int port)
        {
            soc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            while (!soc.Connected)
            {
                try
                {
                    IPAddress ipAdd = IPAddress.Parse(IP);
                    IPEndPoint remoteEP = new IPEndPoint(ipAdd, port);
                    soc.Connect(remoteEP);
                }
                catch (SocketException)
                {
                    Console.WriteLine("Waiting for connection...");
                    continue;
                }
            }
            IsConnected = true;
        }

        /* write to server */
        public void WriteToServer(String line)
        {
            if (IsConnected)
                {
                        byte[] lineWithEnter = System.Text.Encoding.ASCII.GetBytes(line + "\r\n");
                        try
                        {
                            soc.Send(lineWithEnter);
                        }
                        catch
                        {
                            Console.WriteLine("Writing to server failed");
                            CloseClient();
                            return;
                        }
                }
                else
                {
                    Console.WriteLine("Client not connected. Cannot send data!");
                }
        }

        /* read answer from server */
        public string ReadAnswerFromServer()
        {
            byte[] buffer = new byte[512];
            soc.Receive(buffer);
            string bufferStr = System.Text.Encoding.ASCII.GetString(buffer);
            return bufferStr;
        }

        /* close client */
        public void CloseClient()
        {
            soc.Close();
            IsConnected = false;
            System.Diagnostics.Debug.WriteLine("Client closed!");
        }
    }
}