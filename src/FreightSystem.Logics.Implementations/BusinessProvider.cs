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
        public const int IndexLength = 3;
        public TransportRecordListModel QueryTransportModel(string clientName, DateTime? deliverDate,string received,string paid,string error, int pageIndex)
        {
            return QueryTransportModel(clientName, deliverDate,received, paid,error, pageIndex, null);
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
                    BusinessArea = model.BusinessArea,
                    Error = model.Error,
                    Paid = model.Paid,
                    Received = model.Received,
                    ReceivedDate = model.ReceivedDate
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

        public TransportRecordListModel QueryDailyTransportModel(string clientName, DateTime deliverDate,string received,string paid,string error)
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
                                      && ((received == "Y" && x.Received) || (received == "N" && !x.Received) || (received != "Y" && received != "N"))
                                      && ((paid == "Y" && x.Paid) || (paid == "N" && !x.Paid) || (paid != "Y" && paid != "N"))
                                      && ((error == "Y" && x.Error) || (error == "N" && !x.Error) || (error != "Y" && error != "N"))
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
                                          Error = x.Error,
                                          Received = x.Received,
                                          Paid = x.Paid,
                                          ReceivedDate = x.ReceivedDate
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
        
        public TransportRecordListModel QueryTransportModel(string clientName, DateTime? deliverDate,string received,string paid,string error, int pageIndex, BusinessAreaModel area)
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
                                      && ((received == "Y" && x.Received) || (received == "N" && !x.Received) || (received != "Y" && received != "N"))
                                      && ((paid == "Y" && x.Paid) || (paid == "N" && !x.Paid) || (paid != "Y" && paid != "N"))
                                      && ((error == "Y" && x.Error) || (error == "N" && !x.Error) || (error != "Y" && error != "N"))
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
                                          Error = x.Error,
                                          Paid = x.Paid,
                                          Received = x.Received,
                                          ReceivedDate = x.ReceivedDate,
                                          HistoryItem = (from y in x.TransportRecordsOptionHistory
                                                         orderby y.LogDateTime
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

        public TransportRecordListModel QueryDailyTransportModel(string clientName, DateTime deliverDate,string received,string paid,string error, BusinessAreaModel area)
        {
            throw new NotImplementedException();
        }

        public MonthlyReportModel QueryTransportModel(string clientName,string received,string paid,string error, DateTime fromDate, DateTime toDate)
        {
            MonthlyReportModel listModel = new MonthlyReportModel();
            using (SQLDBDataContext context = new SQLDBDataContext())
            {

                listModel.ItemList = (from x in context.TransportRecords
                                      orderby x.ID descending
                                      where (string.IsNullOrEmpty(clientName) || x.ClientName == clientName)
                                      && x.DeliverDate.Date >= fromDate && x.DeliverDate <= toDate
                                      && ((received == "Y" && x.Received) || (received == "N" && !x.Received) || (received != "Y" && received != "N"))
                                      && ((paid == "Y" && x.Paid) || (paid == "N" && !x.Paid) || (paid != "Y" && paid != "N"))
                                      && ((error == "Y" && x.Error) || (error == "N" && !x.Error) || (error != "Y" && error != "N"))
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
                                          Error = x.Error,
                                          Paid = x.Paid,
                                          Received = x.Received,
                                          ReceivedDate = x.ReceivedDate
                                      }).ToList();
            }
            return listModel;
        }

        public TransportRecordModel GetTransportRecordModel(int id)
        {
            using (SQLDBDataContext context = new SQLDBDataContext())
            {
                return (from x in context.TransportRecords
                        where x.ID == id
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
                            Error = x.Error,
                            ErrorMessage = x.ErrorMessage,
                            Paid = x.Paid,
                            Received = x.Received,
                            ReceivedDate = x.ReceivedDate,
                            HistoryItem = (from y in x.TransportRecordsOptionHistory
                                           orderby y.LogDateTime
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
                                           }).ToList(),
                            DetailItem = (from y in x.TransportRecordDetail
                                          select new TransportRecordDetailModel()
                                          {
                                              DetailNo = y.DetailNo,
                                              ID = y.ID,
                                              PackageName = y.PackageName,
                                              Quantity = y.Quantity,
                                              TransportRecordID = y.TransportRecordID,
                                              Volume = y.Volume
                                          }).ToList()
                        }).FirstOrDefault();
            }
        }

        public void UpdateTransportModel(int id, string trayNo, double volume, int quantity, string userID)
        {
            using (SQLDBDataContext context = new SQLDBDataContext())
            {
                TransportRecords record = context.TransportRecords.FirstOrDefault(x => x.ID == id);
                if (record == null)
                {
                    throw new ApplicationException("要修改的记录不存在");
                }
                record.TrayNo = trayNo;
                record.Volume = volume;
                record.Quantity = quantity;
                record.TransportRecordsOptionHistory.Add(
                    new TransportRecordsOptionHistory()
                    {
                        Description = string.Format("更新单据内容，新内容 托编号:{0} 体积： {1} 数量：{2} ", trayNo, volume, quantity),
                        LogDateTime = DateTime.Now,
                        Operation = "更新",
                        UserID = userID
                    });
                Client client = context.Client.FirstOrDefault(x => x.ClientName == record.ClientName);
                if (client != null)
                {
                    client.Index = GetTrayNoIndex(trayNo);
                    if (client.IndexMonth != DateTime.Now.Month) {
                        client.Index = 0;
                        client.IndexMonth = DateTime.Now.Month;
                    }
                }
                context.SubmitChanges();
            }
        }

        public void UpdateTransportPaymentData(int id, DateTime? paymentDate, double? accountPayble, double? deductions, double? reparations, bool paid, string userID)
        {
            using (SQLDBDataContext context = new SQLDBDataContext())
            {
                TransportRecords record = context.TransportRecords.FirstOrDefault(x => x.ID == id);
                if (record == null)
                {
                    throw new ApplicationException("要修改的记录不存在");
                }
                record.PayDate = paymentDate;
                record.AccountPayble = accountPayble;
                record.Deductions = deductions;
                record.Reparations = reparations;
                record.Paid = paid;
                record.TransportRecordsOptionHistory.Add(
                    new TransportRecordsOptionHistory()
                    {
                        Description = string.Format("财务补充单据内容，新内容 赔款：{0} 扣款:{1} 付款日:{2} 应付金额： {3} ",reparations,deductions, paymentDate,accountPayble),
                        LogDateTime = DateTime.Now,
                        Operation = "更新",
                        UserID = userID
                    });
                context.SubmitChanges();
            }
        }

        public void UpdateTransportErrorStatus(int id, bool error, string errorMessage, string userID)
        {
            using (SQLDBDataContext context = new SQLDBDataContext())
            {
                TransportRecords record = context.TransportRecords.FirstOrDefault(x => x.ID == id);
                if (record == null)
                {
                    throw new ApplicationException("要修改的记录不存在");
                }
                record.Error = error;
                record.ErrorMessage = errorMessage;
                record.TransportRecordsOptionHistory.Add(
                    new TransportRecordsOptionHistory()
                    {
                        Description = string.Format("修改异常状态为{0}, 异常信息：{1}", error?"异常":"正常",errorMessage),
                        LogDateTime = DateTime.Now,
                        Operation = "修改异常状态",
                        UserID = userID
                    });
                context.SubmitChanges();
            }
        }

        public void UpdateTransportReceivedStatus(int id, bool received, DateTime? receivedDate,string userID)
        {
            using (SQLDBDataContext context = new SQLDBDataContext())
            {
                TransportRecords record = context.TransportRecords.FirstOrDefault(x => x.ID == id);
                if (record == null)
                {
                    throw new ApplicationException("要修改的记录不存在");
                }
                record.Received = received;
                record.ReceivedDate = receivedDate;
                record.TransportRecordsOptionHistory.Add(
                    new TransportRecordsOptionHistory()
                    {
                        Description = string.Format("修改到货状态为{0},日期 {1}", received ? "到货" : "未到货", receivedDate),
                        LogDateTime = DateTime.Now,
                        Operation = "修改到货状态",
                        UserID = userID
                    });
                context.SubmitChanges();
            }
        }

        public void InsertClient(ClientModel client)
        {
            using (SQLDBDataContext context = new SQLDBDataContext())
            {
                if (context.Client.Count(x => x.ClientName == client.ClientName) > 0)
                {
                    throw new ApplicationException("同名客户已经存在");
                }
                Client newclient = new Client()
                {
                    ClientName = client.ClientName,
                    CreateTime = DateTime.Now,
                    Index = 0,
                    IndexMonth = DateTime.Now.Month,
                    ShortName = client.ShortName
                };
                context.Client.InsertOnSubmit(newclient);
                context.SubmitChanges();
            }
        }

        public List<ClientModel> QueryClient()
        {
            using (SQLDBDataContext context = new SQLDBDataContext())
            {
                return (from x in context.Client
                        select new ClientModel()
                        {
                            ClientName = x.ClientName,
                            CreateTime = x.CreateTime,
                            IndexMonth = x.IndexMonth,
                            ID = x.ID,
                            Index = x.Index,
                            ShortName = x.ShortName
                        }).ToList();
            }
        }

        public ClientModel GetClient(int id)
        {
            using (SQLDBDataContext context = new SQLDBDataContext())
            {
                if (context.Client.Count(x => x.ID == id) == 0)
                    return null;

                return (from x in context.Client
                        where x.ID == id
                        select new ClientModel()
                        {
                            ClientName = x.ClientName,
                            CreateTime = x.CreateTime,
                            Index = x.Index,
                            ID = x.ID,
                            IndexMonth = x.IndexMonth,
                            ShortName = x.ShortName
                        }).FirstOrDefault();
            }
        }

        public void UpdateClientInformation(ClientModel client)
        {
            using (SQLDBDataContext context = new SQLDBDataContext())
            {
                Client update = context.Client.FirstOrDefault(x => x.ID == client.ID);
                if (update == null)
                    throw new ApplicationException("要修改的客户信息不存在");
                update.ClientName = client.ClientName;
                update.ShortName = client.ShortName;
                context.SubmitChanges();
            }
        }

        public void UpdateTransportPrice(int id, double? deliverPrice, double? shortBargeFee, string userID)
        {
            using (SQLDBDataContext context = new SQLDBDataContext())
            {
                TransportRecords record = context.TransportRecords.FirstOrDefault(x => x.ID == id);
                if (record == null)
                {
                    throw new ApplicationException("要修改的记录不存在");
                }
                record.DeliverPrice = deliverPrice;
                record.ShortBargeFee = shortBargeFee;
                record.TransportRecordsOptionHistory.Add(
                    new TransportRecordsOptionHistory()
                    {
                        Description = string.Format("修改价格信息， 运费：{0}，短驳费：{1}",deliverPrice,shortBargeFee),
                        LogDateTime = DateTime.Now,
                        Operation = "修改价格信息",
                        UserID = userID
                    });
                context.SubmitChanges();
            }
        }


        public string GetNextTrayNo(string clientName)
        {
            using (SQLDBDataContext context = new SQLDBDataContext())
            {
                Client client = context.Client.FirstOrDefault(x => x.ClientName == clientName);
                if (client == null)
                    return string.Empty;
                int index = client.Index + 1;
                if (client.IndexMonth != DateTime.Now.Month)
                    index = 1;
                return string.Format("{0}{1}-{2}", client.ShortName, DateTime.Now.Month.ToString().PadLeft(2, '0'), index.ToString().PadLeft(IndexLength, '0'));
            }
        }

        private int GetTrayNoIndex(string trayNo)
        {
            return int.Parse(trayNo.Substring(trayNo.Length - IndexLength));
        }


        public void InsertNewTransportDetail(TransportRecordDetailModel detail,string userID)
        {
            using (SQLDBDataContext context = new SQLDBDataContext())
            {
                context.TransportRecordDetail.InsertOnSubmit(
                    new TransportRecordDetail()
                    {
                        DetailNo = detail.DetailNo,
                        PackageName = detail.PackageName,
                        Quantity = detail.Quantity,
                        TransportRecordID = detail.TransportRecordID,
                        Volume = detail.Volume
                    }
                    );
                context.TransportRecordsOptionHistory.InsertOnSubmit(
                    new TransportRecordsOptionHistory()
                    {
                        Description = string.Format("插入新货物明细， 编号：{0},货物{1},数量：{2}，体积：{3}", detail.DetailNo, detail.PackageName, detail.Quantity, detail.Volume),
                        LogDateTime = DateTime.Now,
                        Operation = "新货物明细",
                        UserID = userID,
                        TransportRecordID = detail.TransportRecordID
                    }
                    );
                context.SubmitChanges();
            }
        }


        public void UpdateTransportModel(TransportRecordModel model, string userID)
        {
            using (SQLDBDataContext context = new SQLDBDataContext())
            {
                TransportRecords record = context.TransportRecords.FirstOrDefault(x => x.ID == model.ID);
                record.AccountPayble = model.AccountPayble;
                record.CarLicense = model.CarLicense;
                record.ClientName = model.ClientName;
                record.Comment = model.Comment;
                record.Deductions = model.Deductions;
                record.DeliverDate = model.DeliverDate;
                record.DeliverPrice = model.DeliverPrice;
                record.DeliverType = model.DeliverType;
                record.Driver = model.Driver;
                record.Error = model.Error;
                record.ErrorMessage = model.ErrorMessage;
                record.FromLocation = model.FromLocation;
                record.HandlingFee = model.HandlingFee;
                record.OilCard = model.OilCard;
                record.PackageName = model.PackageName;
                record.Paid = model.Paid;
                record.PayDate = model.PayDate;
                record.PrePay = model.PrePay;
                record.Quantity = model.Quantity;
                record.Received = model.Received;
                record.ReceivedDate = model.ReceivedDate;
                record.Reparations = model.Reparations;
                record.ShortBargeFee = model.ShortBargeFee;
                record.Status = model.Status;
                record.ToLocation = model.ToLocation;
                record.TrayNo = model.TrayNo;
                record.Volume = model.Volume;
                context.SubmitChanges();
            }
        }
    }
}