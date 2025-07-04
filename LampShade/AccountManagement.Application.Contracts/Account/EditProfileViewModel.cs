﻿using System.ComponentModel.DataAnnotations;
using _0_Framework.Application;
using Microsoft.AspNetCore.Http;

namespace AccountManagement.Application.Contracts.Account
{
    public class EditProfileViewModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string FullName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = ApplicationMessages.PasswordNotMatch)]
        [DataType(DataType.Password)]
        public string RePassword { get; set; }

        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string Mobile { get; set; }

        [FileExtensionLimitation(new []{".jpeg",".jpg",".png"},ErrorMessage = ValidationMessages.IsInValidFileFormat)]
        [MaxFileSize(10*1024*1024,ErrorMessage = ValidationMessages.MaxFileSize)]
        public IFormFile ProfilePhoto { get; set; }

        public string CurrentPhotoPath { get; set; }
    }

}
