using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FreightSystem.Models
{
    public class TransportRecordModel
    {

        public string CarLicense { get; set; }
        public string ClientName { get; set; }
        public DateTime DeliverDate { get; set; }
        public string DeliverNumber { get; set; }
        public string Driver { get; set; }
        public int FromLocation { get; set; }
        public string PackageName { get; set; }
        public int Quantity { get; set; }
        public string ToLocation { get; set; }
        public decimal Volume { get; set; }
        public decimal AccountPayble { get; set; }
        public string Additional_1 { get; set; }
        public string Additional_2 { get; set; }
        public string Additional_3 { get; set; }
        public string Additional_4 { get; set; }
        public string Additional_5 { get; set; }
        public string Comment { get; set; }
        public int CreatorUserID { get; set; }
        public decimal Deductions { get; set; }
        public decimal DeliverPrice { get; set; }
        /// <summary>
        /// 专线或者直运
        /// </summary>
        public string DeliverType { get; set; }
        public decimal HandlingFee { get; set; }
        public DateTime PayDate { get; set; }
        public decimal PrePay { get; set; }
        public decimal Reparations { get; set; }
        public decimal ShortBargeFee { get; set; }
        public string Status { get; set; }
    }
}