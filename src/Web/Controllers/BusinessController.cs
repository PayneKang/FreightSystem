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

        [LoggedIn(CheckAccess: true, AccessCode: "AREAMGR")]
        [HttpGet]
        public ActionResult AreaList()
        {
            List<BusinessAreaModel> areas = businessProvider.ListAllArea();
            return View(areas);
        }

        [LoggedIn(CheckAccess: true, AccessCode: "AREAMGR")]
        [HttpPost]
        public ActionResult AreaList(string newArea)
        {
            // Create new area
            string newarea = Request["newarea"];
            List<BusinessAreaModel> areas;
            if (string.IsNullOrEmpty(newarea))
            {
                ViewBag.ErrorMessage = "请输入正确的地点";

                // List all area
                areas = businessProvider.ListAllArea();
                return View(areas);
            }

            try{
                businessProvider.InsertNewArea(newarea);
            }catch(Exception ex){
                ViewBag.ErrorMessage = ex.Message;

                // List all area
                areas = businessProvider.ListAllArea();
                return View(areas);
            }

            // List all area
            areas = businessProvider.ListAllArea();
            return View(areas);
        }

        [LoggedIn(CheckAccess: true, AccessCode: "EXPORT")]
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

        [LoggedIn(CheckAccess: true, AccessCode: "EXPORT")]
        public ActionResult ExcelLocalReport()
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

        [LoggedIn(CheckAccess: true, AccessCode: "EXPORT")]
        [HttpGet]
        public ActionResult ExcelMonthlyReport()
        {
            return View();
        }

        [LoggedIn(CheckAccess: true, AccessCode: "EXPORT")]
        [HttpPost]
        public ActionResult ExcelMonthlyReport(MonthlyReportModel model)
        {
            DateTime fromDate;
            if (!DateTime.TryParse(model.YearMonth+"-01", out fromDate))
            {
                ViewBag.ErrorMessage = "必须选中一个月份才能导出月份报表";
                return View("Error");
            }
            Response.ContentType = "application/vnd.ms-excel";
            Response.Charset = "GBK";
            Response.ContentEncoding = Encoding.UTF8;
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + model.YearMonth + ".xls");
            model = businessProvider.QueryTransportModel(string.Empty, fromDate, fromDate.AddMonths(1).AddDays(-1));
            return View("ExportMonthlyReport", model);
        }

        private string TryGetRequiredString(string parameterName)
        {
            if(!RouteData.Values.Keys.Contains(parameterName))
                return string.Empty;
            return RouteData.GetRequiredString(parameterName);
        }

        [LoggedIn(CheckAccess: true, AccessCode: "LOCALRDLIST")]
        public ActionResult LocalTransportRecordList(TransportRecordListModel model, int page = 1)
        {
            model = businessProvider.QueryTransportModel(model.ClientName, model.DeliverDate, page, ((UserModel)ViewBag.LoggedUser).Area);
            return View(model);
        }

        [LoggedIn(CheckAccess: true, AccessCode: "TOTALRDLIST")]
        public ActionResult Index(TransportRecordListModel model, int page = 1)
        {
            model = businessProvider.QueryTransportModel(model.ClientName, model.DeliverDate, page);
            return View(model);
        }

        [LoggedIn(CheckAccess: true, AccessCode: "NEWRD")]
        [HttpGet]
        public ActionResult NewTransportRecord()
        {
            return View();
        }

        [LoggedIn(CheckAccess: true, AccessCode: "NEWRD")]
        [HttpPost]
        public ActionResult NewTransportRecord(TransportRecordModel model)
        {
            model.HistoryItem = new List<TransportRecordsHistoryModel>();
            model.BusinessArea = Request["businessarea"];
            model.HistoryItem.Add(new TransportRecordsHistoryModel()
            {
                Description = "创建动态单",
                Operation = "创建",
                UserID = this.cacheProvider.GetCurrentLoggedUser().UserID
            });
            try
            {
                businessProvider.InsertTransprotModel(model);
                UserModel currUser = ViewBag.LoggedUser;
                if (currUser.Role.AccessList.Contains("LOCALRDLIST"))
                    return RedirectToAction("LocalTransportRecordList", "Business");
                return RedirectToAction("Index", "Business");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }
            return View(model);
        }

        [LoggedIn(CheckAccess: true, AccessCode: "FILLRD")]
        [HttpGet]
        public ActionResult FillTransportRecord()
        {
            string sid = TryGetRequiredString("id");
            int id;
            if (!int.TryParse(sid, out id))
            {
                ViewBag.ErrorMessage = "请正确选择要编辑的纪录";
                return View("Error");
            }
            TransportRecordModel model = businessProvider.GetTransportRecordModel(id);
            return View(model);
        }
        [LoggedIn(CheckAccess: true, AccessCode: "FILLRD")]
        [HttpPost]
        public ActionResult FillTransportRecord(TransportRecordModel model)
        {
            businessProvider.UpdateTransportModel(model.ID, model.TrayNo, model.Volume, model.Quantity);
            return View(model);
        }

        [LoggedIn(CheckAccess: true, AccessCode: "FILLCA")]
        [HttpGet]
        public ActionResult FillCa()
        {
            string sid = TryGetRequiredString("id");
            int id;
            if (!int.TryParse(sid, out id))
            {
                ViewBag.ErrorMessage = "请正确选择要编辑的纪录";
                return View("Error");
            }
            TransportRecordModel model = businessProvider.GetTransportRecordModel(id);
            return View(model);
        }
        [LoggedIn(CheckAccess: true, AccessCode: "FILLCA")]
        [HttpPost]
        public ActionResult FillCa(TransportRecordModel model)
        {
            businessProvider.UpdateTransportPaymentData(model.ID, model.PayDate.Value, model.AccountPayble.Value);
            return View(model);
        }

    }
}
