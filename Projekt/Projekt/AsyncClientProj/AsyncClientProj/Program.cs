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
            //Vælg fælles porten inden Server & Klient kører
            Console.Write("Skriv port ind: ");
            int portInput = int.Parse(Console.ReadLine());

            Server(portInput);

            Klient(portInput);
        }

        static public async void Server(int portInput)
        {
            //Porten sættes ind fra variablen portInput som er brugerdefineret
            int port = portInput;

            //Connect til klienten
            IPAddress ip = IPAddress.Any;
            IPEndPoint localEndpoint = new IPEndPoint(ip, port);

            TcpListener listener = new TcpListener(localEndpoint);

            //Start tcp link
            listener.Start();

            //Venter på klienten connecter
            Console.WriteLine("Awaiting Clients");
            TcpClient client = await listener.AcceptTcpClientAsync();
            
            //Nu er den skiftet til klienten og venter på svar fra klientens side
            NetworkStream stream = client.GetStream();
            ReceiveMessage(stream);

            /*
            Console.WriteLine("Write your message here: ");
            string text = Console.ReadLine();
            byte[] buffer = Encoding.UTF8.GetBytes(text);

            stream.Write(buffer, 0, buffer.Length);
            */
            
            Console.ReadKey();
        }
        static public void Klient(int portInput)
        {
            TcpClient client = new TcpClient();

            //Connect til Serveren
            int port = portInput;
            IPAddress ip = IPAddress.Parse("172.16.115.82");
            IPEndPoint endPoint = new IPEndPoint(ip, port);

            client.Connect(endPoint);

            NetworkStream stream = client.GetStream();
            ReceiveMessage(stream);

            //Skriv den besked du vil sende
            Console.WriteLine("Write your message here: ");
            string text = Console.ReadLine();
            byte[] buffer = Encoding.UTF8.GetBytes(text);

            stream.Write(buffer, 0, buffer.Length);

            client.Close();
        }

        static public async void ReceiveMessage(NetworkStream stream)
        {
            byte[] buffer = new byte[256];
            
            //Funktionen her vil vente på den modtager beskeden også kører videre
            int numberOfBytesRead = await stream.ReadAsync(buffer, 0, 256);
            string receivedMessage = Encoding.UTF8.GetString(buffer, 0, numberOfBytesRead);

            Console.WriteLine($"\n {receivedMessage}");
        }
    }
}
