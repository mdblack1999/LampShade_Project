using System;

namespace _0_Framework.Infrastructure
{
    public class NeedsPermissionAttribute : Attribute
    {
        public int PermissionCode { get; set; }

        public NeedsPermissionAttribute(int permission)
        {
            PermissionCode = permission;
        }
    }
}
