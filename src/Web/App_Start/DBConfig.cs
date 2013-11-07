using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAO_Access;
using System.Configuration;

namespace Web
{
    public class DBConfig
    {
        public static void LoadSettings()
        {
            AccessDBHelper.LoadDBSettingsForWeb(ConfigurationManager.AppSettings["AccessConnectionString"], HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["DBPath"]), ConfigurationManager.AppSettings["DBPassword"]);
        }
    }
}