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
    public class CommonRepository : ConncetionHelper
    {
        public static List<T> GetSomeList<T>(PageCriteria page, out int totalPage, out int totalRecord)
        {
            totalPage = 0;
            totalRecord = 0;
            var list = new List<T>();
            using (SqlConnection conn = new SqlConnection(MHConncetionString))
            {
                var p = new DynamicParameters();
                p.Add("@tableName", page.TableName);
                p.Add("@tableFields", page.Fields);
                p.Add("@sqlWhere", page.Condition);
                p.Add("@orderFields", page.Sort);
                p.Add("@pageSize", page.PageSize);
                p.Add("@pageIndex", page.CurrentPage);
                p.Add("@totalPage", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@totalRecord", dbType: DbType.Int32, direction: ParameterDirection.Output);

                list = conn.Query<T>("Module_Common_PagerNew", p, commandType: CommandType.StoredProcedure).ToList();

                totalPage = p.Get<int>("@totalPage");
                totalRecord = p.Get<int>("@totalRecord");
            }
            return list;
        }
    }
}