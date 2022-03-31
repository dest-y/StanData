using ConsoleApp2;
using Sharp7;
using System.Text;
using Oracle.ManagedDataAccess.Client;

SqliteClass ssll = new SqliteClass();


Stan789 stan7 = new Stan789("7", "140.80.1.1");
Stan789 stan8 = new Stan789("8", "140.80.1.2");
Stan789 stan9 = new Stan789("9", "140.80.1.4");

Stan1011 stan10 = new Stan1011("10", "140.80.0.34");
Stan1011 stan11 = new Stan1011("11", "140.80.0.36");

TelegramParser telegram7 =  new TelegramParser(stan7);
TelegramParser telegram8 = new TelegramParser(stan8);
TelegramParser telegram9 = new TelegramParser(stan9);
TelegramParser telegram10 = new TelegramParser(stan10);
TelegramParser telegram11 = new TelegramParser(stan11);


//OracleClass.ExecuteOraCommand("Select JOB_ID from JOBS");

SqliteClass Sqll = new SqliteClass();

//Sqll.insertTestString();

Console.WriteLine("Начало данных");

while (getStan789Data())
{

}


bool getStan789Data()
{
    try
    {
        //stan7.getData();
        //stan8.getData();
        //stan9.getData();
        //stan10.getData();
        //stan11.getData();

        //telegram7.CheckUpdate();
        //telegram8.CheckUpdate();
        //telegram9.CheckUpdate();
        //telegram10.CheckUpdate();
        //telegram11.CheckUpdate();
        if (telegram7.CheckUpdate()) 
        {
            Sqll.insertTestString(string.Format("'{0}'", telegram7.TelegramData));
        }
        if (telegram8.CheckUpdate())
        {
            Sqll.insertTestString(string.Format("'{0}'", telegram8.TelegramData));
        }
        if (telegram9.CheckUpdate())
        {
            Sqll.insertTestString(string.Format("'{0}'", telegram9.TelegramData));
        }
        if (telegram10.CheckUpdate())
        {
            Sqll.insertTestString(string.Format("'{0}'", telegram10.TelegramData));
        }
        if (telegram11.CheckUpdate())
        {
            Sqll.insertTestString(string.Format("'{0}'", telegram11.TelegramData));
        }



        Thread.Sleep(1000);
        return true;
    }
    catch
    {
        stan7.ClientDisc();
        stan8.ClientDisc();
        stan9.ClientDisc();
        Console.WriteLine("getStan789Data = FALSE!");
        return false;
    }
}


Console.WriteLine("Для завершения программы нажмите <Enter>");
Console.ReadLine();