
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace Web_Ban_Sach.Models
{
    [CustomValidation(typeof(BookDateValidator), nameof(BookDateValidator.ValidatePublicationDate))]
    public class BookDto
    {
        [Required(ErrorMessage = "Vui lòng nhập tên sách")]
        [StringLength(200)]
        public string Name { get; set; } // ten sach


        [Range(1, 31, ErrorMessage = "Ngày không hợp lệ")]
        public int? Day { get; set; } // ngay xuat ban


        [Range(1, 12, ErrorMessage = "Tháng không hợp lệ")]
        public int? Month { get; set; } // thang xuat ban


        [Range(1900, 2100, ErrorMessage = "Năm không hợp lệ")]
        public int? Year { get; set; } // nam xuat ban

        [Display(Name = "Ngày xuất bản")]
        public DateTime? PublicationDate
        {
            get
            {
                if (!Day.HasValue || !Month.HasValue || !Year.HasValue)
                    return null;

                try
                {
                    return new DateTime(Year.Value, Month.Value, Day.Value);
                }
                catch
                {
                    return null;
                }
            }
        }


        [Display(Name = "Mô tả")]
        public string Description { get; set; } // mo ta sach


        [Required(ErrorMessage = "Vui lòng nhập giá sách")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải >= 0")]

        public double? Price { get; set; } // gia sach


        [CustomValidation(typeof(BookImageValidator), nameof(BookImageValidator.ValidateImage))]
        public HttpPostedFileBase ImageFile { get; set; }



        public string SaveImage(string serverPath)
        {
            if (ImageFile == null || ImageFile.ContentLength <= 0)
            {
                return "~/Images/anhmacdinh.jpg";
            }

            var fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);// bỏ đuôi
            var ext = Path.GetExtension(ImageFile.FileName)?.ToLower();// lấy đuôi
            var unique = $"{fileName}_{DateTime.Now:yyyyMMddHHmmss}{ext}";// laays yyyyMMddHHmmss để tránh trùng tên

            // tạo đg dẫn lưu file
            var folderPath = Path.Combine(serverPath, "Images");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var fullPath = Path.Combine(folderPath, unique);
            ImageFile.SaveAs(fullPath);

            return "~/Images/" + unique;
        }
    }
}
