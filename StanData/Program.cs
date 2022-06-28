﻿using ConsoleApp2;
using Newtonsoft.Json;
using System.Drawing;
using System.Runtime.InteropServices;
using DWORD = System.UInt64;

string mutex_id = "StanData(7-11)";
using (Mutex mutex = new Mutex(false, mutex_id))
{

    bool OraNowUpdateStatus = false;
    OracleConnection Oratest = new OracleConnection();
    SqliteDataTransfer SqllOra = new SqliteDataTransfer(Oratest);

    //var icon = new NotifyIcon();
    //icon.Icon = new Icon("icon.ico");
    //icon.Visible = true;

    const int MF_BYCOMMAND = 0x00000000;
    const int SC_CLOSE = 0xF060;

    [DllImport("user32.dll")]
    static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

    [DllImport("user32.dll")]
    static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

    [DllImport("kernel32.dll", ExactSpelling = true)]
    static extern IntPtr GetConsoleWindow();

    DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_CLOSE, MF_BYCOMMAND);

    Console.Title = "StanData";

    [DllImport("user32.dll", SetLastError = true)]
    static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll")]
    static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("kernel32.dll")]
    static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

    [DllImport("kernel32.dll")]
    static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);



    #if RELEASE
    if (!mutex.WaitOne(0, false))
    {
        Console.WriteLine("Копия приложения уже запущена!");
        Console.ReadLine();
        return;
    }
    #endif

    async Task OraUpdateAsync()
    {
        OraNowUpdateStatus = true;
        Thread.Sleep(1000);
        Console.WriteLine("Начало метода OraAsync");
        await Task.Run(() => SqllOra.TransferNotPostedTelegrams());
        Console.WriteLine("Конец метода OraAsync");
        OraNowUpdateStatus = false;
    }


    string path = "StanConfig.json";
    string text;
    // асинхронное чтение
    using (StreamReader reader = new StreamReader(path))
    {
        text = await reader.ReadToEndAsync();
        Console.WriteLine(text);
    }


    Dictionary<string, string> StansConfig = JsonConvert.DeserializeObject<Dictionary<string, string>>(text);

    var Stanss = new List<Stan>();
    var StanssParser = new List<TelegramParser>();
    var MThreadStan = new List<MThreadStan>();

    foreach (var (key, value) in StansConfig)
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

    

    IntPtr hwnd = FindWindow("StanData", null); //запрещаем выделение мышью
    SetConsoleMode(hwnd, 0x0080);


#if DEBUG
    Console.WriteLine("Сейчас в DEBUG");
    while (true)
    {
        //ShowWindow(GetConsoleWindow(), 1);
        Thread.Sleep(5000);
        //ShowWindow(GetConsoleWindow(), 0);
        Thread.Sleep(5000);
    }
#endif

#if RELEASE
    Console.WriteLine("Сейчас в RELEASE");
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
#endif



//    MThreadOrace ThreadOrace = new MThreadOrace(SqllOra);

//    void StanParser_OraUpdate()
//    {
//        ThreadOrace.OracleEvent.Set();
//    }

//Loop:
//    using (SqllOra)
//    {
//        Console.WriteLine("реконект к БД!");
//        while (true)
//        {
//            Thread.Sleep(5000);
//        }
//        Thread.Sleep(5000);
//        goto Loop;
//    }

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
}