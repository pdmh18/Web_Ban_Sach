using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Web_Ban_Sach.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; } // ma sach

        [Required(ErrorMessage ="Vui lòng nhập tên sách: ")]
        [StringLength(200)]
        public string Name { get; set; } // ten sach

        [DataType(DataType.Date)]
        [Display(Name ="Ngày xuất bản")]
        public DateTime PublicationDate { get; set; } // ngay xuat ban

        [Display(Name = "Mô tả")]
        public string Description { get; set; } // mo ta sach

        [Required(ErrorMessage = "Vui lòng nhập giá sách")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải >= 0")]
        public double Price { get; set; } // gia sach

        [Display(Name = "Ảnh bìa")]
        public string CoverImageUrl { get; set; } // anh bia sach

        [DataType(DataType.Date)]
        [Display(Name = "Ngày thêm vào hệ thống")]
        public DateTime CreatedAt { get; set; } // ngay them sach vao he thong
    }
}