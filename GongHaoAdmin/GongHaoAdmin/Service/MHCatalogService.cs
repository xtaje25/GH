using GongHaoAdmin.Models;
using GongHaoAdmin.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GongHaoAdmin.Service
{
    public class MHCatalogService
    {
        MHCatalogRepository mr = new MHCatalogRepository();

        public List<Tab_MHCatalog> GetMHList(int pageIndex, int pageSize, out int totalPage, out int totalRecord)
        {
            return mr.GetMHList(pageIndex, pageSize, out totalPage, out totalRecord);
        }

        public int UpdateMH(Tab_MHCatalog m)
        {
            return mr.UpdateMH(m);
        }
    }
}