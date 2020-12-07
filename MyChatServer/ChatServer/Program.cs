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
                clients.Add(new Client("user", count, tcpListener.AcceptTcpClient()));
                count++;
                Console.WriteLine("Новый пользователь");
                Task.Factory.StartNew(() => { Listner(clients[clients.Count - 1]); });
            }
        }

        public static string GetOnlines()
        {
            string text = "<Onlines>|";
            clients.ForEach(el => text += (el._name + "|"));
            return text += "<end>";  
        }
        public static void SendToAll(string smt)=> clients.ForEach(el => el._client.GetStream().Write(Encoding.UTF8.GetBytes(smt), 0, Encoding.UTF8.GetBytes(smt).Length));
     

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
                    string something = Encoding.UTF8.GetString(buffer);

                    if (something.Contains("<MyName>"))
                    {
                        client._name = something.Remove(0, 8);
                        client._name = client._name.Remove(client._name.IndexOf('\0'));
                        SendToAll(GetOnlines());
                    }
                    else
                    {
                        string message = client._name + ":" + something;
                        Console.WriteLine((Encoding.UTF8.GetString(buffer)));
                        if (buffer.Length > 0)
                        {
                            SendToAll(message);
                            //  Console.WriteLine(message);
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("Кто то нас покинул");
                    clients.Remove(client);
                    SendToAll(GetOnlines());
                    isOnline = false;
                }
            }
       
        }
    }
}
