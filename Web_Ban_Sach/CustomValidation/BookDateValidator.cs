using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Web_Ban_Sach.Models;

namespace Web_Ban_Sach.CustomValidation
{
    public class BookDateValidator
    {
        public static ValidationResult ValidatePublicationDate(BookDto dto, ValidationContext context)
        {

            if (dto == null || !dto.Day.HasValue || !dto.Month.HasValue || !dto.Year.HasValue)
            {
                return new ValidationResult("Vui lòng nhập đầy đủ ngày/tháng/năm xuất bản.", new[] { "Day", "Month", "Year" });
            }

            if (dto.PublicationDate == null)
            {
                return new ValidationResult(
                    "Ngày/tháng/năm xuất bản không hợp lệ.",
                    new[] { "PublicationDate" }
                );
            }

            return ValidationResult.Success;
        }

    }
}