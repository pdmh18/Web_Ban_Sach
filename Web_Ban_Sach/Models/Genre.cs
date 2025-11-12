using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Web_Ban_Sach.Models
{
    public class Genre
    {
        [Key]
        public int Id { get; set; } // ma the loai

        [Required(ErrorMessage = "Vui lòng nhập tên thể loại: ")]
        [StringLength(200)]
        public string Name { get; set; } // ten the loai

        public string Description { get; set; }

    }
}