using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ML_NET40_CF.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/

        public ActionResult Index()
        {
            var data = new Models.UnitOfWork(new Models.Main.MainDBC());
            return View(data.Main.Select_News());
        }

    }
}
