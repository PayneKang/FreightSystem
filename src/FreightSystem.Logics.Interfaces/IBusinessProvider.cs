using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreightSystem.Models;

namespace FreightSystem.Logics.Interfaces
{
    public interface IBusinessProvider
    {
        TransportRecordListModel QueryTransportModel(string clientName, DateTime? deliverDate, int pageIndex);
        TransportRecordListModel QueryTransportModel(string clientName, DateTime? deliverDate, int pageIndex,BusinessAreaModel area);
        TransportRecordListModel QueryDailyTransportModel(string clientName, DateTime deliverDate);
        TransportRecordListModel QueryDailyTransportModel(string clientName, DateTime deliverDate, BusinessAreaModel area);
        void InsertTransprotModel(TransportRecordModel model);
        void InsertNewArea(string newArea);
        List<BusinessAreaModel> ListAllArea();
    }
}
