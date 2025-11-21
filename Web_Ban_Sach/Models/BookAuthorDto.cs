using System.ComponentModel.DataAnnotations;

namespace Web_Ban_Sach.Models
{
    public class BookAuthorDto
    {
        public int Id { get; set; } 

        [Required(ErrorMessage = "Vui lòng chọn sách.")]
        [Display(Name = "Sách")]
        public int BookId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn tác giả.")]
        [Display(Name = "Tác giả")]
        public int AuthorId { get; set; }
    }
}
