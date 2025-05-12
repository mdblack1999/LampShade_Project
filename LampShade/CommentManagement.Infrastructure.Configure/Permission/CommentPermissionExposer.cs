using System.Collections.Generic;
using _0_Framework.Infrastructure;

namespace CommentManagement.Infrastructure.Configure.Permission
{
    public class CommentPermissionExposer : IPermissionExposer
    {
        public Dictionary<string , List<PermissionDto>> Expose()
        {
            return new Dictionary<string , List<PermissionDto>>
            {
                {
                    "COMMENTS", new List<PermissionDto>
                    {
                        new PermissionDto(CommentPermission.ListComments,"لیست کامنت ها"),
                        new PermissionDto(CommentPermission.SearchComments,"جستجو در کامنت ها"),
                        new PermissionDto(CommentPermission.ChangeStatusComments,"تغییر وضعیت کامنت ها"),
                    }
                }
            };
        }
    }
}
