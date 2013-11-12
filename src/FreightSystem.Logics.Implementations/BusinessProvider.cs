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
    public class BusinessProvider : IBusinessProvider
    {
        public TransportRecordListModel QueryTransportModel(int pageIndex)
        {
            TransportRecordListModel listModel = new TransportRecordListModel();
            if (pageIndex < 1)
                pageIndex = 1;
            using (SQLDBDataContext context = new SQLDBDataContext()) {

                int totalCount = context.TransportRecords.Count();
                listModel.TotalCount = totalCount;
                listModel.TotalPage = totalCount / TransportRecordListModel.PageSize;
                listModel.PageIndex = pageIndex;
                if (totalCount % TransportRecordListModel.PageSize > 0)
                    listModel.TotalPage++;
                if (listModel.TotalPage < 1)
                    listModel.TotalPage = 1;
                if (pageIndex > listModel.TotalPage)
                    pageIndex = listModel.TotalPage;
                int startIndex = (pageIndex - 1) * TransportRecordListModel.PageSize;
                listModel.ItemList = (from x in context.TransportRecords.OrderByDescending(x=>x.ID).Skip(startIndex).Take(TransportRecordListModel.PageSize)
                                      select new TransportRecordModel()
                                      {
                                          AccountPayble = x.AccountPayble,
                                          CarLicense = x.CarLicense,
                                          ClientName = x.ClientName,
                                          Comment = x.Comment,
                                          CreatorUserID = x.CreatorUserID,
                                          Deductions = x.Deductions,
                                          DeliverDate = x.DeliverDate,
                                          DeliverNumber = x.DeliverNumber,
                                          DeliverPrice = x.DeliverPrice,
                                          DeliverType = x.DeliverType,
                                          Driver = x.Driver,
                                          FromLocation = x.FromLocation,
                                          HandlingFee = x.HandlingFee,
                                          PackageName = x.PackageName,
                                          PayDate = x.PayDate,
                                          PrePay = x.PrePay,
                                          Quantity = x.Quantity,
                                          Reparations = x.Reparations,
                                          ShortBargeFee = x.ShortBargeFee,
                                          Status = x.Status,
                                          ToLocation = x.ToLocation,
                                          Volume = x.Volume
                                      }).ToList();
            }
            return listModel;
        }


        public void InsertTransprotModel(TransportRecordModel model)
        {
            using (SQLDBDataContext context = new SQLDBDataContext())
            {
                TransportRecord newRecord = new TransportRecord()
                {
                    AccountPayble = model.AccountPayble,
                    CarLicense = model.CarLicense,
                    ClientName = model.ClientName,
                    Comment = model.Comment,
                    CreatorUserID = model.CreatorUserID,
                    Deductions = model.Deductions,
                    DeliverDate = model.DeliverDate,
                    DeliverNumber = model.DeliverNumber,
                    DeliverPrice = model.DeliverPrice,
                    DeliverType = model.DeliverType,
                    Driver = model.Driver,
                    FromLocation = model.FromLocation,
                    HandlingFee = model.HandlingFee,
                    PackageName = model.PackageName,
                    PayDate = model.PayDate,
                    PrePay = model.PrePay,
                    Quantity = model.Quantity,
                    Reparations = model.Reparations,
                    ShortBargeFee = model.ShortBargeFee,
                    Status = model.Status,
                    ToLocation = model.ToLocation,
                    Volume = model.Volume
                };
                context.TransportRecords.InsertOnSubmit(newRecord);
                context.SubmitChanges();
            }
        }
    }
}
