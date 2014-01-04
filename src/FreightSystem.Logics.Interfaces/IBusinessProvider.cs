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
        void InsertTransprotModel(TransportRecordModel model);
        void InsertNewArea(string newArea);
        List<BusinessAreaModel> ListAllArea();
        TransportRecordModel GetTransportRecordModel(int id);
        void UpdateTransportModel(int id, string trayNo, double volume, int quantity, string userID);
        void UpdateTransportPaymentData(int id, DateTime? paymentDate, double? accountPayble, double? deductions, double? reparations, bool paid, string userID);
        void UpdateTransportErrorStatus(int id, bool error,string errorMessage,string userID);
        void UpdateTransportReceivedStatus(int id, bool received,DateTime? receivedDate, string userID);
        void InsertClient(ClientModel client);
        List<ClientModel> QueryClient();
        ClientModel GetClient(int id);
        void UpdateClientInformation(ClientModel client);
    }
}
