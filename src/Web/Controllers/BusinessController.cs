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
        public ActionResult Export()
        {
            string clientName = RouteData.GetRequiredString("ClientName");
            string deliverDate = RouteData.GetRequiredString("DeliverDate");
            string id = Request["id"];
            return View();
        }

        private string TryGetRequiredString(string parameterName)
        {
            return string.Empty;
        }

        [LoggedIn]
        public ActionResult Index(TransportRecordListModel model, int page = 1)
        {
            model = businessProvider.QueryTransportModel(model.ClientName, model.DeliverDate, page);
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
