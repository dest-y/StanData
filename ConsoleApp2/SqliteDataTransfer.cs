using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace ConsoleApp2
{

    internal class SqliteDataTransfer : SqliteClass
    {
        SqliteCommand m_sqlCmdOra = new SqliteCommand();

        public SqliteDataTransfer() 
        {
            connection = new SqliteConnection("Data Source=Telegrams.db");
            connection.Open();
        }

        internal void TransferNotPostedTelegrams(OracleConnection OraConnection)
        {
            try
            {
                m_sqlCmdOra.Connection = connection;

                m_sqlCmdOra.CommandText = "SELECT Catalog.*, count(*) over() conter FROM [Catalog] WHERE [send_state] = 0";
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

                OraConnection.StartTransaction();
                StartTransaction();                    //Local DB
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
                    try
                    {
                        OraConnection.ExecuteCommand(j + n);

                        //OracleClass.ExecuteOraCommand(j + n);

                        id_telegramm = (Int64)sqlReader["id"];

                        temp = (Int64)sqlReader["conter"];
                        count++;

                        if (count == 1)
                        {
                            id_update_str += id_telegramm;
                        }
                        else
                        {
                            id_update_str += ",";
                            id_update_str += id_telegramm;

                        }
                    }
                    catch
                    {
                        Console.WriteLine("Ошибка записи в БД ORACLE");
                        OraConnection.RollbackTransaction();
                        RollbackTransaction();                    //Local DB
                    }
                }
                sqlReader.Close();

                OraConnection.CommitTransaction();
                CommitTransaction();                    //Local DB

                string sqlQuery;
                sqlQuery = "update Catalog set send_state=1 where id in";
                sqlQuery += string.Format("({0})", id_update_str);
                m_sqlCmdOra.CommandText = sqlQuery;
                m_sqlCmdOra.ExecuteNonQuery();
                sqlReader.Close();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
