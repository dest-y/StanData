using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharp7;

namespace ConsoleApp2
{
    internal class Stan1011 : Stan
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public int WireBreak;
        int connectionResult;
        S7Client client = new S7Client();

        int Speed;
        float SpoolLifetime;

        public Stan1011(string name, string ipAddress, int counter = 0, bool status = false)
        {
            Name = name;
            IpAddress = ipAddress;
            Counter = counter;
            Status = status;

            client.SetConnectionType(S7Client.CONNTYPE_BASIC);

            connectionResult = client.ConnectTo(IpAddress, 0, 2); //10-11 стан 
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
                    int WireBreakReadResult;
                    int ReadResult;
                    int SpeedReadResult;
                    int SpoolLifetimeReadResult;

                    CointerErase = false;               //реализация нажатия обнуления счетчика.

                    SpeedReadResult = client.DBRead(11, 40, 2, buffer);
                    Speed = buffer[1] * 256 + buffer[0];

                    WireBreakReadResult = client.MBRead(202, 1, buffer);
                    WireBreak = buffer[0];

                    SpoolLifetimeReadResult = client.DBRead(141, 30, 4, dbuffer);
                    dbuffer = dbuffer.Reverse().ToArray();
                    SpoolLifetime = BitConverter.ToSingle(dbuffer, 0);

                    CointerReadResult = client.DBRead(11, 20, 4, dbuffer);
                    int tmp = dbuffer[0] * 16777216 + dbuffer[1] * 65536 + dbuffer[2] * 256 + dbuffer[3];

                    if (Counter > tmp)                   //реализация нажатия обнуления счетчика.
                    {                                    //реализация нажатия обнуления счетчика.            
                        CointerErase = true;             //реализация нажатия обнуления счетчика.     
                    }                                    //реализация нажатия обнуления счетчика. 

                    Counter = tmp;

                    Status = Speed > 100 ? false : true;   //Инверсия статуса false = стан в работе


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
                    ReadResult = CointerReadResult + SpoolLifetimeReadResult + SpeedReadResult + WireBreakReadResult;
                    if (ReadResult == 0)
                    {
                        Logger.Info("Имя стана: {0}; Длина счетчика: {1}; Стан В работе: {2}!; Обрыв(>1 обрыв): {3}; Время жизни волок: {4};Скорость: {5};", Name, Counter, !Status, WireBreak, SpoolLifetime, Speed);
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
                Logger.Info("Имя стана: {0} - Недоступен; Попытка переподключения - Connection result = {1}", Name, connectionResult);
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
