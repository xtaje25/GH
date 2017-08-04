using Dapper;
using GongHaoAdmin.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace GongHaoAdmin.Repository
{
    public class MenuRepository : ConncetionHelper
    {
        public List<Tab_Menu> GetUrlList(int userId)
        {
            var sql = @"SELECT f.F_Id, f.F_Name, f.F_URL, f.F_ParentId, f.F_Site FROM dbo.Tab_User a 
                        JOIN dbo.Tab_User_Role_Relation b ON b.F_UserId = a.F_Id 
                        JOIN dbo.Tab_Role_Auth_Relation c ON c.F_RoleId = b.F_RoleId 
                        JOIN dbo.Tab_Authorization d ON d.F_Id = c.F_AuthId 
                        JOIN dbo.Tab_Auth_Menu_Relation e ON e.F_AuthId = c.F_AuthId
                        JOIN dbo.Tab_Menu f ON f.F_Id = e.F_MenuId 
                        WHERE f.F_Site = 2 AND a.F_Id = @F_Id";

            using (SqlConnection conn = new SqlConnection(MHConncetionString))
            {
                var list = conn.Query<Tab_Menu>(sql, new { F_Id = userId });

                return list.ToList();
            }
        }

        public List<Tab_Menu> GetMenuList(int userId)
        {
            var sql = @"SELECT f.F_Id, f.F_Name, f.F_URL, f.F_ParentId, f.F_Site FROM dbo.Tab_User a 
                        JOIN dbo.Tab_User_Role_Relation b ON b.F_UserId = a.F_Id 
                        JOIN dbo.Tab_Role_Auth_Relation c ON c.F_RoleId = b.F_RoleId 
                        JOIN dbo.Tab_Authorization d ON d.F_Id = c.F_AuthId 
                        JOIN dbo.Tab_Auth_Menu_Relation e ON e.F_AuthId = c.F_AuthId
                        JOIN dbo.Tab_Menu f ON f.F_Id = e.F_MenuId 
                        WHERE f.F_Site = 2 AND d.F_AuthType = 'menu' AND a.F_Id = @F_Id";

            using (SqlConnection conn = new SqlConnection(MHConncetionString))
            {
                var list = conn.Query<Tab_Menu>(sql, new { F_Id = userId });

                return list.ToList();
            }
        }

        public List<Tab_Menu> GetMenuCatalog()
        {
            var sql = "SELECT F_Id, F_Name, F_URL, F_ParentId, F_Site FROM dbo.Tab_Menu WHERE F_ParentId = 0";

            using (SqlConnection conn = new SqlConnection(MHConncetionString))
            {
                var list = conn.Query<Tab_Menu>(sql);

                return list.ToList();
            }
        }
    }
}