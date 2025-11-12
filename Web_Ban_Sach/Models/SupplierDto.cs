using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Web_Ban_Sach.Models
{
    public class SupplierDto
    {
        

        [Required(ErrorMessage = "Vui lòng nhập tên nhà cung cấp")]
        [StringLength(200, ErrorMessage = "Tên nhà cung cấp không được vượt quá 200 ký tự")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại liên hệ")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string ContactPhone { get; set; }

        [StringLength(500, ErrorMessage = "Địa chỉ không được vượt quá 500 ký tự")]
        public string Address { get; set; }
    }
}