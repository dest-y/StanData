using ConsoleApp2;


bool OraNowUpdateStatus = false;
OracleConnection Oratest = new OracleConnection();
SqliteDataTransfer SqllOra = new SqliteDataTransfer(Oratest);

async Task OraUpdateAsync()
{
    OraNowUpdateStatus = true;
    Thread.Sleep(1000);
    Console.WriteLine("Начало метода OraAsync");
    await Task.Run(() => SqllOra.TransferNotPostedTelegrams());
    Console.WriteLine("Конец метода OraAsync");
    OraNowUpdateStatus = false;
}
int count = 0;


Stan789 stan7 = new Stan789("7", "140.80.1.1");
Stan789 stan8 = new Stan789("8", "140.80.1.2");
Stan789 stan9 = new Stan789("9", "140.80.1.4");
Stan1011 stan10 = new Stan1011("10", "140.80.0.34");
Stan1011 stan11 = new Stan1011("11", "140.80.0.36");

TelegramParser StanParser7 = new TelegramParser(stan7);
TelegramParser StanParser78 = new TelegramParser(stan8);
TelegramParser StanParser79 = new TelegramParser(stan9);
TelegramParser StanParser710 = new TelegramParser(stan10);
TelegramParser StanParser711 = new TelegramParser(stan11);

MThreadStan MThreadStan7 = new MThreadStan(StanParser7, 1000);
MThreadStan MThreadStan8 = new MThreadStan(StanParser78, 1000);
MThreadStan MThreadStan9 = new MThreadStan(StanParser79, 1000);
MThreadStan MThreadStan10 = new MThreadStan(StanParser710, 1000);
MThreadStan MThreadStan11 = new MThreadStan(StanParser711, 1000);

//StanParser710.OraUpdate += StanParser_OraUpdate;
//StanParser711.OraUpdate += StanParser_OraUpdate;

MThreadOrace ThreadOrace = new MThreadOrace(SqllOra);

void StanParser_OraUpdate()
{
    ThreadOrace.OracleEvent.Set();
}

Loop:
using (SqllOra)
{
    Console.WriteLine("реконект к БД!");
    while (true)
    {
        Thread.Sleep(5000);
    }
    Thread.Sleep(5000);
    goto Loop;
}

//bool PullDataToOra()
//{

//    try
//    {
//        count++;

//        if (count == 5)
//        {
//            count = 0;
//            OraUpdateAsync();
//            OraNowUpdateStatus = true;
//        }

//        Thread.Sleep(3000);
//        OraNowUpdateStatus = false;
//        return true;
//    }
//    catch
//    {
//        OraNowUpdateStatus = false;
//        return false;
//    }
//}

Console.WriteLine("Для завершения программы нажмите <Enter>");
Console.ReadLine();