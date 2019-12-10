using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace ClientMessage
{
    class Program
    {
        static void Main(string[] args)
        {
            //Vælg om du skal være klient eller server
            Console.WriteLine("Skriv 'Server' eller 'Klient' hvis ingenting bliver skrevet ind kommer du ind som 'Klient'");
            string ClientOrServer = Console.ReadLine().ToLower().Trim();
            
            if (ClientOrServer.Contains("server"))
            {
                ServerProgram();
            }
            else
            {
                ClientProgram();
            }  
        }


        static void ServerProgram()
        {
            //Angiv portnummer som vi skal kigge efter clients på
            Console.Write("Enter port number: ");
            int portnumber = Int32.Parse(Console.ReadLine());
            //Connect til client
            int port = portnumber;
            IPAddress ip = IPAddress.Any;
            IPEndPoint localEndpoint = new IPEndPoint(ip, port);

            TcpListener listener = new TcpListener(localEndpoint);
            //Starter tcp link
            listener.Start();
            //Venter på connceting
            Console.WriteLine("Awaiting Clients");
            TcpClient client = listener.AcceptTcpClient();

            NetworkStream stream = client.GetStream();
            string messageRespond = "";
            string message1 = "";
            while (messageRespond != "dø" || message1 != "dø")
            {
                byte[] buffer = new byte[512];
                try
                {
                    int numberOfBytesRead = stream.Read(buffer, 0, 512);
                    message1 = Encoding.UTF8.GetString(buffer, 0, numberOfBytesRead);
                }
                catch (Exception)
                {
                    return;
                }


                Console.WriteLine(message1);

                messageRespond = Console.ReadLine();     //Besked indhold
                if (messageRespond == "dø")
                {
                    return;
                }
                byte[] buffer1 = Encoding.UTF8.GetBytes(messageRespond);    //Besked i bytes
                try
                {
                    stream.Write(buffer1, 0, buffer1.Length);   //Sender besked 
                }
                catch (Exception)
                {
                    return;
                }
            }
            return;
        }


        static void ClientProgram()
        {
            TcpClient client = new TcpClient();

            //Connect til Server/Client2
            Console.WriteLine("Indtast port: ");
            int portCR = int.Parse(Console.ReadLine());
            Console.WriteLine("Indtast ip'en på serveren du vil connecte til: ");
            string ipCR = Console.ReadLine();
            int port = portCR;
            IPAddress ip = IPAddress.Parse(ipCR); //Jeppe's ip 172.16.115.86 Mathias's ip 172.16.115.82
            IPEndPoint endpoint = new IPEndPoint(ip, port);

            client.Connect(endpoint);

            NetworkStream stream = client.GetStream();
            Console.WriteLine("Connected");
            string input = "";
            string message = "";
            while (input != "dø" || message != "dø")
            {
                //Skriv den besked du vil sende
                input = Console.ReadLine();
                if (input == "dø")
                {
                    return;
                }
                string text = input;

                byte[] buffer = Encoding.UTF8.GetBytes(text);

                stream.Write(buffer, 0, buffer.Length);

                //Modtag en ny besked fra Server/Client2
                byte[] buffer1 = new byte[512];
                try
                {
                    int numberOfBytesRead = stream.Read(buffer1, 0, 512);

                    message = Encoding.UTF8.GetString(buffer1, 0, numberOfBytesRead);
                }
                catch (Exception)
                {
                    return;
                }

                Console.WriteLine(message);

            }
            return;
        }
    }
}
