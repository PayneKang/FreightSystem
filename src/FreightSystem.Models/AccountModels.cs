﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

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
    public class UserListModel{
        public List<UserModel> ItemList { get; set; }
    }
    public class RoleModel
    {
        public int RoleID { get; set; }
        [Required(ErrorMessage = "角色名称必须填写")]
        public string RoleName { get; set; }
        public List<AccessModel> Accesses { get; set; }
        public List<MenuItemModel> Menus { get; set; }
    }
    public class MenuItemModel
    {
        public string MenuCode { get; set; }
        public string MenuText { get; set; }
        public string Link { get; set; }
        public int OrderIndex { get; set; }
    }
    public class RoleListModel
    {
        public List<RoleModel> ItemList { get; set; }
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
    public class ChangePasswordModel {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}