using ConsoleApp2;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;


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


string path = "json2.json";
string text;
// асинхронное чтение
using (StreamReader reader = new StreamReader(path))
{
    text = await reader.ReadToEndAsync();
    Console.WriteLine(text);
}


Dictionary<string, string> htmlAttributes = JsonConvert.DeserializeObject<Dictionary<string, string>>(text);

var Stanss = new List<Stan>();
var StanssParser = new List<TelegramParser>();
var MThreadStan = new List<MThreadStan>();

foreach (var (key, value) in htmlAttributes)
{
    Stan stan;
    TelegramParser stanParser;

    if (Convert.ToInt32(key) >= 10)
    {
        stan = new Stan1011(key, value);
        stanParser = new TelegramParser(stan);
    }
    else 
    {
        stan = new Stan789(key, value);
        stanParser = new TelegramParser(stan);
    }

    Stanss.Add(stan);
    StanssParser.Add(stanParser);
    MThreadStan.Add(new MThreadStan(stanParser, 1000));
}

//Stan789 stan7 = new Stan789("7", "140.80.1.1");
//TelegramParser StanParser7 = new TelegramParser(stan7);               Ручное создание станов.
//MThreadStan MThreadStan7 = new MThreadStan(StanParser7, 1000);


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