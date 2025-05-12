using System.Collections.Generic;
using _0_Framework.Infrastructure;

namespace ShopManagement.Configuration.Permissions
{
    public class ShopPermissionExposer : IPermissionExposer
    {
        public Dictionary<string , List<PermissionDto>> Expose()
        {
            return new Dictionary<string , List<PermissionDto>>
            {
                {
                    "PRODUCT", new List<PermissionDto>
                    {
                        new PermissionDto(ShopPermissions.ListProducts,"لیست محصولات"),
                        new PermissionDto(ShopPermissions.SearchProducts,"جستجوی محصولات"),
                        new PermissionDto(ShopPermissions.CreateProduct,"ایجاد محصول"),
                        new PermissionDto(ShopPermissions.EditProduct,"ویرایش محصول"),
                    }
                },
                {
                    "PRODUCT CATEGORY", new List<PermissionDto>
                    {
                        new PermissionDto(ShopPermissions.ListProductCategories,"مشاهده گروه محصولات"),
                        new PermissionDto(ShopPermissions.SearchProductCategories,"جستجو در گروه محصولات"),
                        new PermissionDto(ShopPermissions.CreateProductCategory,"ایجاد گروه محصول"),
                        new PermissionDto(ShopPermissions.EditProductCategory,"ویرایش گروه محصول"),
                        new PermissionDto(ShopPermissions.RemoveAndRestoreProductCategory,"حذف و بازگردانی گروه محصول"),
                    }
                },
                {
                    "PRODUCT PICTURE", new List<PermissionDto>
                    {
                        new PermissionDto(ShopPermissions.ListProductPictures,"مشاهده عکس محصولات"),
                        new PermissionDto(ShopPermissions.SearchProductPictures,"جستجو در عکس محصولات"),
                        new PermissionDto(ShopPermissions.CreateProductPictures,"ایجاد عکس محصولات"),
                        new PermissionDto(ShopPermissions.EditProductPictures, "ویرایش عکس محصولات"),
                        new PermissionDto(ShopPermissions.RemoveAndRestoreProductPictures, "حذف و بازگردانی عکس محصولات"),
                    }
                },
                {
                    "SLIDES", new List<PermissionDto>
                    {
                        new PermissionDto(ShopPermissions.ListSlides,"مشاهده اسلایدر"),
                        new PermissionDto(ShopPermissions.CreateSlide,"ایجاد اسلایدر"),
                        new PermissionDto(ShopPermissions.EditSlide, "ویرایش اسلایدر"),
                        new PermissionDto(ShopPermissions.RemoveAndRestoreSlide, "حذف و بازگردانی اسلایدر"),
                    }
                }
            };
        }
    }
}
