using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppTaquilla.Controllers
{
    public class ErrorNotFoundController : Controller
    {
        // GET: ErrorNotFound
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PageNotFound()
        {
            return View();
        }

    }
}