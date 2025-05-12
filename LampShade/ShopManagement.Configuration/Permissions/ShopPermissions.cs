namespace ShopManagement.Configuration.Permissions
{
    public static class ShopPermissions
    {
        //Product
        public const int ListProducts = 10;
        public const int SearchProducts = 11;    
        public const int CreateProduct = 12;
        public const int EditProduct = 13;

        //Product Category
        public const int ListProductCategories = 20;
        public const int SearchProductCategories = 21;
        public const int CreateProductCategory = 22;
        public const int EditProductCategory = 23;
        public const int RemoveAndRestoreProductCategory = 24;

        //Product Picture
        public const int ListProductPictures = 30;  
        public const int SearchProductPictures = 31;
        public const int CreateProductPictures = 32;
        public const int EditProductPictures = 33;
        public const int RemoveAndRestoreProductPictures = 34;
            
        //Slides
        public const int ListSlides = 40;
        public const int CreateSlide = 41;
        public const int EditSlide= 42;
        public const int RemoveAndRestoreSlide = 43;
    }
}