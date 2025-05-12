using System.Collections.Generic;
using _0_Framework.Infrastructure;

namespace BlogManagement.Infrastructure.Configuration.Permission
{
    public class BlogPermissionExposer : IPermissionExposer
    {
        public Dictionary<string, List<PermissionDto>> Expose()
        {
            return new Dictionary<string , List<PermissionDto>>
            {
                {
                    "ARTICLE-CATEGORY", new List<PermissionDto>
                    {
                        new PermissionDto(BlogPermission.ListArticleCategories,"لیست گروه مقالات"),
                        new PermissionDto(BlogPermission.SearchArticleCategories,"جستجو در گروه مقالات"),
                        new PermissionDto(BlogPermission.CreateArticleCategory,"ایجاد گروه مقاله جدید"),
                        new PermissionDto(BlogPermission.EditArticleCategory,"ویرایش گروه مقاله ها"),
                    }
                },
                {
                    "ARTICLES", new List<PermissionDto>
                    {
                        new PermissionDto(BlogPermission.ListArticles,"لیست مقالات"),
                        new PermissionDto(BlogPermission.SearchArticles,"جستجو در مقالات"),
                        new PermissionDto(BlogPermission.CreateArticle,"ایجاد مقاله جدید"),
                        new PermissionDto(BlogPermission.EditArticle,"ویرایش مقاله ها"),
                    }
                }
            };
        }
    }
}
