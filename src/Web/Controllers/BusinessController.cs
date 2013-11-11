using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Filters;
using FreightSystem.Models;
using FreightSystem.Logics.Interfaces;
using FreightSystem.Logics.Implementations;

namespace Web.Controllers
{
    public class BusinessController : Controller
    {
        IBusinessProvider businessProvider;

        public BusinessController()
        {
            this.businessProvider = new BusinessProvider();
        }

        [LoggedIn]
        public ActionResult Index(int pageIndex = 1)
        {
            TransportRecordListModel model = new TransportRecordListModel();
            return View(model);
        }

        [LoggedIn]
        [HttpGet]
        public ActionResult NewTransportRecord()
        {
            return View();
        }

        [LoggedIn]
        [HttpPost]
        public ActionResult NewTransportRecord(TransportRecordModel model)
        {
            businessProvider.InsertTransprotModel(model);
            return View(model);
        }

    }
}
