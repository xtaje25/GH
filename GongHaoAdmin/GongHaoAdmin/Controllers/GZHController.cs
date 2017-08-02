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
    public class GZHController : Controller
    {
        private GongZhongHaoService _gzhs = new GongZhongHaoService();
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

            var gzh = _gzhs.GetGZH(u.F_Id);

            ViewBag.gzh = gzh;
            ViewBag.msg = gzh == null ? "账号没有关联公众号" : "";

            return View();
        }

        [CustomAuthorize]
        [CustomAjaxLogin]
        public ActionResult Edit()
        {
            var name = Request.Form["name"];
            var about = Request.Form["about"];

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

            if (name != null && name.Length > 200)
            {
                return View(new DWZJson() { statusCode = (int)DWZStatusCode.ERROR, message = "公号名称长度必须小于200字符" });
            }

            if (about != null && about.Length > 4000)
            {
                return View(new DWZJson() { statusCode = (int)DWZStatusCode.ERROR, message = "公号简介长度必须小于4000字符" });
            }

            var logo = "";
            if (Request.Files.Count > 0
               && Request.Files[0].ContentLength > 0
               && new string[] { ".gif", ".jpeg", ".jpg", ".png" }.Contains(System.IO.Path.GetExtension(Request.Files[0].FileName.ToLower())))
            {
                var key = QN.GZHLogo(gid);

                FormUploader fu = new FormUploader();
                HttpResult result = fu.UploadStream(Request.Files[0].InputStream, key, QN.GetUploadToken(QN.BUCKET, key));
                if (result.Code == 200)
                {
                    logo = QN.IMGSRC + "/" + key;
                }
            }

            Tab_GongZhongHao g = new Tab_GongZhongHao();
            g.F_About = (about != null && about.Length > 0) ? about : null;
            g.F_Logo = logo != "" ? logo : null;
            g.F_Id = gid;

            var i = _gzhs.UpdateGZHInfo(g);

            if (i == 1)
            {
                return View(new DWZJson() { statusCode = (int)DWZStatusCode.OK, message = "成功" });
            }
            else
            {
                return View(new DWZJson() { statusCode = (int)DWZStatusCode.ERROR, message = "失败" });
            }
        }
    }
}