using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    public class Client
    {
        public string _name { get; set; }
        public int _id {get;set; }
        public TcpClient _client { get; }
        public Client(string name, int id, TcpClient client)
        {
            _name = name;
            _id = id;
            _client = client;
        }

    }
}
