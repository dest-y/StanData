using Sharp7;

namespace ConsoleApp2
{
    internal class Stan1011 : Stan
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();


        int connectionResult;
        S7Client client = new S7Client();

        int Speed;
        float SpoolLifetimeCurrent;
        float SpoolLifetimeOld = 100;

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
            DrawingChange = false;
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

                    

                    SpeedReadResult = client.DBRead(11, 40, 2, buffer);
                    Speed = buffer[1] * 256 + buffer[0];

                    WireBreakReadResult = client.MBRead(202, 1, buffer);
                    WireBreak = buffer[0] > 0 ? true : false;

                    SpoolLifetimeReadResult = client.DBRead(141, 30, 4, dbuffer);
                    dbuffer = dbuffer.Reverse().ToArray();
                    SpoolLifetimeCurrent = BitConverter.ToSingle(dbuffer, 0);

                    int tmp1 = dbuffer[0] * 16777216 + dbuffer[1] * 65536 + dbuffer[2] * 256 + dbuffer[3];

                    CointerReadResult = client.DBRead(11, 20, 4, dbuffer);                  //По старшему Биту идет сигнал?
                    int tmp = dbuffer[1] * 65536 + dbuffer[2] * 256 + dbuffer[3];           //Переполнение Int!!!
#if DEBUG                                                                                   //По старшему Биту идет сигнал?
                    tmp = tmp * dbuffer[0] * 16777216;                                      //Переполнение Int!!!
#endif
                    //Counter = tmp;


#if DEBUG
                    Logger.Info("Имя стана: {0}; TMPCOINTER: {1}", Name, tmp);
                    Logger.Info("Имя стана: {0}; dbuffer[0]: {1}; dbuffer[1]: {2}; dbuffer[2]: {3}; dbuffer[3]: {4};", Name, dbuffer[0], dbuffer[1], dbuffer[2], dbuffer[3]);
#endif
                    if (CointerErase)
                    {
                        Counter = 0;

                        DrawingChange = SpoolLifetimeCurrent > SpoolLifetimeOld ? true : false;
                        Status = Speed > 5 ? false : true;   //Инверсия статуса false = стан в работе

                        CointerErase = false;

                        return true;
                    }

                    CointerErase = false;               //реализация нажатия обнуления счетчика.
                    if (Counter > tmp)                   //реализация нажатия обнуления счетчика.
                    {                                    //реализация нажатия обнуления счетчика.            
                        CointerErase = true;             //реализация нажатия обнуления счетчика.     
                    }                                    //реализация нажатия обнуления счетчика. 

                    DrawingChange = SpoolLifetimeCurrent > SpoolLifetimeOld ? true : false;

                    Counter = tmp;

                    Status = Speed > 5 ? false : true;   //Инверсия статуса false = стан в работе

                    ReadResult = CointerReadResult + SpoolLifetimeReadResult + SpeedReadResult + WireBreakReadResult;

#if DEBUG
                    Logger.Info("Имя стана: {0}; SpeedReadResult: {1}; WireBreakReadResult: {2}!; SpoolLifetimeReadResult: {3}; CointerReadResult: {4}", Name, SpeedReadResult, WireBreakReadResult, SpoolLifetimeReadResult, CointerReadResult);
                    Logger.Info("Имя стана: {0}; Длина счетчика: {1}; Стан В работе: {2}!; Обрыв(>1 обрыв): {3}; Время жизни волок: {4};Скорость: {5};", Name, Counter, !Status, WireBreak, SpoolLifetimeCurrent, Speed);
#endif
                    if (ReadResult == 0)
                    {
                        SpoolLifetimeOld = SpoolLifetimeCurrent;
                        
                        return true;
                    }
                    return false;
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
