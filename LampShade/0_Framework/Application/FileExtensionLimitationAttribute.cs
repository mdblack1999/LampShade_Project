﻿using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace _0_Framework.Application
{
    public class FileExtensionLimitationAttribute : ValidationAttribute, IClientModelValidator
    {
        private readonly string[] _validExtension;

        public FileExtensionLimitationAttribute(string[] validExtension)
        {
            _validExtension = validExtension;
        }

        public override bool IsValid(object value)
        {
            var file = value as IFormFile;
            if (file == null) return true;

            var fileExtension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
            return _validExtension.Select(ext => ext.ToLowerInvariant()).Contains(fileExtension);
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.Add("data-val-FileExtensionLimit", ErrorMessage);
        }
    }
}