using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace AsyncClientProj
{
    class Program
    {
        static void Main(string[] args)
        {
            Server();

            Klient();
        }

        static public async void Server()
        {
            int port = 14000;
            IPAddress ip = IPAddress.Any;
            IPEndPoint localEndpoint = new IPEndPoint(ip, port);

            TcpListener listener = new TcpListener(localEndpoint);

            listener.Start();

            Console.WriteLine("Awaiting Clients");
            TcpClient client = await listener.AcceptTcpClientAsync();

            NetworkStream stream = client.GetStream();
            ReceiveMessage(stream);

            /*Console.WriteLine("Write your message here: ");
            string text = Console.ReadLine();
            byte[] buffer = Encoding.UTF8.GetBytes(text);

            stream.Write(buffer, 0, buffer.Length);*/

            Console.ReadKey();
        }
        static public void Klient()
        {
            TcpClient client = new TcpClient();

            int port = 14000;
            IPAddress ip = IPAddress.Parse("172.16.115.82");
            IPEndPoint endPoint = new IPEndPoint(ip, port);

            client.Connect(endPoint);

            NetworkStream stream = client.GetStream();
            ReceiveMessage(stream);

            Console.WriteLine("Write your message here: ");
            string text = Console.ReadLine();
            byte[] buffer = Encoding.UTF8.GetBytes(text);

            stream.Write(buffer, 0, buffer.Length);

            client.Close();
        }

        static public async void ReceiveMessage(NetworkStream stream)
        {
            byte[] buffer = new byte[256];

            int numberOfBytesRead = await stream.ReadAsync(buffer, 0, 256);
            string receivedMessage = Encoding.UTF8.GetString(buffer, 0, numberOfBytesRead);

            Console.WriteLine($"\n {receivedMessage}");
        }
    }
}
