﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Filters;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        [LoggedIn]
        public ActionResult Index()
        {
            return View();
        }

    }
}
