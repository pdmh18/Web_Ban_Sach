using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Web_Ban_Sach.Models
{
    public class Supplier
    {
        [Key]
        public int Id { get; set; } // ma nha cung cap

        [Required(ErrorMessage = "Vui lòng nhập tên nhà cung cấp: ")]
        [StringLength(200)]
        public string Name { get; set; } // ten nha cung cap


        [Required(ErrorMessage = "Vui lòng nhập số điện thoại liên hệ: ")]
        [StringLength(200)]
        public string ContactPhone { get; set; } // so dien thoai nha cung cap


        [Required(ErrorMessage = "Vui lòng nhập địa chỉ: ")]
        [StringLength(200)]
        public string Address { get; set; } // dia chi nha cung cap
    }
}