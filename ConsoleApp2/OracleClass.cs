using Oracle.ManagedDataAccess.Client;

namespace ConsoleApp2
{
    public class OracleClass
    {

        OracleConnection con;

        public OracleClass() 
        {
            UserSecrets secret = new UserSecrets();
            secret.SecretsInit();
            string conString = string.Format("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT=1521))(CONNECT_DATA=(SERVICE_NAME={1})));User ID={2};Password={3};", secret.HOST, secret.SERVICE_NAME, secret.UserID, secret.Password);
            con = new OracleConnection(conString);
        }

        public bool ExecuteOraCommand2(string CommandString) 
        {
            try
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.BindByName = true;

                    cmd.CommandText = CommandString;

                    OracleDataReader reader = cmd.ExecuteReader();

                    con.Close();
                    reader.Dispose();
                    return true;
                }
            }
            catch 
            {
                con.Dispose();
                return false;
            }
        }

        public static bool ExecuteOraCommand(string CommandString)
        {
            UserSecrets secret = new UserSecrets();
            secret.SecretsInit();

            string conString = string.Format("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT=1521))(CONNECT_DATA=(SERVICE_NAME={1})));User ID={2};Password={3};", secret.HOST, secret.SERVICE_NAME, secret.UserID, secret.Password);
            using (OracleConnection con = new OracleConnection(conString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.BindByName = true;

                    cmd.CommandText = CommandString;//"Select JOB_ID from JOBS"

                    OracleDataReader reader = cmd.ExecuteReader();

                    //while (reader.Read())
                    //{
                    //    Console.WriteLine("Имя пользователя:" + reader.GetString(0));
                    //}
                    con.Close();
                    reader.Dispose();
                    return true;
                }
            }
        }
    }




}
