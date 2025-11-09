using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace Web_Ban_Sach.Models
{
    public class BookImageValidator
    {
        public static ValidationResult ValidateImage(HttpPostedFileBase imageFile, ValidationContext context)
        {
            if (imageFile == null || imageFile.ContentLength <= 0)
                return ValidationResult.Success;

            var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var ext = Path.GetExtension(imageFile.FileName)?.ToLower();

            if (!allowed.Contains(ext))
                return new ValidationResult("File không hợp lệ! (Chỉ chấp nhận .jpg, .jpeg, .png, .gif)");

            return ValidationResult.Success;
        }
    }
}