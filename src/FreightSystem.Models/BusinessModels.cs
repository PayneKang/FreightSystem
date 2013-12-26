using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FreightSystem.Models
{
    public class TransportRecordListModel
    {
        public const int PageSize = 50;
        public string ClientName { get; set; }
        public DateTime? DeliverDate { get; set; }
        public string Received { get; set; }
        public string Paid { get; set; }
        public string Error { get; set; }
        public int PageIndex { get; set; }
        public int TotalCount { get; set; }
        public int TotalPage { get; set; }
        public List<TransportRecordModel> ItemList { get; set; }
    }

    public class TransportRecordModel
    {
        public int ID { get; set; }
        [Required(ErrorMessage="车牌号必须填写")]
        public string CarLicense { get; set; }
        [Required(ErrorMessage = "客户必须填写")]
        public string ClientName { get; set; }
        [Required(ErrorMessage = "日期必须填写")]
        [Display(Name="发货日期")]
        public DateTime DeliverDate { get; set; }
        [Required(ErrorMessage = "驾驶员必须填写")]
        public string Driver { get; set; }
        [Required(ErrorMessage = "起运地必须填写")]
        public string FromLocation { get; set; }
        [Required(ErrorMessage = "货物名称必须填写")]
        public string PackageName { get; set; }
        [Required(ErrorMessage = "数量必须填写")]
        [Display(Name="数量")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "卸货地必须填写")]
        public string ToLocation { get; set; }
        [Required(ErrorMessage = "体积必须填写")]
        [Display(Name="体积")]
        public double Volume { get; set; }
        public double? AccountPayble { get; set; }
        public string Comment { get; set; }
        public double? Deductions { get; set; }
        public double? DeliverPrice { get; set; }
        public string DeliverType { get; set; }
        public double? HandlingFee { get; set; }
        public DateTime? PayDate { get; set; }
        public double? PrePay { get; set; }
        public double? OilCard { get; set; }
        public double? Reparations { get; set; }
        public double? ShortBargeFee { get; set; }
        public string Status { get; set; }
        [Required(ErrorMessage = "托盘号必须填写")]
        public string TrayNo { get; set; }
        public string BusinessArea { get; set; }
        public bool Error { get; set; }
        public bool Received { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public bool Paid { get; set; }
        public double ShouldPayPrice { get {
            return 0f;
        } }
        public List<TransportRecordsHistoryModel> HistoryItem { get; set; }
    }

    public class TransportRecordsHistoryModel
    {
        public string UserID { get; set; }
        public UserModel User { get; set; }
        public int TransportRecordID { get; set; }
        public DateTime LogDateTime { get; set; }
        public string Operation { get; set; }
        public string Description { get; set; }
    }

    public class BusinessAreaModel
    {
        public int ID { get; set; }
        public string AreaName { get; set; }
    }

    public class MonthlyReportModel
    {
        [Required(ErrorMessage="年月必须设定")]
        public string YearMonth { get; set; }
        public List<TransportRecordModel> ItemList { get; set; }
    }
}