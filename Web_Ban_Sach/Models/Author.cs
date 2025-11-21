using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Web_Ban_Sach.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }    
        

        [Required(ErrorMessage = "Vui lòng nhập tên tác giả: ")]
        [StringLength(200)]
        public string Name { get; set; }                // tên tác giả


        [Required(ErrorMessage = "Vui lòng nhập tiểu sử tác giả: ")]
        [StringLength(200)]
        public string Bio{ get; set; }           // tiểu sử tác giả

        [DataType(DataType.Date)]
        [Display(Name = "Ngày sinh")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime BirthDate { get; set; }        // ngày sinh

        [Required(ErrorMessage = "Vui lòng nhập quốc tịch: ")]
        [StringLength(200)]
        public string Nationality { get; set; }          // quốc tịch



        public virtual ICollection<BookAuthor> BookAuthors { get; set; }
    }
}