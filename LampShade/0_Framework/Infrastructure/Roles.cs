namespace _0_Framework.Infrastructure
{
    public static class Roles
    {
        public const string Administrator = "1";
        public const string SystemUser = "2";
        public const string ContentAdmin = "3";
        public const string ProductAdmin = "4";
        public const string UserAdmin = "5";
        public const string StoreKeeper = "6";
        public const string DiscountAdmin = "7";
        public const string ColleagueUser = "8";


        public static string GetRoleBy(long id)
        {
            return id switch
            {
                1 => "مدیرسیستم",
                2 => "کاربرسیستم",
                3 => "مدیرمحتوا",
                4 => "مدیرمحصول",
                5 => "مدیرکاربران",
                6 => "مدیرانبارداری",
                7 => "مدیرتخفیفات",
                8 => "کاربر همکار",
                _ => ""
            };
        }
    }
}
