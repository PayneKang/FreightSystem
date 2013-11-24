using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Filters;
using FreightSystem.Models;
using FreightSystem.Logics.Interfaces;
using FreightSystem.Logics.Implementations;
using System.Text;

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
            string clientName = TryGetRequiredString("ClientName");
            string deliverDate = TryGetRequiredString("DeliverDate");
            DateTime dtDeliverDate;
            if (!DateTime.TryParse(deliverDate, out dtDeliverDate))
            {
                ViewBag.ErrorMessage = "必须选中一个日期才能导出日报表";
                return View("Error");
            }
            Response.ContentType = "application/vnd.ms-excel";
            Response.Charset = "GBK";
            Response.ContentEncoding = Encoding.UTF8;
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + deliverDate + "_" + clientName + ".xls");
            if (clientName == "NA")
                clientName = string.Empty;
            TransportRecordListModel model = businessProvider.QueryDailyTransportModel(clientName, dtDeliverDate);
            return View(model);
        }

        private string TryGetRequiredString(string parameterName)
        {
            if(!RouteData.Values.Keys.Contains(parameterName))
                return string.Empty;
            return RouteData.GetRequiredString(parameterName);
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
            model.HistoryItem = new List<TransportRecordsHistoryModel>();
            model.HistoryItem.Add(new TransportRecordsHistoryModel()
            {
                Description = "创建动态单",
                Operation = "创建",
                UserID = this.cacheProvider.GetCurrentLoggedUser().UserID
            });
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
