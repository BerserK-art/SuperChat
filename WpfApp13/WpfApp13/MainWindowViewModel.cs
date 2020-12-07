using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace WpfApp13
{
   public class MainWindowViewModel : INotifyPropertyChanged
    {
        TcpClient client;
        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Message"));
            }
        }

        private  List<MessageChat> _messages;
        public  List<MessageChat> Messages
        {
            get { return _messages; }
            set {
                _messages = value;
                  PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Messages"));
            }

        }

        private List<string> _online;
        public List<string> Online
        {
            get { return _online; }
            set
            {
                _online = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Online"));
            }

        }
        public Command SendMessageCommand { get; set; }
        public void SendToServer(string smt)
        => client.GetStream().Write(Encoding.UTF8.GetBytes(smt), 0, Encoding.UTF8.GetBytes(smt).Length);
        
        public MainWindowViewModel()
        {

            SendMessageCommand = new Command(SendMessage);
            Messages = new List<MessageChat>();

            IPAddress ip = IPAddress.Parse("127.0.0.1");
            int port = 8888;

            client = new TcpClient();
            client.Connect(ip, port);

            Random random = new Random();
            SendToServer("<MyName>user" + random.Next(0, 1000).ToString());
            

            Task.Factory.StartNew(() => { Listner(); });
        }


        void Listner()
        {
            while (true)
            {
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[255];
                stream.Read(buffer, 0, 255);
                
                string temp = Encoding.UTF8.GetString(buffer);
                temp = temp.Remove(temp.IndexOf('\0') + 1);
                if(temp.Contains("<Onlines>|"))
                {
                    temp= temp.Remove(0,10);
                   temp = temp.Remove(temp.IndexOf("|<end>"));
                    Online = temp.Split('|').ToList();
                }
                else
                if (temp.IndexOf(":")>0)
                {
                    temp =temp.Remove(temp.IndexOf('\0'));
                    Messages.Add (new MessageChat(temp.Remove(temp.IndexOf(":") + 1),  temp.Remove(0, temp.IndexOf(":") + 1), DateTime.Now));
                    Messages = new List<MessageChat>(Messages);
                }
            }
        }
        void SendMessage(object parametr)
        {
            SendToServer(Message +"\0\0");
            Message = "";
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
