using GongHaoAdmin.Models;
using GongHaoAdmin.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GongHaoAdmin.Controllers
{
    public class UserController : Controller
    {
        private GongZhongHaoService _gzhs = new GongZhongHaoService();

        public ActionResult IndexView()
        {
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

            var list = _gzhs.GetGZHList(pageIndex, pageSize, out totalPage, out totalRecord);

            VM_Page<Tab_GongZhongHao> vm = new VM_Page<Tab_GongZhongHao>();
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