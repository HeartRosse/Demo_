using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp6.Lib
{
    public class Connector
    {
        public static DB.user9Entities connection;
        public static DB.User user;
        public static string Login;
        public static DB.user9Entities GetModel()
        {
            if (connection == null)
            {
               connection =  new DB.user9Entities();
            }
            return connection;
        }
        public static DB.User GetUser()
        {
            return user;
        }

        public static bool Authorization(string login,string Password)
        {
            Login = login.Trim();
            string pas = Password.Trim();
            user = GetModel().User.Where( u => Login == u.UserLogin && pas == u.UserPassword).FirstOrDefault();
            return user != null;
        }
    }
}
