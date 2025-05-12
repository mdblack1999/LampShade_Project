using System.Collections.Generic;
using _0_Framework.Infrastructure;

namespace DiscountManagement.Configuration.Permission
{
    public class DiscountPermissionExposer : IPermissionExposer
    {
        public Dictionary<string , List<PermissionDto>> Expose()
        {
            return new Dictionary<string , List<PermissionDto>>
            {
                {
                    "CUSTOMER DISCOUNT", new List<PermissionDto>
                    {
                        new PermissionDto(DiscountPermission.ListCustomerDiscounts,"مشاهده تخفیفات مشتری"),
                        new PermissionDto(DiscountPermission.SearchCustomerDiscounts,"جستجو در تخفیفات مشتری"),
                        new PermissionDto(DiscountPermission.DefineCustomerDiscounts,"تعریف تخفیفات مشتری"),
                        new PermissionDto(DiscountPermission.EditCustomerDiscounts,"ویرایش تخفیفات مشتری"),
                    }
                },
                {
                    "COLLEAGUE DISCOUNT", new List<PermissionDto>
                    {
                        new PermissionDto(DiscountPermission.ListColleagueDiscounts,"مشاهده تخفیفات همکاری"),
                        new PermissionDto(DiscountPermission.SearchColleagueDiscounts,"جستجو در تخفیفات همکاری"),
                        new PermissionDto(DiscountPermission.DefineColleagueDiscounts,"تعریف تخفیفات همکاری"),
                        new PermissionDto(DiscountPermission.EditColleagueDiscounts,"ویرایش تخفیفات همکاری"),
                        new PermissionDto(DiscountPermission.ActiveDeActiveColleagueDiscount,"فعال و غیرفعال سازی تخفیفات همکاری"),
                    }
                }
            };
        }
    }
}
