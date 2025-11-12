using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Web_Ban_Sach.Models
{
    public class GenreDto
    {
        

        [Required(ErrorMessage = "Vui lòng nhập tên thể loại")]
        [StringLength(200, ErrorMessage = "Tên thể loại không được vượt quá 200 ký tự")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Mô tả thể loại không được vượt quá 500 ký tự")]
        public string Description { get; set; }
    }
}