using GongHaoAdmin.Models;
using GongHaoAdmin.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace GongHaoAdmin.Service
{
    /// <summary>
    /// 验证登录
    /// </summary>
    public class CustomLoginAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// 重写时，提供一个入口点用于进行自定义授权检查。
        /// </summary>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            HttpCookie authCookie = httpContext.Request.Cookies["a"]; // 获取cookie
            if (authCookie != null)
            {
                try
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value); // 解密
                    var user = SerializeHelper.FromJson<Tab_User>(ticket.UserData);
                    var u = new UserService().GetUser(user.F_Name, user.F_Password);
                    if (u != null)
                        return true;
                    else
                    {
                        HttpCookie cookie = new HttpCookie("a");
                        cookie.Expires = DateTime.Now.AddDays(-1);
                        httpContext.Response.SetCookie(cookie);
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 处理未能授权的 HTTP 请求。
        /// </summary>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("/Home/Login");
        }
    }

    /// <summary>
    /// 验证登录
    /// </summary>
    public class CustomAjaxLoginAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// 重写时，提供一个入口点用于进行自定义授权检查。
        /// </summary>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            HttpCookie authCookie = httpContext.Request.Cookies["a"]; // 获取cookie
            if (authCookie != null)
            {
                try
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value); // 解密
                    var user = SerializeHelper.FromJson<Tab_User>(ticket.UserData);
                    var u = new UserService().GetUser(user.F_Name, user.F_Password);
                    if (u != null)
                        return true;
                    else
                    {
                        HttpCookie cookie = new HttpCookie("a");
                        cookie.Expires = DateTime.Now.AddDays(-1);
                        httpContext.Response.SetCookie(cookie);
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 处理未能授权的 HTTP 请求。
        /// </summary>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("/Home/AjaxLogin");
        }
    }

    /// <summary>
    /// 验证权限
    /// </summary>
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// 重写时，提供一个入口点用于进行自定义授权检查。
        /// </summary>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            HttpCookie authCookie = httpContext.Request.Cookies["a"]; // 获取cookie
            if (authCookie != null)
            {
                try
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value); // 解密
                    var user = SerializeHelper.FromJson<Tab_User>(ticket.UserData);
                    var u = new UserService().GetUser(user.F_Name, user.F_Password);
                    if (u != null)
                    {
                        var list = new MenuService().GetUrlList(u.F_Id);

                        var menu = list.Find(m => httpContext.Request.Url.AbsolutePath.StartsWith("/" + m.F_URL));
                        if (menu != null)
                            return true;
                        else
                            return false;
                    }
                    else
                    {
                        HttpCookie cookie = new HttpCookie("a");
                        cookie.Expires = DateTime.Now.AddDays(-1);
                        httpContext.Response.SetCookie(cookie);
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 处理未能授权的 HTTP 请求。
        /// </summary>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("/Home/AjaxLogin");
        }
    }
}