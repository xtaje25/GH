using Dapper;
using GongHaoAdmin.Models;
using System;
using System.Collections.Generic;
using System.Data;
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

        public Tab_User GetUser(int uid)
        {
            var sql = "SELECT [F_Id], [F_Name], [F_Password], [F_CreateDate] FROM [Tab_User] WHERE F_Id = @F_Id";

            using (SqlConnection conn = new SqlConnection(MHConncetionString))
            {
                var list = conn.Query<Tab_User>(sql, new { F_Id = uid }).ToList();

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

        public List<Tab_User> GetUserLis()
        {
            var sql = "SELECT [F_Id], [F_Name], [F_CreateDate] FROM [Tab_User]";
            using (SqlConnection conn = new SqlConnection(MHConncetionString))
            {
                return conn.Query<Tab_User>(sql).ToList();
            }
        }

        public List<Tab_User> GetUserByGzhList(int gid, int pageIndex, int pageSize, out int totalPage, out int totalRecord)
        {
            PageCriteria page = new PageCriteria();
            page.TableName = "Tab_User a JOIN Tab_User_GZH_Relation b ON a.F_Id = b.F_UserId JOIN Tab_User_Role_Relation c ON c.F_UserId = a.F_Id JOIN Tab_Role d ON d.F_Id = c.F_RoleId";
            page.Fields = "a.F_Id, a.F_Name, a.F_CreateDate, d.F_Name RoleName";
            page.Condition = " b.F_GZHId = " + gid;
            page.Sort = "a.F_Id DESC";
            page.PageSize = pageSize;
            page.CurrentPage = pageIndex;

            return CommonRepository.GetSomeList<Tab_User>(page, out totalPage, out totalRecord);
        }

        public int AddUser(Tab_User m)
        {
            var sql = @"INSERT INTO [Tab_User]
                                   ([F_Name]
                                   ,[F_Password]
                                   ,[F_CreateDate])
                             VALUES
                                   (@F_Name
                                   ,@F_Password
                                   ,@F_CreateDate);
                             SELECT SCOPE_IDENTITY();";

            var sql1 = @"INSERT INTO [Tab_User_GZH_Relation]
                                    ([F_UserId]
                                    ,[F_GZHId])
                              VALUES
                                    (@F_UserId
                                    ,@F_GZHId)";

            var sql2 = @"INSERT INTO [Tab_User_Role_Relation]
                                    ([F_UserId]
                                    ,[F_RoleId])
                              VALUES
                                    (@F_UserId
                                    ,@F_RoleId)";

            var sql3 = "SELECT COUNT(*) FROM Tab_User WHERE F_Name = @F_Name;";

            using (SqlConnection conn = new SqlConnection(MHConncetionString))
            {
                conn.Open();
                using (SqlTransaction tran = conn.BeginTransaction(IsolationLevel.RepeatableRead))
                {
                    var n = conn.ExecuteScalar(sql3, new { F_Name = m.F_Name }, tran);

                    if (Convert.ToInt32(n) > 0)
                    {
                        tran.Rollback();
                        return 2;
                    }

                    var r = conn.ExecuteScalar(sql, new { F_Name = m.F_Name, F_Password = m.F_Password, F_CreateDate = DateTime.Now }, tran);

                    int uid = 0;
                    if (r != null && int.TryParse(r.ToString(), out uid) && uid > 0)
                    {
                        var a = conn.Execute(sql1, new { F_UserId = uid, F_GZHId = m.GZHId }, tran);

                        var b = conn.Execute(sql2, new { F_UserId = uid, F_RoleId = m.RoleId }, tran);

                        if (a == 1 && b == 1)
                        {
                            tran.Commit();
                            return 1;
                        }
                        else
                        {
                            tran.Rollback();
                            return 3;
                        }
                    }
                    else
                    {
                        tran.Rollback();
                        return 3;
                    }
                }
            }
        }

        public int UpdateUser(int uid)
        {
            var sql = @"UPDATE [Tab_User]
                           SET [F_Password] = '123456'
                         WHERE [F_Id] = @F_Id";

            using (SqlConnection conn = new SqlConnection(MHConncetionString))
            {
                return conn.Execute(sql, new { F_Id = uid });
            }
        }

        public int DeleteUser(int uid)
        {
            var sql = "DELETE FROM [Tab_User] WHERE F_Id = @F_Id";
            var sql1 = "DELETE FROM [Tab_User_GZH_Relation] WHERE F_UserId = @F_UserId";
            var sql2 = "DELETE FROM [Tab_User_Role_Relation] WHERE F_UserId = @F_UserId";

            using (SqlConnection conn = new SqlConnection(MHConncetionString))
            {
                conn.Open();
                using (SqlTransaction tran = conn.BeginTransaction(IsolationLevel.RepeatableRead))
                {

                    var a = conn.Execute(sql, new { F_Id = uid }, tran);

                    var b = conn.Execute(sql1, new { F_UserId = uid }, tran);

                    var c = conn.Execute(sql2, new { F_UserId = uid }, tran);

                    if (a == 1 && b == 1 && c == 1)
                    {
                        tran.Commit();
                        return 1;
                    }
                    else
                    {
                        tran.Rollback();
                        return 0;
                    }
                }
            }
        }
    }
}