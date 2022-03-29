using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    internal class TelegramParser
    {
        private bool status;
        private string name;
        private bool WireBreak;
        private bool DrawingChange;
        private bool CointerErase;
        private bool Changed;
        private int Counter = 0;
        private Stan cstan;

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public TelegramParser(Stan stan)
        {
            status = true;
            name = stan.Name;
            Changed = true;
            DrawingChange = true;
            CointerErase = true;
            Changed = true;
            cstan = stan;
        }
        internal string CheckUpdate() 
        {
            cstan.getData();
            //Logger.Info("Имя стана: {0}; Изменено: {1}; Длина счетчика: {2}; Стан В работе: {3}!; Обрыв: {4}; Смена волок: {5};Сброс счётчика: {6};" ,name, Changed, Counter, !status, WireBreak, DrawingChange, CointerErase);
            if ((cstan.DrawingChange && DrawingChange == !cstan.DrawingChange) || status != cstan.Status || (cstan.CointerErase && CointerErase == !cstan.CointerErase) || (cstan.WireBreak && WireBreak == !cstan.WireBreak))
            {
                Changed = true;
                Logger.Info("Данные изменены нужна телеграмма");
                Logger.Info(CreateTelegram());
            }


            if (Changed) 
            {
                Logger.Info("Данные парсера перезаписаны");
                status = cstan.Status;
                WireBreak = cstan.WireBreak;
                DrawingChange = cstan.DrawingChange;
                CointerErase = cstan.CointerErase;
                Counter = cstan.Counter;
                Changed = false;
            }
            return "test";
        }
        public string CreateTelegram() 
        {
            CurrentTime.getTime();
            string stateOne = "0";
            string stateTwo = "0";
            string stateThree = "0";

            if (!cstan.Status)
            {
                stateOne = "8";
            }
            else 
            {
                stateOne = "0";
            }
            if (cstan.WireBreak)
            {
                stateTwo = "8";
            }
            if (cstan.CointerErase)
            {
                stateTwo = "4";
            }
            if (cstan.DrawingChange)
            {
                stateThree = "1";
            }
            Logger.Error(CurrentTime.getTime() + " " + "S" + "01" + "D" + cstan.Name.PadLeft(3, '0') + cstan.Counter.ToString().PadLeft(6, '0') + stateOne + stateTwo + stateThree + "0000");
            return CurrentTime.getTime() + "D" + "  " + cstan.Name + "  " + Convert.ToInt32(cstan.Status) +"  " + Convert.ToInt32(cstan.CointerErase) + "  " + Convert.ToInt32(cstan.WireBreak) + "  " + Convert.ToInt32(DrawingChange) + "  " + cstan.Counter;
            //return CurrentTime.getTime() + " " + "S" + "01" + "D" + cstan.Name.PadLeft(3, '0') + cstan.Counter.ToString().PadLeft(6, '0') + stateOne + stateTwo + stateThree + "0000";
        }

    }

        
    
}
