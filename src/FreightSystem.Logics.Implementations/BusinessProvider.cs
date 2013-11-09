using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreightSystem.Logics.Interfaces;

namespace FreightSystem.Logics.Implementations
{
    public class BusinessProvider:IBusinessProvider
    {
        public Models.TransportRecordListModel QueryTransportModel(int pageIndex)
        {
            throw new NotImplementedException();
        }
    }
}
