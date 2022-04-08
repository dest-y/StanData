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

        internal MThreadStan(TelegramParser telegram)
        {
            Telegram = telegram;
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
                Thread.Sleep(700);
            }
        }

    }


}
