using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
        public void SendToServer(string smt)=>  client.GetStream().Write(Encoding.UTF8.GetBytes(smt), 0, Encoding.UTF8.GetBytes(smt).Length);
        
        public MainWindowViewModel()
        {

            
            Messages = new List<MessageChat>();

            IPAddress ip = IPAddress.Parse("127.0.0.1");
            int port = 8888;
           
            try
            {
                client = new TcpClient();
                client.Connect(ip, port);
                SendMessageCommand = new Command(SendMessage);
                Random random = new Random();
                SendToServer("<MyName>user" + random.Next(0, 1000).ToString());
                Task.Factory.StartNew(() => { Listner(); });
            }
            catch
            {
                Unconnected();
            }
           
        }
        public  void Unconnected()
        {
            SendMessageCommand = null;
              MessageBox.Show("you are not connected");
        }

        void Listner()
        {
            try
            {
                while (true)
                {
                    NetworkStream stream = client.GetStream();
                    byte[] buffer = new byte[255];


                    stream.Read(buffer, 0, 255);

                    string temp = Encoding.UTF8.GetString(buffer);
                    temp = temp.Remove(temp.IndexOf('\0') + 1);
                    if (temp.Contains("<Onlines>|"))
                    {
                        temp = temp.Remove(0, 10);
                        temp = temp.Remove(temp.IndexOf("|<end>"));
                        Online = temp.Split('|').ToList();
                    }
                    else
                    if (temp.IndexOf(":") > 0)
                    {
                        temp = temp.Remove(temp.IndexOf('\0'));
                        Messages.Add(new MessageChat(temp.Remove(temp.IndexOf(":") + 1), temp.Remove(0, temp.IndexOf(":") + 1), DateTime.Now));
                        Messages = new List<MessageChat>(Messages);
                    }
                }
            }
            catch
            {
                Unconnected();
            }
        }
        void SendMessage(object parametr)
        {
            try
            {
                if (Message.Length >= 255)
                {
                    MessageBox.Show("Message too long");
                }
                else if(Message == "")
                {
                    MessageBox.Show("Message is empty");
                }
                else
                {
                    SendToServer(Message + "\0\0");
                    Message = "";
                }
            }
            catch
            {
                Unconnected();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
