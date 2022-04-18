using ConsoleApp2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace ConsoleApp2
{
    internal class OracleConnection : IConnectionHandler
    {
        public Oracle.ManagedDataAccess.Client.OracleConnection con;
        OracleTransaction transaction;

        public OracleConnection()
        {
            UserSecrets secret = new UserSecrets();
            secret.SecretsInit();
            string conString = string.Format("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT=1521))(CONNECT_DATA=(SERVICE_NAME={1})));User ID={2};Password={3};", secret.HOST, secret.SERVICE_NAME, secret.UserID, secret.Password);
            con = new Oracle.ManagedDataAccess.Client.OracleConnection(conString);
            con.Open();
        }

        public bool ExecuteCommand(string CommandString)
        {
            try
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
            catch
            {
                Console.WriteLine("OraFail");
                return false;
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
        }
        public void Restart()
        {
            this.Close();
            this.Create();
        }
        public void StartTransaction() 
        {
            Console.WriteLine(con);
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
            if (transaction != null)
            transaction.Rollback();
            Console.WriteLine("ORA ROLLBACK");
        }
        
    }
}
