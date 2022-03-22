using ConsoleApp2;
using Sharp7;

Console.WriteLine("Begin");

Stan789 stan7 = new Stan789("7 стан", "140.80.1.1");
Stan789 stan8 = new Stan789("8 стан", "140.80.1.2");
Stan789 stan9 = new Stan789("9 стан", "140.80.1.4");



stan7.getData();
stan8.getData();
stan9.getData();

var client = new S7Client();
client.SetConnectionType(S7Client.CONNTYPE_BASIC);
//int connectionResult = client.ConnectTo("140.80.1.1", 0, 3); //7-9 стан 
int connectionResult = client.ConnectTo("140.80.0.34", 0, 2); //10-11 стан 
if (connectionResult == 0)
{
    Console.WriteLine("Connection OK");
    Console.WriteLine(connectionResult);
    S7Client.S7Protection levelProtect = new S7Client.S7Protection();
    int errorProtected = client.GetProtection(ref levelProtect);
    Console.WriteLine();
    Console.WriteLine("GetErrorProtection : " + errorProtected);
    Console.WriteLine("Protection level");
    Console.WriteLine("sch_schal : " + levelProtect.sch_schal);
    Console.WriteLine("sch_par : " + levelProtect.sch_par);
    Console.WriteLine("sch_rel : " + levelProtect.sch_rel);
    Console.WriteLine("bart_sch : " + levelProtect.bart_sch);
    Console.WriteLine("anl_sch : " + levelProtect.anl_sch);
    Console.WriteLine();

    int status = -1;
    client.PlcGetStatus(ref status);
    Console.WriteLine("Status : " + status + "\n");

    var dbuffer = new byte[4];
    var buffer = new byte[2];
    int ireadResult;
    int breadResult;


    //7-9 стан

    /*
        ireadResult = client.DBRead(66, 210, 4, dbuffer);
        int tmp = dbuffer[0] * 16777216 + dbuffer[1] * 65536 + dbuffer[2] * 256 + dbuffer[3];
        Console.WriteLine("Byte показания счетчика : " + "\n" + dbuffer[0] + "\n" + dbuffer[1] + "\n" + dbuffer[2] + "\n" + dbuffer[3] + "\n");

        breadResult = client.DBRead(251, 8, 2, buffer);
        bool D01DB251DBX = S7.GetBitAt(buffer, 0, 1);

        breadResult = client.MBRead(4, 4, dbuffer);
        bool mb4_0 = S7.GetBitAt(buffer, 0, 3);
        bool mb7_0= S7.GetBitAt(buffer, 0, 0);


        breadResult = client.MBRead(142, 2, buffer);
        bool mb142_0 = S7.GetBitAt(buffer, 0, 0);

        Console.WriteLine();
        //readResult = client.MBRead(4, 4, buffer);

    */

    ireadResult = client.DBRead(11, 40, 2, buffer);
    int tmp =  buffer[0] * 256 + buffer[1];

    breadResult = client.MBRead(202, 1, buffer);
    //bool D01DB251DBX = S7.GetBitAt(buffer, 0, 1);

    ireadResult = client.DBRead(141, 30, 4, dbuffer);

    var testbyte = new byte[4];
    testbyte[0] = dbuffer[3];
    testbyte[1] = dbuffer[2];
    testbyte[2] = dbuffer[1];
    testbyte[3] = dbuffer[0];



    float temp = BitConverter.ToSingle(testbyte, 0);


    //res = self.convert(result[2]) конвертация флоат
    //    dies_changed = int(res + 1 - 90) > 0 если истина -> замена волок

    breadResult = client.MBRead(142, 2, buffer);
    bool mb142_0 = S7.GetBitAt(buffer, 0, 0);


    ireadResult = client.DBRead(11, 20, 4, dbuffer);
    tmp = dbuffer[0] * 16777216 + dbuffer[1] * 65536 + dbuffer[2] * 256 + dbuffer[3];
    Console.WriteLine();
    //readResult = client.MBRead(4, 4, buffer);












    //int readResult = client.DBRead(1, 0, 4, buffer);
    //    if (readResult == 0) 
    //    {
    //        Console.WriteLine("DB1 Read OK");
    //        0bool db1dbx00 = S7.GetBitAt(buffer, 3, 1);
    //        bool db1dbx02 = S7.GetBitAt(buffer, 0, 2);
    //        bool db1dbx03 = S7.GetBitAt(buffer, 0, 3);
    //        Console.WriteLine(db1dbx00);
    //        Console.WriteLine(db1dbx01);
    //        Console.WriteLine(db1dbx02);
    //        Console.WriteLine(db1dbx03);
    //    }
    //    else
    //    {
    //        Console.WriteLine("DB1 Read Error : " + readResult + "," + client.ErrorText(readResult));
    //    }
    //}
    //else
    //{
    //    Console.WriteLine("Connection ERROR : " + connectionResult + "," + client.ErrorText(connectionResult));
    //}
    client.Disconnect();
}
    Console.WriteLine("Для завершения программы нажмите <Enter>");



Console.ReadLine();