using GongHaoAdmin.Models;
using GongHaoAdmin.Service;
using GongHaoAdmin.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace GongHaoAdmin.Controllers
{
    public class MHController : Controller
    {
        private MHCatalogService _ms = new MHCatalogService();
        private UserService _us = new UserService();

        [CustomAuthorize]
        [CustomAjaxLogin]
        public ActionResult IndexView()
        {
            Tab_User u = null;
            HttpCookie authCookie = Request.Cookies["a"]; // 获取cookie
            if (authCookie != null)
            {
                try
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value); // 解密
                    var user = SerializeHelper.FromJson<Tab_User>(ticket.UserData);
                    u = _us.GetUser(user.F_Name, user.F_Password);
                    if (u != null)
                    {
                    }
                    else
                    {
                        return RedirectToAction("SignOut", "Home");
                    }
                }
                catch (Exception ex)
                {
                    return RedirectToAction("SignOut", "Home");
                }
            }

            var option = new int[] { 20, 50, 100, 200 };

            var pageNum = Request.Form["pageNum"];
            var numPerPage = Request.Form["numPerPage"];

            var pageIndex = 0;
            var pageSize = 0;
            var totalPage = 0;
            var totalRecord = 0;

            int.TryParse(pageNum, out pageIndex);
            int.TryParse(numPerPage, out pageSize);

            pageIndex = pageIndex == 0 ? 1 : pageIndex;
            pageSize = pageSize == 0 ? 50 : pageSize;

            var list = _ms.GetMHList(pageIndex, pageSize, out totalPage, out totalRecord);

            VM_Page<Tab_MHCatalog> vm = new VM_Page<Tab_MHCatalog>();
            vm.pageNum = pageIndex;
            vm.numPerPage = pageSize;
            vm.totalcount = totalRecord;
            vm.option = option;
            vm.list = list;

            ViewBag.ca = vm;

            return View();
        }
    }
}