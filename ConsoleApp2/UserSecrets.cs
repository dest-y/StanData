using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ConsoleApp2
{
    internal class UserSecrets
    {

        internal string UserID;
        internal string Password;
        internal string HOST;
        internal string SERVICE_NAME;

        public UserSecrets()
        {
            UserID = "";
            Password = "";
            HOST = "";
            SERVICE_NAME = "";
        }

        public void SecretsInit() 
        {
            var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
            UserID = config["UserID"];
            Password = config["Password"];
            HOST = config["HOST"];
            SERVICE_NAME = config["SERVICE_NAME"];
        }
    }
}
