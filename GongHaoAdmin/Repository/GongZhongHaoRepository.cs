using Dapper;
using GongHaoAdmin.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace GongHaoAdmin.Repository
{
    public class GongZhongHaoRepository : ConncetionHelper
    {
        public List<Tab_GongZhongHao> GetGZHList(int pageIndex, int pageSize, out int totalPage, out int totalRecord)
        {
            PageCriteria page = new PageCriteria();
            page.TableName = "[Tab_GongZhongHao]";
            page.Fields = "[F_Id], [F_GZHName], [F_WXName], [F_Logo], [F_About], [F_CreateDate]";
            page.Condition = "1 = 1";
            page.Sort = "[F_Id] DESC";
            page.PageSize = pageSize;
            page.CurrentPage = pageIndex;

            return CommonRepository.GetSomeList<Tab_GongZhongHao>(page, out totalPage, out totalRecord);
        }

        public int UpdateGZHInfo(Tab_GongZhongHao m)
        {
            if (m.F_Logo == null && m.F_About == null)
                return 1;

            var sql = new StringBuilder();
            sql.Append("UPDATE [Tab_GongZhongHao]");
            if (m.F_About != null)
            {
                sql.Append(" SET [F_About] = @F_About");
            }
            if (m.F_Logo != null)
            {
                if (m.F_About != null)
                    sql.Append(",[F_Logo] = @F_Logo");
                else
                    sql.Append(" SET [F_Logo] = @F_Logo");
            }
            sql.Append(" WHERE [F_Id] = @F_Id");

            using (SqlConnection conn = new SqlConnection(MHConncetionString))
            {
                return conn.Execute(sql.ToString(), new { F_Id = m.F_Id, F_About = m.F_About, F_Logo = m.F_Logo });
            }
        }

        public Tab_GongZhongHao GetGZH(int uid)
        {
            var sql = @"SELECT c.F_Id, c.F_GZHName, c.F_Logo, c.F_About 
                        FROM Tab_User a 
                        JOIN Tab_User_GZH_Relation b ON b.F_UserId = a.F_Id 
                        JOIN Tab_GongZhongHao c ON c.F_Id = b.F_GZHId 
                        WHERE a.F_Id = @F_Id";

            using (SqlConnection conn = new SqlConnection(MHConncetionString))
            {
                var list = conn.Query<Tab_GongZhongHao>(sql, new { F_Id = uid }).ToList();

                if (list != null && list.Count > 0)
                {
                    Tab_GongZhongHao g = new Tab_GongZhongHao();
                    g.F_Id = list[0].F_Id;
                    g.F_GZHName = list[0].F_GZHName;
                    g.F_About = list[0].F_About;
                    g.F_Logo = list[0].F_Logo;

                    return g;
                }
            }

            return null;
        }

    }
}