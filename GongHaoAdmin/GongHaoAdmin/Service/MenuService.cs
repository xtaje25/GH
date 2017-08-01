using GongHaoAdmin.Models;
using GongHaoAdmin.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GongHaoAdmin.Service
{
    public class MenuService
    {
        MenuRepository mr = new MenuRepository();

        public List<Tab_Menu> GetUrlList(int userId)
        {
            return mr.GetUrlList(userId);
        }

        public List<Tab_Menu> GetMenuList(int userId)
        {
            return mr.GetMenuList(userId);
        }

        public List<Tab_Menu> GetMenuCatalog()
        {
            return mr.GetMenuCatalog();
        }
    }
}