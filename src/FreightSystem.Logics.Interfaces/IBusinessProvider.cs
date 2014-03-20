using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreightSystem.Models;

namespace FreightSystem.Logics.Interfaces
{
    public interface IBusinessProvider
    {
        TransportRecordListModel QueryTransportModel(string clientName, DateTime? deliverDate, string received, string paid, string error, int pageIndex);
        TransportRecordListModel QueryTransportModel(string clientName, DateTime? deliverDate, string received, string paid, string error, int pageIndex, BusinessAreaModel area);
        TransportRecordListModel QueryDailyTransportModel(string clientName, DateTime deliverDate, string received, string paid, string error);
        TransportRecordListModel QueryDailyTransportModel(string clientName, DateTime deliverDate, string received, string paid, string error, BusinessAreaModel area);
        MonthlyReportModel QueryTransportModel(string clientName, string received, string paid, string error, DateTime fromDate, DateTime toDate);
        int InsertTransprotModel(TransportRecordModel model);
        void InsertNewArea(string newArea);
        List<BusinessAreaModel> ListAllArea();
        TransportRecordModel GetTransportRecordModel(int id);
        void UpdateTransportModel(int id, string trayNo, double volume, int quantity,bool updateClientIndex, string userID);
        void UpdateTransportModel(TransportRecordModel model, string userID);
        void UpdateTransportPaymentData(int id, DateTime? paymentDate, double? accountPayble, double? deductions, double? reparations,double? handlingfee, bool paid, string userID);
        void UpdateTransportErrorStatus(int id, bool error,string errorMessage,string userID);
        void UpdateTransportReceivedStatus(int id, bool received,DateTime? receivedDate, string userID);
        void UpdateTransportPrice(int id, double? deliverPrice, double? shortBargeFee,double? accoundPayable, string userID);
        void InsertClient(ClientModel client);
        void InsertNewTransportDetail(TransportRecordDetailModel detail, string userID);
        List<ClientModel> QueryClient();
        ClientModel GetClient(int id);
        void UpdateClientInformation(ClientModel client);
        string GetNextTrayNo(string clientName);
        TransportRecordDetailModel GetTransportRecordDetailModel(long id);
        void UpdateTransportDetailModel(TransportRecordDetailModel model);
        void DeleteTransportRecordDetail(long id);
    }
}
