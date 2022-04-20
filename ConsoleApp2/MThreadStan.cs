using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    internal class MThreadStan
    {
        TelegramParser Telegram;
        SqliteClass SqliteConnection;
        int Ttimer;
        internal MThreadStan(TelegramParser telegram, int timer)
        {
            Telegram = telegram;
            Ttimer = timer;
            SqliteConnection = new SqliteClass();

            Thread thread = new Thread(new ThreadStart(StartThread));
            thread.Start();
        }

        void StartThread() 
        {
            while (true)
            {
                if (Telegram.CheckUpdate())
                {
                    SqliteConnection.insertTestString(Telegram);
                }
                Thread.Sleep(Ttimer);
            }
        }

    }
    internal class MThreadOrace
    {
        SqliteDataTransfer SqllOra;
        Thread thread;
        public System.Threading.ManualResetEvent OracleEvent;
        internal MThreadOrace(SqliteDataTransfer sqllOra)
        {
            SqllOra = sqllOra;


            thread = new Thread(new ThreadStart(StartThread));
            thread.Start();
            thread.IsBackground = true;

            OracleEvent = new ManualResetEvent(false);
            OracleEvent.Set();


        }

        void StartThread()
        {
            while (true)
            {


                OracleEvent.WaitOne(15000);  //  ожидаем событие возникновения телеграмммы от СТАНОВ 
                // каждые 15 секунд НА СЛУЧАЙ ЗАВИСАНИЯ проверяем ЛОКАЛЬНУЮ БД на наличие необработанных записей

                OracleEvent.Reset(); // флаг ожидания потока оракла сбрасываем сразу, чтобы понять, что телеграм новых не пришло.

                Int64 result;
                while (true) // пока есть необработанные телеграммы передаем их в оракл
                {
                    result = SqllOra.TransferNotPostedTelegrams();
                    if (result > 0)
                    {
                        Thread.Sleep(1000); // перекур между транзакциями с секунда, продолжаем отправлять записи в ОРАЛК по 10 штук
                        continue;
                    }
                    if (result == -1)
                    {
                        Thread.Sleep(15000); //  ошибка оракла пробуем переключаться к ораклу каждые 15 секунд
                        continue;
                    }
                    if (result == 0)// все записи обработаны ждем событие  следующей телеграммы
                    {
                        if (OracleEvent.WaitOne(0))  // проверим, во время обработки БД ОРАКЛ пришла еще новая ТЕЛЕГРАММА?
                        {
                            OracleEvent.Reset(); // флаг ожидания потока оракла сбрасываем сразу, чтобы понять, что телеграм новых не пришло.
                            continue;  // да телеграмма пришла
                        }
                        else
                        { // нет время ожидания события вышло, событие не было установлено
                            OracleEvent.Reset();
                            break;  // выходим из цикла обработки транзакций 
                        }
                    }

                }


            }
        }

    }

}
