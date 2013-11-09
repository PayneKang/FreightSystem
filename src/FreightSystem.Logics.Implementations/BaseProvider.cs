using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAO_Access;

namespace FreightSystem.Logics.Implementations
{
    public class BaseProvider
    {
        private IOleDBHelper dbHelper;
        public virtual IOleDBHelper DbHelper
        {
            get
            {
                if (dbHelper == null)
                    dbHelper = new AccessDBHelper();
                return dbHelper;
            }
        }

        
    }
}
