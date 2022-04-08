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

                    m_sqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS Catalog (id INTEGER PRIMARY KEY AUTOINCREMENT,  WHEN_DATE DATE, G_UCHASTOK TEXT, N_STN INTEGER, START_STOP INTEGER, ERASE INTEGER, BREAK INTEGER, REPLACE INTEGER, COUNTER INTEGER, SEND_STATE INTEGER)";

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
                    connection.Close();
                }
            }
            return true;
        }

        internal bool insertTestString(TelegramParser Tpars)
        {
            try
            {
                string sqlQuery = "INSERT INTO Catalog (WHEN_DATE, G_UCHASTOK, N_STN, START_STOP, ERASE, BREAK, REPLACE, COUNTER, SEND_STATE) ";
                sqlQuery += string.Format("VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8})", CurrentTime.getTime(), "'D'", Convert.ToInt32(Tpars.name), Convert.ToInt32(!Tpars.status), Convert.ToInt32(Tpars.CointerErase), Convert.ToInt32(Tpars.WireBreak), Convert.ToInt32(Tpars.DrawingChange), Convert.ToInt32(Tpars.Counter), 0);
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

                    string WHEN_DATE;
                    string G_UCHASTOK;
                    string N_STN;
                    string START_STOP;
                    string ERASE;
                    string BREAK;
                    string REPLACE;
                    string COUNTER;


                    string id_update_str = "";
                    Int64 id_telegramm;
                    Int64 count = 0;
                    Int64 temp;

                    while (sqlReader.Read()) // считываем и вносим в лист все параметры
                    {
                        WHEN_DATE = sqlReader["WHEN_DATE"].ToString(); // читаем строки с изображениями, которые хранятся в байтовом формате
                        G_UCHASTOK = sqlReader["G_UCHASTOK"].ToString();
                        N_STN = sqlReader["N_STN"].ToString();
                        START_STOP = sqlReader["START_STOP"].ToString();
                        ERASE = sqlReader["ERASE"].ToString();
                        BREAK = sqlReader["BREAK"].ToString();
                        REPLACE = sqlReader["REPLACE"].ToString();
                        COUNTER = sqlReader["COUNTER"].ToString();

                        //функция отправка телеграммы в БД telegram
                        string j = "insert into GUILD_MILL_FIX (WHEN_DATE, G_UCHASTOK, N_STAN, START_STOP, ERASE, BREAK, REPLAC, COUNTER, INCOMIN_DATE, TYPE_TELEG)";
                        string n = string.Format(" VALUES(TO_DATE('{7}', 'dd.mm.yyyy hh24:mi:ss'),'{0}',{1},{2},{3},{4},{5},{6}, SYSDATE, 'T')", G_UCHASTOK, N_STN, START_STOP, ERASE, BREAK, REPLACE, COUNTER, WHEN_DATE);
                        OracleClass.ExecuteOraCommand(j+n);

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
