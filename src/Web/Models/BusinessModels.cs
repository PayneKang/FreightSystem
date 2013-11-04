using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FreightSystem.Models
{
    public class ClientModel
    {
        public string UserID { get; set; }
        public string Realname { get; set; }
        public string Password { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}