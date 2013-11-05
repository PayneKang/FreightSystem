using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FreightSystem.Models
{
    public class UserModel
    {
        public string Location { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int RoleID { get; set; }
        public string UserID { get; set; }
        public string Comment { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime LastLoginTime { get; set; }
        public string LastLoginIP { get; set; }
        public RoleModel Role { get; set; }
    }
    public class RoleModel
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public AccessModel[] Accesses { get; set; }
    }
    public class AccessModel
    {
        public int AccessID { get; set; }
        public bool CanCreate { get; set; }
        public bool CanDelete { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanQuery { get; set; }
        public string ModelName { get; set; }
        public string PropertyName { get; set; }
        public int RoleID { get; set; }
    }
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}