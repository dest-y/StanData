using ConsoleApp2.Interfaces;
using Oracle.ManagedDataAccess.Client;

namespace ConsoleApp2
{
    internal class OracleConnection : IConnectionHandler
    {
        public Oracle.ManagedDataAccess.Client.OracleConnection con;
        internal OracleTransaction transaction;

        public OracleConnection()
        {
            TryConnect();
        }

        public bool ExecuteCommand(string CommandString)
        {
            using (OracleCommand cmd = con.CreateCommand())
            {

                cmd.BindByName = true;
                cmd.CommandText = CommandString;

                OracleDataReader reader = cmd.ExecuteReader();

                reader.Dispose();
                Console.WriteLine("OraOK");
                return true;
            }

        }
        public void Close()
        {
            con.Close();
            Console.WriteLine(con.State);
        }
        public void Create()
        {
            UserSecrets secret = new UserSecrets();
            secret.SecretsInit();
            string conString = string.Format("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT=1521))(CONNECT_DATA=(SERVICE_NAME={1})));User ID={2};Password={3};", secret.HOST, secret.SERVICE_NAME, secret.UserID, secret.Password);
            con = new Oracle.ManagedDataAccess.Client.OracleConnection(conString);
            con.Open();
        }
        public void Restart()
        {
            try
            {
                this.Close();
                this.Create();
                Console.WriteLine("Попытка переподлючения к Oracle");
                Console.WriteLine(this.con.State);
            }
            catch
            {
                Console.WriteLine("Невозможно создать подлючение к Oracle");
            }
        }
        public void StartTransaction()
        {
            if (con.State == System.Data.ConnectionState.Open)
                transaction = this.con.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
        }
        public void CommitTransaction()
        {
            if (transaction != null)
                transaction.Commit();
            Console.WriteLine("ORA COMMIT");
        }
        public void RollbackTransaction()
        {
            Console.WriteLine("ORA ROLLBACK");
            if (transaction != null && con.State == System.Data.ConnectionState.Open)
                transaction.Rollback();
        }
        public void TryConnect()
        {
            try
            {
                UserSecrets secret = new UserSecrets();
                secret.SecretsInit();
                string conString = string.Format("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT=1521))(CONNECT_DATA=(SERVICE_NAME={1})));User ID={2};Password={3};", secret.HOST, secret.SERVICE_NAME, secret.UserID, secret.Password);
                con = new Oracle.ManagedDataAccess.Client.OracleConnection(conString);
                con.Open();
            }
            catch
            {
                Console.WriteLine("Cant Connect to ORACLE DB");
            }
        }

    }
}
