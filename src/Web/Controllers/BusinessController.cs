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
            string received = TryGetRequiredString("received");
            string paid = TryGetRequiredString("paid");
            string error = TryGetRequiredString("error");
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
            TransportRecordListModel model = businessProvider.QueryDailyTransportModel(clientName, dtDeliverDate,received,paid,error);
            return View(model);
        }

        [LoggedIn(CheckAccess: true, AccessCode: "EXPORT")]
        public ActionResult ExcelLocalReport()
        {
            string clientName = TryGetRequiredString("ClientName");
            string deliverDate = TryGetRequiredString("DeliverDate");
            string received = TryGetRequiredString("received");
            string paid = TryGetRequiredString("paid");
            string error = TryGetRequiredString("error");
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
            TransportRecordListModel model = businessProvider.QueryDailyTransportModel(clientName, dtDeliverDate,received,paid,error);
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
            model = businessProvider.QueryTransportModel(string.Empty, "ALL", "ALL", "ALL", fromDate, fromDate.AddMonths(1).AddDays(-1));
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
            ViewBag.Clients = CreateClientListForQuery();
            model = businessProvider.QueryTransportModel(model.ClientName, model.DeliverDate,model.Received,model.Paid,model.Error, page, ((UserModel)ViewBag.LoggedUser).Area);
            return View(model);
        }

        [LoggedIn(CheckAccess: true, AccessCode: "TOTALRDLIST")]
        public ActionResult Index(TransportRecordListModel model, int page = 1)
        {
            ViewBag.Clients = CreateClientListForQuery(); 
            model = businessProvider.QueryTransportModel(model.ClientName, model.DeliverDate, model.Received, model.Paid, model.Error, page);
            return View(model);
        }

        [LoggedIn(CheckAccess: true, AccessCode: "NEWRD")]
        [HttpGet]
        public ActionResult NewTransportRecord()
        {
            ViewBag.Clients = (from x in businessProvider.QueryClient()
                              select new SelectListItem()
                              {
                                  Text = x.ClientName,
                                  Value = x.ClientName
                              }).ToList();
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
            businessProvider.UpdateTransportModel(model.ID, model.TrayNo, model.Volume, model.Quantity,model.DeliverPrice, model.ShortBargeFee, this.cacheProvider.GetCurrentLoggedUser().UserID);
            return View(model);
        }

        [LoggedIn(CheckAccess: true, AccessCode: "UPDTPAID")]
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
        [LoggedIn(CheckAccess: true, AccessCode: "UPDTPAID")]
        [HttpPost]
        public ActionResult FillCa(TransportRecordModel model)
        {
            if (model.Paid && (model.Error || !model.Received))
            {
                ViewBag.ErrorMessage = "错误：必须已到货并且无错误的单据才能进行结算操作";
                model.Paid = false;
                return View(model);
            }
            businessProvider.UpdateTransportPaymentData(model.ID, model.PayDate, model.AccountPayble, model.Deductions, model.Reparations, model.Paid, this.cacheProvider.GetCurrentLoggedUser().UserID);
            return View(model);
        }

        [LoggedIn(CheckAccess: true, AccessCode: "PRINT")]
        [HttpGet]
        public ActionResult PrintTR(int id)
        {
            TransportRecordModel model = businessProvider.GetTransportRecordModel(id);
            return View(model);
        }
        
        [LoggedIn(CheckAccess: true, AccessCode: "UPDTERR")]
        [HttpGet]
        public ActionResult UpdateErr(int id)
        {
            string returnUrl = Request["returnUrl"];
            string error = Request["error"];
            bool berr = true;
            bool.TryParse(error, out berr);
            UserModel user = this.cacheProvider.GetCurrentLoggedUser();
            businessProvider.UpdateTransportErrorStatus(id, berr, user.UserID);

            if (user.Role.AccessList.Contains("TOTALRDLIST"))
                return RedirectToAction("Index", "Business");
            if (user.Role.AccessList.Contains("LOCALRDLIST"))
                return RedirectToAction("LocalTransportRecordList", "Business");
            return RedirectToLocal(returnUrl);
        }

        [LoggedIn(CheckAccess: true, AccessCode: "UPDTREV")]
        [HttpGet]
        public ActionResult UpdateReceived(int id)
        {
            TransportRecordModel model = this.businessProvider.GetTransportRecordModel(id);
            return View(model);
        }
        [LoggedIn(CheckAccess: true, AccessCode: "UPDTREV")]
        [HttpPost]
        public ActionResult UpdateReceived(TransportRecordModel model)
        {
            string returnUrl = Request["returnUrl"];
            string received = Request["received"];
            bool breceived = true;
            bool.TryParse(received, out breceived);
            UserModel user = this.cacheProvider.GetCurrentLoggedUser();
            businessProvider.UpdateTransportReceivedStatus(model.ID, model.Received, model.ReceivedDate, user.UserID);

            ViewBag.Message = "更新到货状态成功";
            return View(model);
        }
        [LoggedIn(CheckAccess: true, AccessCode: "CLTMGR")]
        [HttpGet]
        public ActionResult ClientList()
        {
            List<ClientModel> clients = businessProvider.QueryClient();
            return View(clients);
        }
        [LoggedIn(CheckAccess: true, AccessCode: "CLTMGR")]
        [HttpPost]
        public ActionResult ClientList(string clientname,string shortname)
        {
            string clientName = Request["clientname"];
            string shortName = Request["shortname"];
            bool foundErr = false;
            if(string.IsNullOrEmpty(clientName)){
                ViewBag.ErrorMessage += "客户名为空\r\n";
                foundErr = true;
            }
            if (string.IsNullOrEmpty(shortName))
            {
                ViewBag.ErrorMessage += "客户简写为空\r\n";
                foundErr = true;
            }
            try
            {
                if (!foundErr)
                {
                    ClientModel newclient = new ClientModel()
                    {
                        ClientName = clientName,
                        ShortName = shortName
                    };
                    businessProvider.InsertClient(newclient);
                    return RedirectToAction("ClientList", "Business");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage += ex.Message;
            }
            
            List<ClientModel> clients = businessProvider.QueryClient();
            return View(clients);
        }

        [LoggedIn(CheckAccess: true, AccessCode: "CLTMGR")]
        [HttpGet]
        public ActionResult ClientMgr(int id)
        {
            ClientModel client = businessProvider.GetClient(id);
            return View(client);
        }

        [LoggedIn(CheckAccess: true, AccessCode: "CLTMGR")]
        [HttpPost]
        public ActionResult ClientMgr(ClientModel client)
        {
            try
            {
                businessProvider.UpdateClientInformation(client);
                ViewBag.Message = "编辑成功";
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }
            return View(client);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Business");
            }
        }
        private List<SelectListItem> CreateClientListForQuery()
        {
            List<SelectListItem> clients = new List<SelectListItem>() { 
                new SelectListItem(){
                     Selected = true,
                      Text = "所有",
                       Value = ""
                }
            };
            clients.AddRange((from x in businessProvider.QueryClient()
                              select new SelectListItem()
                              {
                                  Text = x.ClientName,
                                  Value = x.ClientName
                              }).ToList());
            return clients;
        }
    }
}
