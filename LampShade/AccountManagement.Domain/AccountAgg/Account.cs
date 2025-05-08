using _0_Framework.Domain;
using AccountManagement.Domain.RoleAgg;
using Microsoft.AspNetCore.Http;

namespace AccountManagement.Domain.AccountAgg
{
    public class Account : EntityBase
    {
        public string FullName { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Mobile { get; private set; }
        public long RoleId { get; private set; }
        public Role Role { get; private set; }
        public string ProfilePhoto { get; private set; }

        public Account(string fullName , string username , string password , string mobile , long roleId ,
            string profilePhoto)
        {
            FullName = fullName;
            Username = username;
            Password = password;
            Mobile = mobile;
            RoleId = roleId;
            ProfilePhoto = profilePhoto;
        }
        public void Edit(string fullName , string username , string mobile , long roleId ,
            string profilePhoto)
        {
            FullName = fullName;
            Username = username;
            Mobile = mobile;
            RoleId = roleId;
            if (!string.IsNullOrWhiteSpace(ProfilePhoto))
                ProfilePhoto = profilePhoto;
        }

        public void ChangePassword(string password)
        {
            Password = password;
        }
    }
}
