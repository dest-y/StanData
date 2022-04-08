using ConsoleApp2;
using System;
using System.Collections;
using System.Threading.Tasks;

SqliteClass SqllOra = new SqliteClass();

async Task OraAsync()
{
    Console.WriteLine("Начало метода OraAsync");
    await Task.Run(() => SqllOra.getNotPostedTelegrams());
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

MThreadStan MThreadStan7 = new MThreadStan(telegram7);
MThreadStan MThreadStan8 = new MThreadStan(telegram8);
MThreadStan MThreadStan9 = new MThreadStan(telegram9);
MThreadStan MThreadStan10 = new MThreadStan(telegram10);
MThreadStan MThreadStan11 = new MThreadStan(telegram11);

Console.WriteLine("Начало данных");

while (getStan789Data())
{

}

bool getStan789Data()
{
    try
    {
        //async Task Stan10Async()
        //{
        //    await Task.Delay(0);
        //    if (telegram10.CheckUpdate())
        //    {
        //        Sqll.insertTestString(telegram10);
        //    } 
        //}
        //async Task Stan11Async()
        //{
        //    await Task.Delay(0);
        //    if (telegram11.CheckUpdate())
        //    {
        //        Sqll.insertTestString(telegram11);
        //    }
        //}
        //async Task Stan7Async()
        //{
        //    await Task.Delay(0);
        //    if (telegram7.CheckUpdate())
        //    {
        //        Sqll.insertTestString(telegram7);
        //    }
        //}
        //async Task Stan8Async()
        //{
        //    await Task.Delay(0);
        //    if (telegram8.CheckUpdate())
        //    {
        //        Sqll.insertTestString(telegram8);
        //    }
        //}
        //async Task Stan9Async()
        //{
        //    await Task.Delay(0);
        //    if (telegram9.CheckUpdate())
        //    {
        //        Sqll.insertTestString(telegram9);
        //    }
        //}
        ////if (telegram7.CheckUpdate())
        ////{
        ////    Sqll.insertTestString(telegram7);
        ////}
        ////if (telegram8.CheckUpdate())
        ////{
        ////    Sqll.insertTestString(telegram8);
        ////}
        ////if (telegram9.CheckUpdate())
        ////{
        ////    Sqll.insertTestString(telegram9);
        ////}
        ////if (telegram10.CheckUpdate())
        ////{
        ////    Sqll.insertTestString(string.Format("'{0}'", telegram10.TelegramData));
        ////}
        ////if (telegram11.CheckUpdate())
        ////{
        ////    Sqll.insertTestString(string.Format("'{0}'", telegram11.TelegramData));
        ////}
        ////Stan7Async();
        //Stan8Async();
        //Stan9Async();
        //Stan10Async();
        //Stan11Async();

        count++;

        if (count == 10)
        {
            count = 0;
            //OraAsync();
        }

        Thread.Sleep(1200);
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