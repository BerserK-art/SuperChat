using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp13;

namespace ChatServer
{
    public class DatabaseConnect : IDisposable
    {
        string _connectionString;
        MySqlConnection _mySqLConnection;

        public DatabaseConnect(string Server, string User, string Password, string DataBase)
        {
            _connectionString = $"Server = {Server}; Database = {DataBase}; Uid = {User}; Pwd = {Password}";
            _mySqLConnection = new MySqlConnection(_connectionString);
            _mySqLConnection.Open();
        }
        public void InsertHistory(string Name, string Message)
        {
            string query = $"INSERT INTO History (Name, Info) VALUES ('{Name}','{Message}');";
            MySqlCommand command = new MySqlCommand(query, _mySqLConnection);
            command.ExecuteNonQuery();
        }
        public List<MessageChat> SelectHistory(int count)
        {
            List<MessageChat> messages = new List<MessageChat>();
            string query = $"SELECT TOP {count.ToString()} * FROM History ORDER BY id DESC";
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(query,_mySqLConnection);
            DataTable table = new DataTable();
            dataAdapter.Fill(table);
            return messages;
        }

        public void Dispose()
        {
            _mySqLConnection.Close();
        }
    }
}
