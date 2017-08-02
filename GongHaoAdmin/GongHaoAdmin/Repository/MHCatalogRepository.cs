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
    public class MHCatalogRepository : ConncetionHelper
    {
        public List<Tab_MHCatalog> GetMHList(int pageIndex, int pageSize, out int totalPage, out int totalRecord)
        {
            PageCriteria page = new PageCriteria();
            page.TableName = "[Tab_MHCatalog] a JOIN [Tab_GongZhongHao] b ON a.[F_GZHId] = b.[F_Id] JOIN [dbo].[Tab_User] c ON c.F_Id = a.[F_CreateUser]";
            page.Fields = "a.[F_Id], a.[F_Catalog], a.[F_Logo], a.[F_GZHId], c.[F_Name] [userName], a.[F_CreateUser], a.[F_CreateDate], b.[F_GZHName] [GZHName]";
            page.Condition = "1 = 1";
            page.Sort = "a.[F_Id] DESC";
            page.PageSize = pageSize;
            page.CurrentPage = pageIndex;

            return CommonRepository.GetSomeList<Tab_MHCatalog>(page, out totalPage, out totalRecord);
        }

        public int UpdateMH(Tab_MHCatalog m)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("UPDATE [Tab_MHCatalog]");
            sql.Append("   SET [F_Catalog] = @F_Catalog");
            sql.Append("      ,[F_GZHId] = @F_GZHId");
            if (m.F_Logo != null)
            {
                sql.Append("  ,[F_Logo] = @F_Logo");
            }
            sql.Append(" WHERE [F_Id] = @F_Id");

            using (SqlConnection conn = new SqlConnection(MHConncetionString))
            {
                return conn.Execute(sql.ToString(), new { F_Catalog = m.F_Catalog, F_GZHId = m.F_GZHId, F_Id = m.F_Id, F_Logo = m.F_Logo });
            }
        }
    }
}