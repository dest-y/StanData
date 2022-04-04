using System;
using Oracle.ManagedDataAccess.Client;

namespace ConsoleApp2
{
    public class OracleClass
    {

        

        public static bool ExecuteOraCommand(string CommandString) 
        {
            UserSecrets secret = new UserSecrets();
            secret.SecretsInit();

            Console.WriteLine(secret.UserID);
            Console.WriteLine(secret.Password);
            Console.WriteLine(secret.HOST);
            Console.WriteLine(secret.SERVICE_NAME);


            string conString = string.Format("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT=1521))(CONNECT_DATA=(SERVICE_NAME={1})));User ID={2};Password={3};", secret.HOST, secret.SERVICE_NAME, secret.UserID, secret.Password);
            using (OracleConnection con = new OracleConnection(conString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
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
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine("Oracle false");
                        return false;
                    }
                    Console.WriteLine("Oracle true");
                    return true;
                }
            }
        }
    }

        
  
    
}
