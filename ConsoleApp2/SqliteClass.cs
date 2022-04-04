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
        SqliteCommand m_sqlCmdOra = new SqliteCommand();

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
            try
            {
                string sqlQuery = "INSERT INTO Catalog (telegram, send_state) ";
                sqlQuery += string.Format("VALUES ({0},{1})", TelegramDataString, 0);
                ExecutCommand(sqlQuery);
                return true;
            }
            catch 
            {
                return false;
            }
            
        }

        internal void getNotPostedTelegrams()
        {
            using (var connection = new SqliteConnection("Data Source=Telegrams.db"))
            {
                try
                {
                    connection.Open();

                    m_sqlCmdOra.Connection = connection;

                    m_sqlCmdOra.CommandText = "SELECT Catalog.*, count(*) over() conter FROM [Catalog] WHERE [send_state] =0";

                    m_sqlCmdOra.ExecuteNonQuery();

                    SqliteDataReader sqlReader = m_sqlCmdOra.ExecuteReader();

                    string telegram;
                    string id_update_str = "";
                    Int64 id_telegramm;
                    Int64 count = 0;
                    Int64 temp;

                    while (sqlReader.Read()) // считываем и вносим в лист все параметры
                    {
                        telegram = sqlReader["telegram"].ToString(); // читаем строки с изображениями, которые хранятся в байтовом формате

                        //функция отправка телеграммы в БД telegram
                        OracleClass.ExecuteOraCommand("insert into test_sequence values(t_sequence.nextval,'" + telegram + "')");

                        id_telegramm = (Int64)sqlReader["id"];

                        temp = (Int64)sqlReader["conter"];
                        count++;
                        id_update_str += id_telegramm;

                        if (count < (Int64)sqlReader["conter"])
                            id_update_str += ",";


                    }
                    sqlReader.Close();

                    string sqlQuery;
                    sqlQuery = "update Catalog set send_state=1 where id in";
                    sqlQuery += string.Format("({0})", id_update_str);
                    m_sqlCmdOra.CommandText = sqlQuery;
                    m_sqlCmdOra.ExecuteNonQuery();
                    sqlReader.Close();

                    connection.Close();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

        }

    }
}
