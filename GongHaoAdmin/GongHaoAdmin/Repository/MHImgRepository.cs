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
    public class MHImgRepository : ConncetionHelper
    {
        public List<Tab_MHImg> GetMHImgList(int mhid, int pageIndex, int pageSize, out int totalPage, out int totalRecord)
        {
            PageCriteria page = new PageCriteria();
            page.TableName = "[Tab_MHImg]";
            page.Fields = "[F_Id], [F_Name], [F_Img], [F_MHId], [F_Sort], [F_IsEnable], [F_UserId], [F_CreateDate]";
            page.Condition = "[F_MHId] = " + mhid + " AND [F_IsEnable] = 1";
            page.Sort = "[F_Sort] DESC";
            page.PageSize = pageSize;
            page.CurrentPage = pageIndex;

            return CommonRepository.GetSomeList<Tab_MHImg>(page, out totalPage, out totalRecord);
        }

        public int AddImg(Tab_MHImg m)
        {
            var sql = @"INSERT INTO [Tab_MHImg]
                                   ([F_Name]
                                   ,[F_Img]
                                   ,[F_MHId]
                                   ,[F_Sort]
                                   ,[F_IsEnable]
                                   ,[F_UserId]
                                   ,[F_CreateDate])
                             VALUES
                                   (@F_Name
                                   ,@F_Img
                                   ,@F_MHId
                                   ,@F_Sort
                                   ,@F_IsEnable
                                   ,@F_UserId
                                   ,@F_CreateDate)";

            var sql1 = "SELECT COUNT(*) FROM [Tab_MHImg] WHERE [F_MHId] = @F_MHId AND [F_Sort] = @F_Sort";

            using (SqlConnection conn = new SqlConnection(MHConncetionString))
            {
                conn.Open();
                using (SqlTransaction tran = conn.BeginTransaction(IsolationLevel.RepeatableRead))
                {
                    var a = conn.ExecuteScalar(sql1, new { F_MHId = m.F_MHId, F_Sort = m.F_Sort }, tran);

                    if (0 == Convert.ToInt32(a))
                    {
                        int r = conn.Execute(sql, new
                        {
                            F_Name = m.F_Name,
                            F_Img = m.F_Img,
                            F_MHId = m.F_MHId,
                            F_Sort = m.F_Sort,
                            F_IsEnable = true,
                            F_UserId = m.F_UserId,
                            F_CreateDate = DateTime.Now,
                        }, tran);

                        if (r == 1)
                        {
                            tran.Commit();
                            return 1;
                        }
                    }

                    if (Convert.ToInt32(a) > 0)
                    {
                        tran.Rollback();
                        return 2;
                    }
                }
            }

            return 0;
        }

        public int GetMaxSort(int mhid)
        {
            var sql = "SELECT MAX(F_Sort) FROM dbo.Tab_MHImg WHERE [F_MHId] = @F_MHId";

            using (SqlConnection conn = new SqlConnection(MHConncetionString))
            {
                object r = conn.ExecuteScalar(sql, new { F_MHId = mhid });

                return Convert.ToInt32(r);
            }
        }
    }
}