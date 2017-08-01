using Dapper;
using GongHaoAdmin.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace GongHaoAdmin.Repository
{
    public class UserRepository : ConncetionHelper
    {
        public Tab_User GetUser(string name, string password)
        {
            var sql = "SELECT [F_Id], [F_Name], [F_Password], [F_CreateDate] FROM [Tab_User] WHERE F_Name = @F_Name AND F_Password = @F_Password";

            using (SqlConnection conn = new SqlConnection(MHConncetionString))
            {
                var list = conn.Query<Tab_User>(sql, new { F_Name = name, F_Password = password }).ToList();

                if (list != null && list.Count > 0)
                {
                    Tab_User u = new Tab_User();
                    u.F_Id = list[0].F_Id;
                    u.F_Name = list[0].F_Name;
                    u.F_Password = list[0].F_Password;

                    return u;
                }
                else
                {
                    return null;
                }
            }
        }

        public int UpdatePassword(int uid, string opwd, string npwd)
        {
            var sql = "UPDATE dbo.Tab_User SET F_Password = @F_Password WHERE F_Id = @F_Id AND F_Password = @F_Password2";

            using (SqlConnection conn = new SqlConnection(MHConncetionString))
            {
                return conn.Execute(sql, new { F_Password = npwd, F_Id = uid, F_Password2 = opwd });
            }
        }
    }
}