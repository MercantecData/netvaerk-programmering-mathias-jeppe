using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace Multi_peer_server
{
    class Program
    {
        static void Main(string[] args)
        {
            List<TcpClient> clients = new List<TcpClient>();
         
            MyServer();
            
            
            void MyServer()
            {
                //Connect til server
                //IPAddress ip = IPAddress.Parse("127.0.0.1");
                IPAddress ip = IPAddress.Any;
                int port = 14000;
                TcpListener listener = new TcpListener(ip, port);
                listener.Start();

                //AcceptClients(listener);

                bool isRunning = true;
                while (isRunning)
                {
                    AcceptClients(listener);
                    //Send en besked
                    //string brugernavn = Console.ReadLine();
                    //byte[] buffer2 = Encoding.UTF8.GetBytes(brugernavn);
                    //Console.WriteLine(clients[1]);
                    //clients.ToString().FindIndex(brugernavn);
                    /*
                    foreach (TcpClient client in clients)
                    {
                        client.GetStream().Write(buffer2, 0, buffer2.Length);
                    }
                    */
                    Console.Write("Write message: ");
                    string text = Console.ReadLine();
                    byte[] buffer = Encoding.UTF8.GetBytes(text);


                    foreach (TcpClient client in clients)
                    {
                        try { client.GetStream().Write(buffer, 0, buffer.Length); }
                        catch (Exception)
                        {
                            return;
                            
                        }
                    }
                }
            }

            async void AcceptClients(TcpListener listener)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                clients.Add(client);
                NetworkStream stream = client.GetStream();
                ReceiveMessages(stream);
            }


            async void ReceiveMessages(NetworkStream stream)
            {
                byte[] buffer = new byte[256];
                bool isRunning = true;
                while (isRunning)
                {
                    int read = 0;
                    try { read = await stream.ReadAsync(buffer, 0, buffer.Length); }
                    catch (Exception) {
                        Console.WriteLine("Client Disconnected");
                        return;
                    }
                    string text = Encoding.UTF8.GetString(buffer, 0, read);
                    Console.WriteLine($"{clients[0]} writes: {text}");
                    byte[] buffer1 = Encoding.UTF8.GetBytes(text);

                    char[] chars = text.ToCharArray();
                    //Få til at sende kun til en (Ikke færdig)
                    /*if(chars[0] == 1)
                    {
                        clients[1].GetStream().Write(buffer1, 0, buffer1.Length);
                    } 
                    else if (chars[0] == 2)
                    {
                        clients[2].GetStream().Write(buffer1, 0, buffer1.Length);
                    }
                    */
                    foreach (TcpClient client in clients)
                    {
                            client.GetStream().Write(buffer1, 0, buffer1.Length);
                    }
                    
                }                
            }
        }
    }
}
