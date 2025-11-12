using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Web_Ban_Sach.Models
{
    public class EditorDto
    {
        

        [Required(ErrorMessage = "Vui lòng nhập số lần tái bản")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lần tái bản phải lớn hơn 0")]
        public int EditionNumber { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập ngôn ngữ")]
        [StringLength(50, ErrorMessage = "Ngôn ngữ không được vượt quá 50 ký tự")]
        public string Language { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số trang")]
        [Range(1, int.MaxValue, ErrorMessage = "Số trang phải lớn hơn 0")]
        public int Pages { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên nhà xuất bản")]
        [StringLength(200, ErrorMessage = "Tên nhà xuất bản không được vượt quá 200 ký tự")]
        public string Publisher { get; set; }
    }
}