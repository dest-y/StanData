using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.Interfaces
{
    interface IConnectionHandler
    {
        void Create();
        void Close();
        void Restart();
        bool ExecuteCommand(string CommandString);
        void StartTransaction();
        void CommitTransaction();
        void RollbackTransaction();
    }
}
