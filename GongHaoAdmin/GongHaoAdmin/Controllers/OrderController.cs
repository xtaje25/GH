using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GongHaoAdmin.Controllers
{
    public class OrderController : Controller
    {
        public ActionResult CancelView()
        {
            return View();
        }

        public ActionResult PayView()
        {
            return View();
        }
    }
}