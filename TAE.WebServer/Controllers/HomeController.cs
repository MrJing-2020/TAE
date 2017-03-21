using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TAE.WebServer.Controllers
{
    using TAE.Data.Model;
    using TAE.WebServer.Common;

    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            return View();
        }
        public ActionResult Test()
        {
            var test = ServiceBase.FindBy<Test>().ToList();
            return View(test);
        }
    }
}
