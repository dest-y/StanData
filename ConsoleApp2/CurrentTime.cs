using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public class CurrentTime
    {
        public static string getTime() 
        {
            return "'" + DateTime.Now.ToString() + "'";
        }
    }
}
