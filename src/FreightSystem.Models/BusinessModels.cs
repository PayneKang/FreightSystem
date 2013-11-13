using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FreightSystem.Models
{
    public class TransportRecordListModel
    {
        public const int PageSize = 20;
        public int PageIndex { get; set; }
        public int TotalCount { get; set; }
        public int TotalPage { get; set; }
        public List<TransportRecordModel> ItemList { get; set; }
    }

    public class TransportRecordModel
    {

        public string CarLicense { get; set; }
        public string ClientName { get; set; }
        public string Consignee { get; set; }
        public DateTime? DeliverDate { get; set; }
        public string DeliverNumber { get; set; }
        public string Driver { get; set; }
        public string FromLocation { get; set; }
        public string PackageName { get; set; }
        public int? Quantity { get; set; }
        public string ToLocation { get; set; }
        public double? Volume { get; set; }
        public double? AccountPayble { get; set; }
        public string Comment { get; set; }
        public string CreatorUserID { get; set; }
        public double? Deductions { get; set; }
        public double? DeliverPrice { get; set; }
        public string DeliverType { get; set; }
        public double? HandlingFee { get; set; }
        public DateTime? PayDate { get; set; }
        public double? PrePay { get; set; }
        public double? Reparations { get; set; }
        public double? ShortBargeFee { get; set; }
        public string Status { get; set; }
    }
}