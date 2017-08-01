using GongHaoAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}