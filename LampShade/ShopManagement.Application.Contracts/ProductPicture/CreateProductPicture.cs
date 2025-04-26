using _0_Framework.Application;
using ShopManagement.Application.Contracts.Product;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ShopManagement.Application.Contracts.ProductPicture
{
    public class CreateProductPicture
    {
        [Range(1 , 300000 , ErrorMessage = ValidationMessages.IsRequired)]
        public long ProductId { get; set; }

        [FileExtensionLimitation(new[] { "jpeg" , "jpg" , "png" } , ErrorMessage = ValidationMessages.IsInValidFileFormat)]
        [MaxFileSize(1 * 1024 * 1024 , ErrorMessage = ValidationMessages.MaxFileSize)]
        public IFormFile Picture { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string PictureAlt { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string PictureTitle { get; set; }

        public List<ProductViewModel> Products { get; set; }
    }
}
