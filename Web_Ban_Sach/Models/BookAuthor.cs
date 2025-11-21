using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_Ban_Sach.Models
{
    public class BookAuthor
    {
        [Key]
        public int Id { get; set; }      // khóa chính bảng trung gian

        [Required]
        [Display(Name = "Sách")]
        public int BookId { get; set; }  // FK tới Book

        [Required]
        [Display(Name = "Tác giả")]
        public int AuthorId { get; set; } // FK tới Author

        // Navigation
        [ForeignKey("BookId")]
        public virtual Book Book { get; set; }

        [ForeignKey("AuthorId")]
        public virtual Author Author { get; set; }
    }
}
