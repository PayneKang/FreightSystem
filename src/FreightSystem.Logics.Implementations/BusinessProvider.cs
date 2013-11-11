using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreightSystem.Logics.Interfaces;
using DAO_Access;
using FreightSystem.Models;
using System.Data;
using System.Data.OleDb;

namespace FreightSystem.Logics.Implementations
{
    public class BusinessProvider : BaseProvider,IBusinessProvider
    {
        public const string CountTransportListSql = @"select count(*) from [TransportRecords]";
        public const string QueryTransportListSql = @"select * from [TransportRecords] order by id desc";
        public const string InsertTransportRecordSql = @"insert into [TransportRecords] (CarLicense, ClientName, DeliverDate, DeliverNumber, Driver, FromLocation, PackageName, Quantity, ToLocation, Volumn, AccountPayble, Comment, CreatorUserID, Deductions, DeliverPrice, DeliverType, HandlingFee, PayDate, PrePay, Reparations, ShortBargeFee, Status) Values(@CarLicense, @ClientName, @DeliverDate, @DeliverNumber, @Driver, @FromLocation, @PackageName, @Quantity, @ToLocation, @Volumn, @AccountPayble, @Comment, @CreatorUserID, @Deduction, @DeliverPrice, @DeliverType, @HandlingFee, @PayDate, @PrePay, @Reparations, @ShortBargeFee, @Status)";

        public TransportRecordListModel QueryTransportModel(int pageIndex)
        {
            TransportRecordListModel listModel = new TransportRecordListModel();
            if (pageIndex < 1)
                pageIndex = 1;
            int totalCount = (int)DbHelper.ExecuteScalar(CountTransportListSql, null);

            listModel.TotalCount = totalCount;

            listModel.TotalPage = totalCount / TransportRecordListModel.PageSize;

            if (totalCount % TransportRecordListModel.PageSize > 0)
                listModel.TotalPage++;

            DataSet ds = DbHelper.ExecuteSql2DataSet(QueryTransportListSql, null, (pageIndex - 1) * TransportRecordListModel.PageSize, TransportRecordListModel.PageSize, "TransportRecords");

            listModel.ItemList = new List<TransportRecordModel>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                listModel.ItemList.Add(
                    new TransportRecordModel()
                    {
                        AccountPayble = (decimal)row["accountpayble"],
                        CarLicense = row["carlicense"].ToString(),
                        ClientName = row["ClientName"].ToString(),
                        Comment = row["ClientName"].ToString(),
                        CreatorUserID = row["CreatorUserID"].ToString(),
                        Deductions = (decimal)row["CreatorUserID"],
                        DeliverDate = (DateTime)row["DeliverDate"],
                        DeliverNumber = row["DeliverNumber"].ToString(),
                        DeliverPrice = (decimal)row["DeliverPrice"],
                        DeliverType = row["DeliverType"].ToString(),
                        Driver = row["Driver"].ToString(),
                        FromLocation = row["FromLocation"].ToString(),
                        HandlingFee = (decimal)row["HandlingFee"],
                        PackageName = row["PackageName"].ToString(),
                        PayDate = (DateTime)row["PayDate"],
                        PrePay = (decimal)row["PrePay"],
                        Quantity = (int)row["Quantity"],
                        Reparations = (decimal)row["Reparations"],
                        ShortBargeFee = (decimal)row["ShortBargeFee"],
                        Status = row["Status"].ToString(),
                        ToLocation = row["ToLocation"].ToString(),
                        Volume = (decimal)row["Volume"]
                    }
                    );
            }

            return listModel;
        }


        public void InsertTransprotModel(TransportRecordModel model)
        {
            OleDbParameter[] insertParameters = new OleDbParameter[] { 
                //@, @, @, @, @, @, @, @, @, @, @, @, @, @, @, @, @, @, @, @, @, @
                new OleDbParameter("CarLicense",model.CarLicense),
                new OleDbParameter("ClientName",model.ClientName),
                new OleDbParameter("DeliverDate",model.DeliverDate),
                new OleDbParameter("DeliverNumber",model.DeliverNumber),
                new OleDbParameter("Driver",model.Driver),
                new OleDbParameter("FromLocation",model.FromLocation),
                new OleDbParameter("PackageName",model.PackageName),
                new OleDbParameter("Quantity",model.Quantity),
                new OleDbParameter("ToLocation",model.ToLocation),
                new OleDbParameter("Volumn",model.Volume),
                new OleDbParameter("AccountPayble",model.AccountPayble),
                new OleDbParameter("Comment",model.Comment),
                new OleDbParameter("CreatorUserID",model.CreatorUserID),
                new OleDbParameter("Deduction",model.Deductions),
                new OleDbParameter("DeliverPrice",model.DeliverPrice),
                new OleDbParameter("DeliverType",model.DeliverType),
                new OleDbParameter("HandlingFee",model.HandlingFee),
                new OleDbParameter("PayDate",model.PayDate),
                new OleDbParameter("PrePay",model.PrePay),
                new OleDbParameter("Reparations",model.Reparations),
                new OleDbParameter("ShortBargeFee",model.ShortBargeFee),
                new OleDbParameter("Status",model.Status)
            };
            DbHelper.ExecuteNonQuery(InsertTransportRecordSql, insertParameters);
        }
    }
}
