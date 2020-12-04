using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp13
{
   public class MessageChat
    {
        public MessageChat(string name, string info, DateTime date)
        {
            Name = name;
            Info = info;
            Date = date;
        }

        public string Name { get; set; }
        public string Info { get; set; }
        public DateTime Date { get; set; }

    }
}
