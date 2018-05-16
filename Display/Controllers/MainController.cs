using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Display.Controllers
{
    public class MainController : Controller
    {
        [Route("main")]
        public ActionResult Index(int id = 0)
        {
            ViewBag.ID = id;
            return View();
        }
    }
}