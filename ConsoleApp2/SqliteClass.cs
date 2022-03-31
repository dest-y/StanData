using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace ConsoleApp2
{
    internal class SqliteClass
    {
        SqliteCommand m_sqlCmd = new SqliteCommand();

        internal SqliteClass() 
        {
            using (var connection = new SqliteConnection("Data Source=Telegrams.db"))
            {
                try
                {
                    connection.Open();

                    m_sqlCmd.Connection = connection;

                    m_sqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS Catalog (id INTEGER PRIMARY KEY AUTOINCREMENT,  telegram TEXT, send_state INTEGER)";

                    m_sqlCmd.ExecuteNonQuery();

                    m_sqlCmd.CommandText = "CREATE INDEX IF NOT EXISTS  ind_send_state ON Catalog(send_state)";

                    m_sqlCmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
        internal bool ExecutCommand(string CommandString) 
        {
            using (var connection = new SqliteConnection("Data Source=Telegrams.db"))
            {
                try
                {
                    connection.Open();

                    m_sqlCmd.Connection = connection;

                    m_sqlCmd.CommandText = CommandString;

                    m_sqlCmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            return true;
        }

        internal bool insertTestString(string TelegramDataString)
        {
            { 
                string sqlQuery = "INSERT INTO Catalog (telegram, send_state) ";
                sqlQuery += string.Format("VALUES ({0},{1})", TelegramDataString, 0);
                ExecutCommand(sqlQuery);
                return true;
            }
            return false;
        }

    }
}
