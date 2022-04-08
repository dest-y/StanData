using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharp7;

namespace ConsoleApp2
{
    internal class Stan789 : Stan
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        int connectionResult;
        int SpoolLifetimeCurrent;
        int SpoolLifetimeOld = 100;
        int SpoolLifetimeReadResult;

        S7Client client = new S7Client();

        public Stan789(string name, string ipAddress, int counter = 0, bool status = false)
        {
            Name = name;
            IpAddress = ipAddress;
            Counter = counter;
            Status = status;

            client.SetConnectionType(S7Client.CONNTYPE_BASIC);

            connectionResult = client.ConnectTo(IpAddress, 0, 3); //10-11 стан 
            if (connectionResult == 0)
            {
                Console.WriteLine("Connection OK");
                PrintName();
            }
        }

        override public bool getData() 
        {
            connectionResult = client.Connect();
            try
            {
                if (connectionResult == 0)
                {
                    var dbuffer = new byte[4];
                    var buffer = new byte[2];
                    int CointerReadResult;
                    int StatusReadResult;
                    int DrawingChangeReadResult;
                    int WireBreakReadResult;
                    int ReadResult;

                    CointerReadResult = client.DBRead(66, 210, 4, dbuffer);
                    int tmp = dbuffer[0] * 16777216 + dbuffer[1] * 65536 + dbuffer[2] * 256 + dbuffer[3];
                    Counter = tmp;

                    DrawingChangeReadResult = client.DBRead(251, 8, 2, buffer);
                    bool D01DB251DBX = S7.GetBitAt(buffer, 1, 1);
                    DrawingChange = D01DB251DBX;

                    StatusReadResult = client.MBRead(4, 4, dbuffer);
                    bool mb4_0 = S7.GetBitAt(dbuffer, 0, 0);
                    bool mb7_0 = S7.GetBitAt(dbuffer, 3, 0);
                    Status = mb4_0;
                    CointerErase = mb7_0;

                    WireBreakReadResult = client.MBRead(142, 2, buffer);
                    bool mb142_0 = S7.GetBitAt(buffer, 0, 0);
                    WireBreak = mb142_0;

                    SpoolLifetimeReadResult = client.DBRead(251, 64, 2, buffer);
                    SpoolLifetimeCurrent = buffer[0] * 256 + buffer[1];

                    DrawingChange = SpoolLifetimeCurrent > SpoolLifetimeOld ? true : false;

                    ReadResult = CointerReadResult + DrawingChangeReadResult + StatusReadResult + WireBreakReadResult;
                    if (ReadResult == 0)
                    {
                        SpoolLifetimeOld = SpoolLifetimeCurrent;
                        Logger.Info("Имя стана: {0}; Длина счетчика: {1}; Стан В работе: {2}!; Обрыв: {3}; Смена волок: {4};Сброс счётчика: {5};Время жизни волок: {6};", Name, Counter, !Status, WireBreak, DrawingChange, CointerErase, SpoolLifetimeCurrent);
                    }
                    return true;
                }
                else 
                {
                    Logger.Info("Имя стана: {0} - Недоступен; Попытка переподключения - Connection result = {1}", Name, connectionResult);
                    client.Disconnect();
                    return false;
                }
            }
            catch 
            {
                Logger.Info("Имя стана: {0} - Ошибка подключения; Попытка переподключения - Connection result = {1}", Name, connectionResult);
                client.Disconnect();
                return false;
            }
        }

        public string PrintName()
        {
            Console.WriteLine("Имя Стана: {0}", Name);
            return Name;
        }

        internal void ClientDisc()
        {
            client.Disconnect();
        }
    }
}
