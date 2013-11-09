using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreightSystem.Models;

namespace FreightSystem.Logics.Interfaces
{
    public interface IBusinessProvider
    {
        TransportRecordListModel QueryTransportModel(int pageIndex);
    }
}
