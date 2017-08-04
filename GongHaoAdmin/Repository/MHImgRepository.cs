using Dapper;
using GongHaoAdmin.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
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

        public int DeleteImg(int id)
        {
            var sql = "DELETE FROM dbo.Tab_MHImg WHERE F_Id = @F_Id";

            using (SqlConnection conn = new SqlConnection(MHConncetionString))
            {
                return conn.Execute(sql, new { F_Id = id });
            }
        }

        public Tab_MHImg GetImg(int img)
        {
            var sql = @"SELECT [F_Id]
                              ,[F_Name]
                              ,[F_Img]
                              ,[F_MHId]
                              ,[F_Sort]
                              ,[F_IsEnable]
                              ,[F_UserId]
                              ,[F_CreateDate]
                          FROM [Tab_MHImg]
                         WHERE [F_Id] = @F_Id";

            using (SqlConnection conn = new SqlConnection(MHConncetionString))
            {
                var list = conn.Query<Tab_MHImg>(sql, new { F_Id = img }).ToList();

                if (list != null && list.Count > 0)
                {
                    Tab_MHImg m = new Tab_MHImg();
                    m.F_Id = list[0].F_Id;
                    m.F_Name = list[0].F_Name;
                    m.F_Img = list[0].F_Img;
                    m.F_MHId = list[0].F_MHId;
                    m.F_Sort = list[0].F_Sort;
                    m.F_IsEnable = list[0].F_IsEnable;
                    m.F_UserId = list[0].F_UserId;
                    m.F_CreateDate = list[0].F_CreateDate;

                    return m;
                }
            }

            return null;
        }

        public int UpdateImg(Tab_MHImg m)
        {
            var sql = new StringBuilder();
            sql.Append("UPDATE [Tab_MHImg]");
            sql.Append("   SET [F_Name] = @F_Name");
            if (m.F_Img != null)
            {
                sql.Append("  ,[F_Img] = @F_Img");
            }
            sql.Append("      ,[F_Sort] = @F_Sort");
            sql.Append("      ,[F_UserId] = @F_UserId");
            sql.Append(" WHERE [F_Id] = @F_Id");
            sql.Append("   AND [F_MHId] = @F_MHId");

            using (SqlConnection conn = new SqlConnection(MHConncetionString))
            {
                return conn.Execute(sql.ToString(), new
                {
                    F_Name = m.F_Name,
                    F_Img = m.F_Img,
                    F_Sort = m.F_Sort,
                    F_UserId = m.F_UserId,
                    F_Id = m.F_Id,
                    F_MHId = m.F_MHId,
                });
            }
        }
    }
}