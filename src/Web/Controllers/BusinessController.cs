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
        IUserCacheProvider cacheProvider;

        public BusinessController()
        {
            this.businessProvider = new BusinessProvider();
            this.cacheProvider = new UserCacheProvider();
        }

        [LoggedIn]
        public ActionResult Index(int page = 1)
        {
            TransportRecordListModel model = businessProvider.QueryTransportModel(page);
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
            model.CreatorUserID = this.cacheProvider.GetCurrentLoggedUser().UserID;
            try
            {
                businessProvider.InsertTransprotModel(model);
                return RedirectToAction("Index", "Business");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }
            return View(model);
        }

    }
}
