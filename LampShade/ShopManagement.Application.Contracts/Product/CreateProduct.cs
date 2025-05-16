﻿using _0_Framework.Application;
using ShopManagement.Application.Contracts.ProductCategory;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ShopManagement.Application.Contracts.Product
{
    public class CreateProduct
    {
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string Name { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string Code { get; set; }
        public bool IsInStock { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string ShortDescription { get; set; }
        public string Description { get; set; }

        [MaxFileSize(2 * 1024 * 1024 , ErrorMessage = ValidationMessages.MaxFileSize)]
        [FileExtensionLimitation(new[] { "jpeg" , "jpg" , "png" } , ErrorMessage = ValidationMessages.IsInValidFileFormat)]
        public IFormFile Picture { get; set; }

        public string PictureAlt { get; set; }
        public string PictureTitle { get; set; }
        [Range(1 , 100000 , ErrorMessage = ValidationMessages.IsRequired)]
        public long CategoryId { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string Slug { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string Keywords { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        [MaxLength(500,ErrorMessage = ValidationMessages.MaxTextSize)]
        public string MetaDescription { get; set; }
        public List<ProductCategoryViewModel> Categories { get; set; }
    }

}
