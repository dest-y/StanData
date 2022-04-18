using ConsoleApp2.Interfaces;
using Microsoft.Data.Sqlite;

namespace ConsoleApp2
{
    internal class SqliteClass : IDisposable, IConnectionHandler
    {
        SqliteCommand m_sqlCmd = new SqliteCommand();
        internal SqliteConnection connection;
        SqliteTransaction transaction;
        public SqliteClass()
        {
            connection = new SqliteConnection("Data Source=Telegrams.db");
            connection.Open();

            try
            {
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
        internal bool insertTestString(TelegramParser Tpars)
        {
            try
            {
                string sqlQuery = "INSERT INTO Catalog (WHEN_DATE, G_UCHASTOK, N_STN, START_STOP, ERASE, BREAK, REPLACE, COUNTER, SEND_STATE) ";
                sqlQuery += string.Format("VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8})", CurrentTime.getTime(), "'D'", Convert.ToInt32(Tpars.name), Convert.ToInt32(!Tpars.status), Convert.ToInt32(Tpars.CointerErase), Convert.ToInt32(Tpars.WireBreak), Convert.ToInt32(Tpars.DrawingChange), Convert.ToInt32(Tpars.Counter), 0);
                ExecuteCommand(sqlQuery);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public void Create()
        {
            connection = new SqliteConnection("Data Source=Telegrams.db");
            connection.Open();

            try
            {
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
        public void Close()
        {
            connection.Close();
        }
        public void Restart()
        {
            Close();
            Create();
        }
        public bool ExecuteCommand(string CommandString)
        {
            try
            {
                connection.Open();
                m_sqlCmd.Connection = connection;
                m_sqlCmd.CommandText = CommandString;
                m_sqlCmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
        public void StartTransaction()
        {
            transaction = connection.BeginTransaction();
        }

        public void CommitTransaction()
        {
            transaction.Commit();
        }

        public void RollbackTransaction()
        {
            transaction.Rollback();
        }
    }
}
