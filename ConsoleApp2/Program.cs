using ConsoleApp2;
using Sharp7;

Console.WriteLine("Begin");

Stan789 stan7 = new Stan789("7", "140.80.1.1");
Stan789 stan8 = new Stan789("8", "140.80.1.2");
Stan789 stan9 = new Stan789("9", "140.80.1.4");

Stan1011 stan10 = new Stan1011("10", "140.80.0.34");
Stan1011 stan11 = new Stan1011("11", "140.80.0.36");

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
        stan7.getData();
        stan8.getData();
        stan9.getData();
        stan10.getData();
        stan11.getData();

        //telegram7.CheckUpdate();
        //telegram8.CheckUpdate();
        //telegram9.CheckUpdate();

        CurrentTime.getTime();
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