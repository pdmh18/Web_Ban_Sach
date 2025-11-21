using System;
using System.ComponentModel.DataAnnotations;

namespace Web_Ban_Sach.Models
{
    public class AuthorDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên tác giả.")]
        [StringLength(200)]
        [Display(Name = "Tên tác giả")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tiểu sử tác giả.")]
        [StringLength(200)]
        [Display(Name = "Tiểu sử")]
        public string Bio { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày sinh.")]
        [DataType(DataType.Date)]
        [Display(Name = "Ngày sinh")]
        // Nullable để nếu user không chọn thì ModelState sẽ bắt lỗi
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? BirthDate { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập quốc tịch.")]
        [StringLength(200)]
        [Display(Name = "Quốc tịch")]
        public string Nationality { get; set; }
    }
}
