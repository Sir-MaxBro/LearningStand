using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace StandServer
{
    class Program
    {
        private const string IpAddress = "127.0.0.1";

        static void Main(string[] args)
        {
            var program = new Program();
            program.StartClient(23); // start telnet client
            program.StartClient(22); // start ssh client
            Console.ReadLine();
        }

        private async Task StartClient(int port)
        {
            IPAddress localAdd = IPAddress.Parse(IpAddress);
            TcpListener listener = new TcpListener(localAdd, port);
            Console.WriteLine("Listening...");
            listener.Start();
            while (true)
            {
                //---incoming client connected---
                TcpClient client = listener.AcceptTcpClient();

                //---get the incoming data through a network stream---
                NetworkStream nwStream = client.GetStream();
                byte[] buffer = new byte[client.ReceiveBufferSize];

                //---read incoming stream---
                int bytesRead = await Task.Run(() => nwStream.Read(buffer, 0, client.ReceiveBufferSize));

                //---convert the data received into a string---
                string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Received: " + dataReceived.TrimEnd('\n', '\r'));

                //---write back the text to the client---
                Console.WriteLine("Sending back: " + dataReceived.TrimEnd('\n', '\r') + " response");
                nwStream.Write(buffer, 0, bytesRead);
                client.Close();
            }

            listener.Stop();
        }
    }
}
