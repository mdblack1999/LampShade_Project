using System.Collections.Generic;
using _0_Framework.Infrastructure;

namespace AccountManagement.Configuration.Permission
{
    public class AccountPermissionExposer : IPermissionExposer
    {
        public Dictionary<string , List<PermissionDto>> Expose()
        {
            return new Dictionary<string , List<PermissionDto>>
            {
                {
                    "ACCOUNTS",new List<PermissionDto>
                    {
                        new PermissionDto(AccountPermission.ListAccounts,"لیست کاربران"),
                        new PermissionDto(AccountPermission.SearchAccounts,"جستجو در کاربران"),
                        new PermissionDto(AccountPermission.RegisterAccount,"ایجاد کاربر جدید"),
                        new PermissionDto(AccountPermission.EditAccounts,"ویرایش کاربران"),
                        new PermissionDto(AccountPermission.ChangePassword,"تغییر پسورد کاربران*"),
                    }
                },
                {
                    "(ROLES)", new List<PermissionDto>
                    {
                        new PermissionDto(AccountPermission.ListRoles,"لیست نقش ها*"),
                        new PermissionDto(AccountPermission.SearchRoles,"جستجو در نقش ها*"),
                        new PermissionDto(AccountPermission.CreateRole,"ایجاد نقش جدید*"),
                        new PermissionDto(AccountPermission.EditRoles,"ویرایش نقش ها*"),
                    }
                }
            };
        }
    }
}
