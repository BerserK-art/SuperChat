using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace ChatServer
{
    class Program
    {
       static  List<Client> clients;
       static int count = 0;
        static void Main(string[] args)
        {
            clients = new List<Client>();
            TcpListener tcpListener = new TcpListener(IPAddress.Any, 8888);
            tcpListener.Start();
           
            while (true)
            {
                clients.Add(new Client("user",count,tcpListener.AcceptTcpClient()));
                count++;
                Console.WriteLine("Новый пользователь");
                Task.Factory.StartNew(() => { Listner(clients[clients.Count-1]); });
            }
        }

       static  void Listner (Client client)
        {
            bool isOnline = true;
            while(isOnline)
            {
               
                    NetworkStream stream = client._client.GetStream();
                    byte[] buffer = new byte[255];
                    try
                    {
                        stream.Read(buffer, 0, 255);
                    string message = client._name + ":" + (Encoding.UTF8.GetString(buffer));
                        Console.WriteLine((Encoding.UTF8.GetString(buffer)));
                        if (buffer.Length > 0)
                        {
                        buffer = Encoding.UTF8.GetBytes(message);
                        Console.WriteLine(message);
                            foreach (var SendTo in clients)
                            {
                                NetworkStream networkStream = SendTo._client.GetStream();
                                networkStream.Write(buffer, 0, buffer.Length);
                            }
                        }
                    } catch
                    {
                        Console.WriteLine("Кто то нас покинул");
                        clients.Remove(client);
                        isOnline = false;
                    }
            }
       
        }
    }
}
