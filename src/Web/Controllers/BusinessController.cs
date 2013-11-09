using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Filters;
using FreightSystem.Models;

namespace Web.Controllers
{
    public class BusinessController : Controller
    {
        [LoggedIn]
        public ActionResult Index(int pageIndex = 1)
        {
            TransportRecordListModel model = new TransportRecordListModel();
            return View(model);
        }

    }
}
