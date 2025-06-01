using System.ComponentModel.DataAnnotations;
using _0_Framework.Application;
using Microsoft.AspNetCore.Http;

namespace BlogManagement.Application.Contracts.Article
{
    public class CreateArticle
    {
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string Title { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string ShortDescription { get; set; }

        public string Description { get; set; }

        [FileExtensionLimitation(new []{"jpeg","jpg","png"},ErrorMessage = ValidationMessages.IsInValidFileFormat)]
        [MaxFileSize(5*1024*1024,ErrorMessage = ValidationMessages.MaxFileSize)]
        public IFormFile Picture { get; set; }
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string PictureAlt { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string PictureTitle { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string PublishDate { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string Slug { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string Keywords { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        [MaxLength(500,ErrorMessage = ValidationMessages.MaxTextSize)]
        public string MetaDescription { get; set; }

        [MaxLength(1000, ErrorMessage = ValidationMessages.MaxTextSize)]
        [Url(ErrorMessage = ValidationMessages.InvalidUrl)]
        public string CanonicalAddress { get; set; }

        [Range(1,long.MaxValue,ErrorMessage = ValidationMessages.IsInvalid)]
        public long CategoryId { get; set; }

    }
}
