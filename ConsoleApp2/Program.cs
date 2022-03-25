using ConsoleApp2;
using Sharp7;

Console.WriteLine("Begin");

Stan789 stan7 = new Stan789("7", "140.80.1.1");
Stan789 stan8 = new Stan789("8", "140.80.1.2");
Stan789 stan9 = new Stan789("9", "140.80.1.4");

TelegramParser telegram7 =  new TelegramParser(stan7);
TelegramParser telegram8 = new TelegramParser(stan8);
TelegramParser telegram9 = new TelegramParser(stan9);

Console.WriteLine("Начало данных");

while (getStan789Data())
{
    
}

bool getStan789Data()
{
    try
    {
        //stan7.getData();
        telegram7.CheckUpdate();
        telegram8.CheckUpdate();
        telegram9.CheckUpdate();
        //stan8.getData();
        //stan9.getData();
        //CurrentTime.getTime();
        Thread.Sleep(5000);
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