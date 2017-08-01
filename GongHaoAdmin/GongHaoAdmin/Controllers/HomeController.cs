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
    public class HomeController : Controller
    {
        private UserService _us = new UserService();
        private MenuService _ms = new MenuService();

        /// <summary>
        /// 首页
        /// </summary>


        [CustomLogin]
        public ActionResult Index()
        {
            HttpCookie authCookie = Request.Cookies["a"]; // 获取cookie
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value); // 解密
            var user = SerializeHelper.FromJson<Tab_User>(ticket.UserData);

            var list = _ms.GetMenuList(user.F_Id);

            var catalog = _ms.GetMenuCatalog();

            var newlist = list.GroupBy(i => i.F_ParentId).ToList();

            ViewBag.ls = list;

            ViewBag.ca = catalog;

            ViewBag.nl = newlist;

            ViewBag.name = user.F_Name;

            return View();
        }

        /// <summary>
        /// 登录
        /// </summary>
        public ActionResult Login()
        {
            ViewBag.msg = "";

            HttpCookie authCookie = Request.Cookies["a"]; // 获取cookie
            if (authCookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value); // 解密
                var user = SerializeHelper.FromJson<Tab_User>(ticket.UserData);
                var u = new UserService().GetUser(user.F_Name, user.F_Password);
                if (u != null)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            var username = Request.Form["username"];
            var password = Request.Form["password"];

            if (username != null && password != null)
            {
                var user = _us.GetUser(username, password);
                if (user != null)
                {
                    var now = DateTime.Now;

                    user.F_CreateDate = DateTime.Now;
                    string UserData = SerializeHelper.ToJson<Tab_User>(user); // 序列化用户实体

                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, "user", now, now, true, UserData);
                    HttpCookie cookie = new HttpCookie("a", FormsAuthentication.Encrypt(ticket).ToLower()); // 加密身份信息，保存至Cookie
                    cookie.HttpOnly = true;
                    Response.Cookies.Add(cookie);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.msg = "用户名或密码不正确";
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// 登出
        /// </summary>
        public ActionResult SignOut()
        {
            HttpCookie cookie = new HttpCookie("a");
            cookie.Expires = DateTime.Now.AddDays(-1);
            Response.SetCookie(cookie);

            return RedirectToAction("Login", "Home");
        }

        [CustomAjaxLogin]
        public ActionResult Password()
        {
            return View();
        }

        [CustomAjaxLogin]
        public ActionResult PasswordUpdate()
        {
            HttpCookie authCookie = Request.Cookies["a"]; // 获取cookie
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value); // 解密
            var user = SerializeHelper.FromJson<Tab_User>(ticket.UserData);

            //var nvc = Request.Form;
            //string[] keys = nvc.AllKeys;
            //var a = keys.Contains("pwd1");
            //var b = keys.Contains("pwd2");
            //var c = keys.Contains("pwd3");

            var pwd1 = Request.Form["pwd1"]; // 旧密码
            var pwd2 = Request.Form["pwd2"]; // 新密码
            var pwd3 = Request.Form["pwd3"]; // 确认密码

            if (pwd1 == null || pwd1.Length < 6 || pwd1.Length > 20)
            {
                return Json(new DWZJson() { statusCode = (int)DWZStatusCode.ERROR, message = "旧密码长度必须大于6个字符小于20字符" });
            }

            if (pwd2 == null || pwd2.Length < 6 || pwd2.Length > 20)
            {
                return Json(new DWZJson() { statusCode = (int)DWZStatusCode.ERROR, message = "新密码长度必须大于6个字符小于20字符" });
            }

            if (pwd3 == null || pwd3.Length < 6 || pwd3.Length > 20)
            {
                return Json(new DWZJson() { statusCode = (int)DWZStatusCode.ERROR, message = "新密码长度必须大于6个字符小于20字符" });
            }

            if (pwd2 != pwd3)
            {
                return Json(new DWZJson() { statusCode = (int)DWZStatusCode.ERROR, message = "密码不一致" });
            }

            if (pwd1 == pwd2)
            {
                return Json(new DWZJson() { statusCode = (int)DWZStatusCode.ERROR, message = "新旧密码不能相同" });
            }

            var i = _us.UpdatePassword(user.F_Id, pwd1, pwd2);

            if (i == 1)
            {
                var now = DateTime.Now;
                user.F_CreateDate = DateTime.Now;
                user.F_Password = pwd2;
                string UserData = SerializeHelper.ToJson<Tab_User>(user); // 序列化用户实体
                ticket = new FormsAuthenticationTicket(1, "user", now, now, true, UserData);
                HttpCookie cookie = new HttpCookie("a", FormsAuthentication.Encrypt(ticket).ToLower()); // 加密身份信息，保存至Cookie
                cookie.HttpOnly = true;

                Response.SetCookie(cookie);

                return Json(new DWZJson() { statusCode = (int)DWZStatusCode.OK, message = "操作成功" });
            }
            else
            {
                return Json(new DWZJson() { statusCode = (int)DWZStatusCode.ERROR, message = "旧密码不正确" });
            }
        }
    }
}