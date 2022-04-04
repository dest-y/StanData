using ConsoleApp2;


SqliteClass Sqll = new SqliteClass();


async Task OraAsync()
{
    Console.WriteLine("Начало метода OraAsync");
    await Task.Run(() => Sqll.getNotPostedTelegrams());
    Console.WriteLine("Конец метода OraAsync");
}
int count = 0;

Stan789 stan7 = new Stan789("7", "140.80.1.1");
Stan789 stan8 = new Stan789("8", "140.80.1.2");
Stan789 stan9 = new Stan789("9", "140.80.1.4");

Stan1011 stan10 = new Stan1011("10", "140.80.0.34");
Stan1011 stan11 = new Stan1011("11", "140.80.0.36");

TelegramParser telegram7 = new TelegramParser(stan7);
TelegramParser telegram8 = new TelegramParser(stan8);
TelegramParser telegram9 = new TelegramParser(stan9);
TelegramParser telegram10 = new TelegramParser(stan10);
TelegramParser telegram11 = new TelegramParser(stan11);


Console.WriteLine("Начало данных");

while (getStan789Data())
{

}


bool getStan789Data()
{
    try
    {
        async Task Stan10Async()
        {
            await Task.Delay(500);
            if (telegram10.CheckUpdate())
            {
                Sqll.insertTestString(string.Format("'{0}'", telegram10.TelegramData));
            }
        }
        async Task Stan11Async()
        {
            await Task.Delay(500);
            if (telegram11.CheckUpdate())
            {
                Sqll.insertTestString(string.Format("'{0}'", telegram11.TelegramData));
            }
        }

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
        //if (telegram10.CheckUpdate())
        //{
        //    Sqll.insertTestString(string.Format("'{0}'", telegram10.TelegramData));
        //}
        //if (telegram11.CheckUpdate())
        //{
        //    Sqll.insertTestString(string.Format("'{0}'", telegram11.TelegramData));
        //}
        Stan10Async();
        Stan11Async();
        count++;

        if (count == 10)
        {
            count = 0;
            OraAsync();
        }

        Thread.Sleep(1000);
        return true;
    }
    catch
    {
        stan7.ClientDisc();
        stan8.ClientDisc();
        stan9.ClientDisc();
        stan10.ClientDisc();
        stan11.ClientDisc();
        Console.WriteLine("getStan789Data = FALSE!");
        return false;
    }
}


Console.WriteLine("Для завершения программы нажмите <Enter>");
Console.ReadLine();