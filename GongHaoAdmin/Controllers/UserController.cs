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
    public class UserController : Controller
    {
        private GongZhongHaoService _gzhs = new GongZhongHaoService();
        private UserService _us = new UserService();

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
            var gid = 0;
            if (gzh != null) { gid = gzh.F_Id; }

            var list = _us.GetUserByGzhList(gid, pageIndex, pageSize, out totalPage, out totalRecord);

            VM_Page<Tab_User> vm = new VM_Page<Tab_User>();
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
            var id = Request.QueryString["id"];

            int uid = 0;
            if (id == null || !int.TryParse(id, out uid) || uid == 0)
            {
                return View();
            }

            var user = _us.GetUser(uid);
            if (user != null)
            {
                ViewBag.user = user;
            }

            return View();
        }

        [CustomAuthorize]
        [CustomAjaxLogin]
        public ActionResult Edit()
        {
            var a = Request.Form["pwd"];
            var b = Request.Form["id"];

            var pwd = 0;
            var uid = 0;
            if (!int.TryParse(a, out pwd) || pwd == 0)
            {
                return Json(new DWZJson { statusCode = (int)DWZStatusCode.ERROR, message = "什么都没做!" });
            }

            if (!int.TryParse(b, out uid) || uid == 0)
            {
                return Json(new DWZJson { statusCode = (int)DWZStatusCode.ERROR, message = "账号不存在" });
            }

            int i = _us.UpdateUser(uid);

            if (i == 1)
            {
                return Json(new DWZJson { statusCode = (int)DWZStatusCode.OK, message = "成功" });
            }
            else
            {
                return Json(new DWZJson { statusCode = (int)DWZStatusCode.ERROR, message = "失败" });
            }
        }

        [CustomAuthorize]
        [CustomAjaxLogin]
        public ActionResult AddView()
        {
            return View();
        }

        [CustomAuthorize]
        [CustomAjaxLogin]
        public ActionResult Add()
        {
            var name = Request.Form["name"];
            var id = Request.Form["rid"];


            if (name == null || name.Length > 50)
            {
                return Json(new DWZJson() { statusCode = (int)DWZStatusCode.ERROR, message = "用户名必须小于50个字符" });
            }

            int rid = 0;
            if (id == null || !int.TryParse(id, out rid) || !new int[] { 2, 3, 4 }.Contains(rid))
            {
                return Json(new DWZJson() { statusCode = (int)DWZStatusCode.ERROR, message = "章节ID无效" });
            }

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
            var gid = 0;
            if (gzh != null)
            {
                gid = gzh.F_Id;
            }
            else
            {
                return Json(new DWZJson() { statusCode = (int)DWZStatusCode.ERROR, message = "你没有关联公众号不能添加账号" });
            }

            Tab_User uu = new Tab_User();
            uu.F_Name = name;
            uu.F_Password = "123456";
            uu.GZHId = gid;
            uu.RoleId = rid;

            var i = _us.AddUser(uu);

            if (i == 1)
            {
                return Json(new DWZJson { statusCode = (int)DWZStatusCode.OK, message = "成功" });
            }
            else if (i == 2)
            {
                return Json(new DWZJson { statusCode = (int)DWZStatusCode.ERROR, message = "登录名已存在" });
            }
            else
            {
                return Json(new DWZJson { statusCode = (int)DWZStatusCode.ERROR, message = "失败" });
            }
        }

        [CustomAuthorize]
        [CustomAjaxLogin]
        public ActionResult Delete()
        {
            var a = Request.QueryString["id"];

            var uid = 0;
            if (!int.TryParse(a, out uid) || uid == 0)
            {
                return Json(new DWZJson() { statusCode = (int)DWZStatusCode.ERROR, message = "用户不存在" });
            }

            int i = _us.DeleteUser(uid);

            if (i == 1)
            {
                return Json(new DWZJson { statusCode = (int)DWZStatusCode.OK, message = "成功" });
            }
            else
            {
                return Json(new DWZJson { statusCode = (int)DWZStatusCode.ERROR, message = "失败" });
            }
        }
    }
}