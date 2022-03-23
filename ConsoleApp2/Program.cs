using ConsoleApp2;
using Sharp7;

Console.WriteLine("Begin");

Stan789 stan7 = new Stan789("7 стан", "140.80.1.1");
Stan789 stan8 = new Stan789("8 стан", "140.80.1.32");
Stan789 stan9 = new Stan789("9 стан", "140.80.1.4");

Console.WriteLine("Начало данных");

while (getStan789Data())
{
    Console.WriteLine();
}

bool getStan789Data()
{
    try
    {
        stan7.getData();
        stan8.getData();
        stan9.getData();

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