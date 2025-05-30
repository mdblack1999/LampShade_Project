﻿using _0_Framework.Application;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ShopManagement.Application.Contracts.ProductCategory
{
    public class CreateProductCategory
    {
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string Name { get; set; }

        public string Description { get; set; }

        [FileExtensionLimitation(new[] { ".jpeg" , ".jpg" , ".png" } , ErrorMessage = ValidationMessages.IsInValidFileFormat)]
        //[Required(ErrorMessage = ValidationMessages.IsRequired)]
        [MaxFileSize(3 * 1024 * 1024 , ErrorMessage = ValidationMessages.MaxFileSize)]
        public IFormFile Picture { get; set; }
        public string PictureAlt { get; set; }
        public string PictureTitle { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string Keywords { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string MetaDescription { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string Slug { get; set; }

        public string SubText { get; set; } = "همین الان بخر!";
    }
}
