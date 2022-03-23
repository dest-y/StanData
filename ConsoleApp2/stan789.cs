using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharp7;

namespace ConsoleApp2
{
    public class Stan789
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private string Name;
        private string IpAddress;
        private int Counter;
        private bool Status;
        private bool WireBreak;
        private bool DrawingChange;
        private bool CointerErase = false;
        int connectionResult;
        S7Client client = new();

        public Stan789(string name, string ipAddress, int counter = 0, bool status = false)
        {
            Name = name;
            IpAddress = ipAddress;
            Counter = counter;
            Status = status;

            connectionResult = client.ConnectTo(IpAddress, 0, 3); //10-11 стан 
            if (connectionResult == 0)
            {
                Console.WriteLine("Connection OK");
                PrintName();
            }
        }

        public bool getData() 
        {
            try
            {
                if (connectionResult == 0)
                {
                    var dbuffer = new byte[4];
                    var buffer = new byte[2];
                    int ireadResult;
                    int breadResult;

                    client.SetConnectionType(S7Client.CONNTYPE_BASIC);

                    ireadResult = client.DBRead(66, 210, 4, dbuffer);
                    int tmp = dbuffer[0] * 16777216 + dbuffer[1] * 65536 + dbuffer[2] * 256 + dbuffer[3];
                    Counter = tmp;

                    breadResult = client.DBRead(251, 8, 2, buffer);
                    bool D01DB251DBX = S7.GetBitAt(buffer, 1, 1);
                    DrawingChange = D01DB251DBX;

                    breadResult = client.MBRead(4, 4, dbuffer);
                    bool mb4_0 = S7.GetBitAt(dbuffer, 0, 0);
                    bool mb7_0 = S7.GetBitAt(dbuffer, 3, 0);
                    Status = mb4_0;
                    CointerErase = mb7_0;

                    breadResult = client.MBRead(142, 2, buffer);
                    bool mb142_0 = S7.GetBitAt(buffer, 0, 0);
                    WireBreak = mb142_0;

                    //Console.WriteLine("Длина счетчика : " + Counter);
                    //Console.Write("Замена Волок: ");
                    //Console.WriteLine(DrawingChange);
                    //Console.Write("Стан в работе(если false значит ON): ");
                    //Console.WriteLine(Status);
                    //Console.Write("Сброс счетчика: ");
                    //Console.WriteLine(CointerErase);
                    //Console.Write("Обрыв проволоки: ");
                    //Console.WriteLine(WireBreak);
                    //Console.WriteLine("Конец данных");
                    Logger.Info("Имя стана: {0}; Длина счетчика: {1}; Стан В работе: {2}!; Обрыв: {3}; Смена волок: {4};", Name, Counter, !Status, WireBreak, CointerErase);

                    return true;
                }
                else 
                {
                    connectionResult = client.ConnectTo(IpAddress, 0, 3);
                    Logger.Info("Имя стана: {0} - Недоступен; Попытка переподключения - Connection result = {1}", Name, connectionResult);
                    return false;
                }
            }
            catch 
            {
                client.Disconnect();
                return false;
            }
        }

        public void PrintName()
        {
            Console.WriteLine("Имя Стана: {0}", Name);
        }

        public void ClientDisc()
        {
            client.Disconnect();
        }
    }
}
