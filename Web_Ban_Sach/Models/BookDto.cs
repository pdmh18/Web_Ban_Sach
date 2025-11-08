using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web_Ban_Sach.Models
{
    public class BookDto
    {
        [Required(ErrorMessage = "Vui lòng nhập tên sách")]
        [StringLength(200)]
        public string Name { get; set; } // ten sach

        [Required(ErrorMessage = "Vui lòng nhập ngày xuất bản")]
        [Range(1, 31, ErrorMessage = "Ngày không hợp lệ")]
        public int Day { get; set; } // ngay xuat ban

        [Required(ErrorMessage = "Vui lòng nhập tháng xuất bản")]
        [Range(1, 12, ErrorMessage = "Tháng không hợp lệ")]
        public int Month { get; set; } // thang xuat ban

        [Required(ErrorMessage = "Vui lòng nhập năm xuất bản")]
        [Range(1900, 2100, ErrorMessage = "Năm không hợp lệ")]
        public int Year { get; set; } // nam xuat ban

        public DateTime? PublicationDate
        {
            get
            {
                try
                {
                    return new DateTime(Year, Month, Day);
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
        public double Price { get; set; } // gia sach

        public HttpPostedFileBase ImageFile { get; set; } 
    }
}