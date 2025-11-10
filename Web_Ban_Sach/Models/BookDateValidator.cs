using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web_Ban_Sach.Models
{
    public class BookDateValidator
    {
        public static ValidationResult ValidatePublicationDate(BookDto dto, ValidationContext context)
        {
            if (dto == null)
                return ValidationResult.Success;

            if (dto.Day <= 0 || dto.Month <= 0 || dto.Year <= 0)
                return new ValidationResult("Vui lòng nhập đầy đủ ngày/tháng/năm xuất bản.");

            if(dto.PublicationDate == null)
                return new ValidationResult("Ngày/tháng/năm xuất bản không hợp lệ.");

            return ValidationResult.Success;
        }

    }
}