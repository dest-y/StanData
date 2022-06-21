using Microsoft.Data.Sqlite;

namespace ConsoleApp2
{

    internal class SqliteDataTransfer : SqliteClass
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        OracleConnection OraCon;
        public SqliteDataTransfer(OracleConnection OraConnection)
        {
            connection = new SqliteConnection("Data Source=Telegrams.db");
            connection.Open();
            OraCon = OraConnection;
        }

        internal Int64 TransferNotPostedTelegrams()
        {
            if (OraCon.con.State == System.Data.ConnectionState.Open)
                using (SqliteCommand m_sqlCmd = connection.CreateCommand())
                {
                    try
                    {
                        m_sqlCmd.Connection = connection;
                        m_sqlCmd.CommandText = "SELECT Catalog.*, count(*) over() conter FROM [Catalog] WHERE [send_state] = 0 LIMIT 5";
                        m_sqlCmd.ExecuteNonQuery();
                        SqliteDataReader sqlReader = m_sqlCmd.ExecuteReader();

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
                        Int64 temp = 0;



                        while (sqlReader.Read() && OraCon.con.State == System.Data.ConnectionState.Open) // считываем и вносим в лист все параметры
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

                                if (count == 0)
                                    OraCon.StartTransaction();
                                
                                OraCon.ExecuteCommand(j + n);

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
                        sqlReader.Close();

                        if (count > 0)
                            OraCon.CommitTransaction(); // сохраним транзакцию в оракл со всеми записями.

                        

                        string sqlQuery;
                        sqlQuery = "update Catalog set send_state=1 where id in";
                        sqlQuery += string.Format("({0})", id_update_str);
                        m_sqlCmd.CommandText = sqlQuery;
                        m_sqlCmd.ExecuteNonQuery();

                        if (temp == 0)
                            return 0;   // нет записей для обработки

                        if (temp == count && count > 0)
                            return 0;  // все записи обработаны. НЕОБРАБОТАННЫХ НЕТ

                        if (temp > 0 && temp > count) // если число необработанных строк  ЕСТЬ и обработано 10, но НЕОбработанных БОЛЬШЕ 10
                            return temp - count; // число необработанных строк.

                    }
                    catch (Exception ex)
                    {
                        Logger.Debug(ex);
                        Logger.Debug("NEWLINE");

                        Console.WriteLine("Ошибка записи в удаленную БД ORACLE");
                        OraCon.RollbackTransaction();
                        return 0;
                    }
                }
            else
                OraCon.Restart();
            return 0;
        }
    }
}
