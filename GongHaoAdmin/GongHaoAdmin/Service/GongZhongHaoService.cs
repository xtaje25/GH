using GongHaoAdmin.Models;
using GongHaoAdmin.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GongHaoAdmin.Service
{
    public class GongZhongHaoService
    {
        GongZhongHaoRepository gzhr = new GongZhongHaoRepository();

        public List<Tab_GongZhongHao> GetGZHList(int pageIndex, int pageSize, out int totalPage, out int totalRecord)
        {
            return gzhr.GetGZHList(pageIndex, pageSize, out totalPage, out totalRecord);
        }
    }
}