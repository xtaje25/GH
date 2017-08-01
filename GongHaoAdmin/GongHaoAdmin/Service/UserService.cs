using GongHaoAdmin.Models;
using GongHaoAdmin.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GongHaoAdmin.Service
{
    public class UserService
    {
        UserRepository ur = new UserRepository();

        public Tab_User GetUser(string name, string password)
        {
            return ur.GetUser(name, password);
        }

        public int UpdatePassword(int uid, string opwd, string npwd)
        {
            return ur.UpdatePassword(uid, opwd, npwd);
        }
    }
}