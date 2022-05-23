using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigniSightModel
{
    public class User
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string AccountType { get; set; }

        public string LangSet { get; set; }

        public User()
        {
            Username = "LoremIpsum";
            Password = "Password";
            AccountType = "basic";
            LangSet = "English";
        }

        public override string ToString()
        {
            return $" UserName - {Username}, Password - {Password}, LangSet - {LangSet}";
        }
    }
}
