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
        TransportRecordListModel QueryDailyTransportModel(string clientName, DateTime deliverDate);
        void InsertTransprotModel(TransportRecordModel model);
        List<BusinessAreaModel> ListAllArea();
    }
}
