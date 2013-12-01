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
        public TransportRecordListModel QueryTransportModel(string clientName, DateTime? deliverDate,int pageIndex)
        {
            return QueryTransportModel(clientName, deliverDate, pageIndex, null);
        }


        public void InsertTransprotModel(TransportRecordModel model)
        {
            using (SQLDBDataContext context = new SQLDBDataContext())
            {
                TransportRecords newRecord = new TransportRecords()
                {
                    AccountPayble = model.AccountPayble,
                    CarLicense = model.CarLicense,
                    ClientName = model.ClientName,
                    Comment = model.Comment,
                    Deductions = model.Deductions,
                    DeliverDate = model.DeliverDate,
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
                    Volume = model.Volume,
                    TrayNo = model.TrayNo,
                    OilCard = model.OilCard,
                    BusinessArea = model.BusinessArea

                };
                newRecord.TransportRecordsOptionHistory.AddRange(from x in model.HistoryItem
                                                                 select new TransportRecordsOptionHistory()
                                                                 {
                                                                     Description = x.Description,
                                                                     LogDateTime = DateTime.Now,
                                                                     Operation = x.Operation,
                                                                     TransportRecordID = newRecord.ID,
                                                                     UserID = x.UserID
                                                                 });
                context.TransportRecords.InsertOnSubmit(newRecord);
                context.SubmitChanges();
            }
        }


        public TransportRecordListModel QueryDailyTransportModel(string clientName, DateTime deliverDate)
        {
            TransportRecordListModel listModel = new TransportRecordListModel();
            using (SQLDBDataContext context = new SQLDBDataContext())
            {

                int totalCount = context.TransportRecords.Count(x => (string.IsNullOrEmpty(clientName) || x.ClientName == clientName)
                                      && x.DeliverDate.Date == deliverDate);
                listModel.TotalCount = totalCount;
                if (totalCount % TransportRecordListModel.PageSize > 0)
                    listModel.TotalPage++;
                if (listModel.TotalPage < 1)
                    listModel.TotalPage = 1;
                listModel.ClientName = clientName;
                listModel.DeliverDate = deliverDate;
                listModel.ItemList = (from x in context.TransportRecords
                                      orderby x.ID descending
                                      where (string.IsNullOrEmpty(clientName) || x.ClientName == clientName)
                                      && x.DeliverDate.Date == deliverDate
                                      select new TransportRecordModel()
                                      {
                                          ID = x.ID,
                                          AccountPayble = x.AccountPayble,
                                          CarLicense = x.CarLicense,
                                          ClientName = x.ClientName,
                                          Comment = x.Comment,
                                          Deductions = x.Deductions,
                                          DeliverDate = x.DeliverDate,
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
                                          Volume = x.Volume,
                                          TrayNo = x.TrayNo,
                                          OilCard = x.OilCard,
                                          BusinessArea = x.BusinessArea
                                      }).ToList();
            }
            return listModel;
        }


        public List<BusinessAreaModel> ListAllArea()
        {
            using (SQLDBDataContext context = new SQLDBDataContext())
            {
                return (from x in context.BusinessArea
                        select new BusinessAreaModel()
                        {
                            ID = x.ID,
                            AreaName = x.AreaName
                        }).ToList();
            }
        }


        public void InsertNewArea(string newArea)
        {
            using (SQLDBDataContext context = new SQLDBDataContext())
            {
                if (context.BusinessArea.Count(x => x.AreaName == newArea) > 0)
                {
                    throw new Exception("输入的地区名已经存在");
                }
                BusinessArea area = new BusinessArea()
                {
                     AreaName = newArea
                };
                context.BusinessArea.InsertOnSubmit(area);
                context.SubmitChanges();
            }
        }


        public TransportRecordListModel QueryTransportModel(string clientName, DateTime? deliverDate, int pageIndex, BusinessAreaModel area)
        {
            TransportRecordListModel listModel = new TransportRecordListModel();
            if (pageIndex < 1)
                pageIndex = 1;

            using (SQLDBDataContext context = new SQLDBDataContext())
            {

                bool checkDeliverDate = deliverDate.HasValue;
                DateTime dd = DateTime.MinValue;
                if (checkDeliverDate)
                    dd = deliverDate.Value;
                string areaName = string.Empty;
                if (area != null)
                    areaName = area.AreaName;

                int totalCount = context.TransportRecords.Count(x => (string.IsNullOrEmpty(clientName) || x.ClientName == clientName)
                                      && (!checkDeliverDate || x.DeliverDate.Date == dd) && (string.IsNullOrEmpty(areaName) || x.BusinessArea == areaName));
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
                listModel.ClientName = clientName;
                listModel.DeliverDate = deliverDate;
                listModel.ItemList = (from x in context.TransportRecords
                                      orderby x.ID descending
                                      where (string.IsNullOrEmpty(clientName) || x.ClientName == clientName)
                                      && (!checkDeliverDate || x.DeliverDate.Date == dd)
                                      && (string.IsNullOrEmpty(areaName) || x.BusinessArea == areaName)
                                      select new TransportRecordModel()
                                      {
                                          ID = x.ID,
                                          AccountPayble = x.AccountPayble,
                                          CarLicense = x.CarLicense,
                                          ClientName = x.ClientName,
                                          Comment = x.Comment,
                                          Deductions = x.Deductions,
                                          DeliverDate = x.DeliverDate,
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
                                          Volume = x.Volume,
                                          TrayNo = x.TrayNo,
                                          OilCard = x.OilCard,
                                          BusinessArea = x.BusinessArea,
                                          HistoryItem = (from y in x.TransportRecordsOptionHistory
                                                         select new TransportRecordsHistoryModel()
                                                         {
                                                             Description = y.Description,
                                                             LogDateTime = y.LogDateTime,
                                                             Operation = y.Operation,
                                                             TransportRecordID = y.TransportRecordID,
                                                             UserID = y.UserID,
                                                             User = new UserModel()
                                                             {

                                                                 Comment = y.Users.Comment,
                                                                 CreateDateTime = y.Users.CreateDateTime,
                                                                 LastLoginIP = y.Users.LastLoginIP,
                                                                 LastLoginTime = y.Users.LastLoginTime,
                                                                 AreaID = y.Users.AreaID,
                                                                 Area = new BusinessAreaModel()
                                                                 {
                                                                     AreaName = y.Users.BusinessArea.AreaName,
                                                                     ID = y.Users.BusinessArea.ID
                                                                 },
                                                                 Name = y.Users.Name,
                                                                 Password = y.Users.Password,
                                                                 RoleID = y.Users.RoleId,
                                                                 UserID = y.Users.UserID,
                                                                 Role = new RoleModel()
                                                                 {
                                                                     RoleID = y.Users.Roles.RoleID,
                                                                     RoleName = y.Users.Roles.RoleName
                                                                 }
                                                             }
                                                         }).ToList()
                                      }).Skip(startIndex).Take(TransportRecordListModel.PageSize).ToList();
            }
            return listModel;
        }


        public TransportRecordListModel QueryDailyTransportModel(string clientName, DateTime deliverDate, BusinessAreaModel area)
        {
            throw new NotImplementedException();
        }


        public MonthlyReportModel QueryTransportModel(string clientName, DateTime fromDate, DateTime toDate)
        {
            MonthlyReportModel listModel = new MonthlyReportModel();
            using (SQLDBDataContext context = new SQLDBDataContext())
            {

                listModel.ItemList = (from x in context.TransportRecords
                                      orderby x.ID descending
                                      where (string.IsNullOrEmpty(clientName) || x.ClientName == clientName)
                                      && x.DeliverDate.Date >= fromDate && x.DeliverDate <= toDate
                                      select new TransportRecordModel()
                                      {
                                          ID = x.ID,
                                          AccountPayble = x.AccountPayble,
                                          CarLicense = x.CarLicense,
                                          ClientName = x.ClientName,
                                          Comment = x.Comment,
                                          Deductions = x.Deductions,
                                          DeliverDate = x.DeliverDate,
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
                                          Volume = x.Volume,
                                          TrayNo = x.TrayNo,
                                          OilCard = x.OilCard,
                                          BusinessArea = x.BusinessArea
                                      }).ToList();
            }
            return listModel;
        }
    }
}
