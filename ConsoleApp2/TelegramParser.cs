using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    internal class TelegramParser
    {
        private bool? status;
        private string name;
        private bool WireBreak;
        private bool DrawingChange;
        private bool CointerErase;
        private bool Changed;
        private int Counter = 0;
        private Stan cstan;
        internal string? TelegramData;

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public TelegramParser(Stan stan)
        {
            status = null;
            name = stan.Name;
            Changed = true;
            DrawingChange = true;
            CointerErase = true;
            Changed = true;
            cstan = stan;
            TelegramData = null;
        }
        internal bool CheckUpdate() 
        {
            cstan.getData();
            //Logger.Info("Имя стана: {0}; Изменено: {1}; Длина счетчика: {2}; Стан В работе: {3}!; Обрыв: {4}; Смена волок: {5};Сброс счётчика: {6};" ,name, Changed, Counter, !status, WireBreak, DrawingChange, CointerErase);
            if ((cstan.DrawingChange && !DrawingChange) || (status != cstan.Status) || (cstan.CointerErase && !CointerErase) || (cstan.WireBreak && !WireBreak))
            {
                Changed = true;
                Logger.Info("Данные изменены, отправлена телеграмма");
                TelegramData = CreateTelegram();
                Logger.Info(TelegramData);
            }
            if (Changed)  // Убрать проверку?
            {
                status = cstan.Status;
                WireBreak = cstan.WireBreak;
                DrawingChange = cstan.DrawingChange;
                CointerErase = cstan.CointerErase;
                Counter = cstan.Counter;
                Changed = false;
                return true;
            }
            return false;
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
            //Logger.Error(CurrentTime.getTime() + " " + "S" + "01" + "D" + cstan.Name.PadLeft(3, '0') + cstan.Counter.ToString().PadLeft(6, '0') + stateOne + stateTwo + stateThree + "0000");
            Logger.Error( CurrentTime.getTime() + "D" + "  " + cstan.Name + "  " + Convert.ToInt32(cstan.Status) +"  " + Convert.ToInt32(cstan.CointerErase) + "  " + Convert.ToInt32(cstan.WireBreak) + "  " + Convert.ToInt32(DrawingChange) + "  " + cstan.Counter);
            return CurrentTime.getTime() + " " + "S" + "01" + "D" + cstan.Name.PadLeft(3, '0') + cstan.Counter.ToString().PadLeft(6, '0') + stateOne + stateTwo + stateThree;
        }

    }

        
    
}
