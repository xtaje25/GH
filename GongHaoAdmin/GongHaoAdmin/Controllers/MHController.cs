using GongHaoAdmin.Models;
using GongHaoAdmin.Service;
using GongHaoAdmin.Utility;
using Qiniu.Http;
using Qiniu.IO;
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
        private GongZhongHaoService _gzhs = new GongZhongHaoService();
        private MHCatalogService _ms = new MHCatalogService();
        private UserService _us = new UserService();
        private MHImgService _mhs = new MHImgService();

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

            var gzh = _gzhs.GetGZH(u.F_Id);
            var gid = 0;
            if (gzh != null)
            {
                gid = gzh.F_Id;
            }

            var list = _ms.GetMHList(gid, pageIndex, pageSize, out totalPage, out totalRecord);

            VM_Page<Tab_MHCatalog> vm = new VM_Page<Tab_MHCatalog>();
            vm.pageNum = pageIndex;
            vm.numPerPage = pageSize;
            vm.totalcount = totalRecord;
            vm.option = option;
            vm.list = list;

            ViewBag.ca = vm;

            return View();
        }

        [CustomAuthorize]
        [CustomAjaxLogin]
        public ActionResult EditView()
        {
            var option = new int[] { 20, 50, 100, 200 };

            var pageNum = Request.Form["pageNum"];
            var numPerPage = Request.Form["numPerPage"];
            var id = Request["id"];

            var pageIndex = 0;
            var pageSize = 0;
            var totalPage = 0;
            var totalRecord = 0;
            var mhid = 0;

            int.TryParse(pageNum, out pageIndex);
            int.TryParse(numPerPage, out pageSize);
            int.TryParse(id, out mhid);

            pageIndex = pageIndex == 0 ? 1 : pageIndex;
            pageSize = pageSize == 0 ? 50 : pageSize;

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
            var gzh = _gzhs.GetGZH(u.F_Id);
            ViewBag.gzh = gzh;

            var list = _mhs.GetMHImgList(mhid, pageIndex, pageSize, out totalPage, out totalRecord);

            VM_Page<Tab_MHImg> vm = new VM_Page<Tab_MHImg>();
            vm.pageNum = pageIndex;
            vm.numPerPage = pageSize;
            vm.totalcount = totalRecord;
            vm.option = option;
            vm.list = list;
            ViewBag.ca = vm;

            var mh = _ms.GetMH(mhid);
            ViewBag.mh = mh;

            return View();
        }

        [CustomAuthorize]
        [CustomAjaxLogin]
        public ActionResult Edit()
        {
            return Json(new { });
        }

        [CustomAuthorize]
        [CustomAjaxLogin]
        public ActionResult AddView()
        {
            var id = Request.QueryString["id"];
            int mhid = 0;
            int.TryParse(id, out mhid);

            if (mhid > 0)
            {
                var mh = _ms.GetMH(mhid);
                if (mh != null)
                {
                    int i = _mhs.GetMaxSort(mhid);

                    ViewBag.mh = mh;
                    ViewBag.max = i + 1;
                }
            }

            return View();
        }

        [CustomAuthorize]
        [CustomAjaxLogin]
        public ActionResult Add()
        {
            var id = Request.Form["mhid"];
            var sort = Request.Form["sort"]; // 章节ID
            var name = Request.Form["name"];

            var gid = 0;
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
            var gzh = _gzhs.GetGZH(u.F_Id);
            if (gzh != null) gid = gzh.F_Id;

            if (gid == 0)
            {
                // IE浏览器对非ajax请求Content-Type:是json的不友好所以使用View而非Json
                return View(new DWZJson() { statusCode = (int)DWZStatusCode.ERROR, message = "账号没有关联公众号" });
            }

            int mhid = 0;
            if (sort == null || !int.TryParse(id, out mhid) || mhid == 0)
            {
                return View(new DWZJson() { statusCode = (int)DWZStatusCode.ERROR, message = "公号ID无效" });
            }

            int zjid = 0;
            if (sort == null || !int.TryParse(sort, out zjid) || zjid == 0)
            {
                return View(new DWZJson() { statusCode = (int)DWZStatusCode.ERROR, message = "章节ID无效" });
            }

            if (name == null || name.Length > 50)
            {
                return View(new DWZJson() { statusCode = (int)DWZStatusCode.ERROR, message = "章节名称长度必须小于50字符" });
            }

            var img = "";
            if (Request.Files.Count > 0
               && Request.Files[0].ContentLength > 0
               && new string[] { ".gif", ".jpeg", ".jpg", ".png" }.Contains(System.IO.Path.GetExtension(Request.Files[0].FileName.ToLower())))
            {
                var key = QN.MHimg(gid, mhid);

                FormUploader fu = new FormUploader();
                HttpResult result = fu.UploadStream(Request.Files[0].InputStream, key, QN.GetUploadToken(QN.BUCKET, key));
                if (result.Code == 200)
                {
                    img = key;
                }
            }
            else
            {
                return View(new DWZJson() { statusCode = (int)DWZStatusCode.ERROR, message = "请为章节内容添加图片" });
            }

            Tab_MHImg m = new Tab_MHImg();
            m.F_Name = name;
            m.F_Img = img != "" ? img : null;
            m.F_MHId = mhid;
            m.F_Sort = zjid;
            m.F_UserId = u.F_Id;

            var i = _mhs.AddImg(m);

            if (i == 1)
            {
                return View(new DWZJson() { statusCode = (int)DWZStatusCode.OK, message = "成功" });
            }
            else if (i == 2)
            {
                return View(new DWZJson() { statusCode = (int)DWZStatusCode.ERROR, message = "章节重复" });
            }
            else
            {
                return View(new DWZJson() { statusCode = (int)DWZStatusCode.ERROR, message = "失败" });
            }
        }
    }
}