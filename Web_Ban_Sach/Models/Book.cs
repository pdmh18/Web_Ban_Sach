using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
namespace Web_Ban_Sach.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; } // ma sach



        [Required(ErrorMessage = "Vui lòng nhập tên sách: ")]
        [StringLength(200)]
        public string Name { get; set; } // ten sach




        [DataType(DataType.Date)]
        [Display(Name = "Ngày xuất bản")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]

        public DateTime PublicationDate { get; set; } // ngay xuat ban




        [Display(Name = "Mô tả")]
        public string Description { get; set; } // mo ta sach



        [Required(ErrorMessage = "Vui lòng nhập giá sách")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải >= 0")]
        [Display(Name = "Giá")]
        [DisplayFormat(DataFormatString = "{0:N0} đ", ApplyFormatInEditMode = false)]
        public double Price { get; set; }




        [Display(Name = "Ảnh bìa")]
        public string CoverImageUrl { get; set; } // anh bia sach



        [DataType(DataType.Date)]
        [Display(Name = "Ngày thêm vào hệ thống")]
        public DateTime CreatedAt { get; set; } // ngay them sach vao he thong



        [Display(Name = "Thể loại")]
        [Required(ErrorMessage = "Vui lòng chọn thể loại sách")]
        public int? genreId { get; set; }
        [ForeignKey(nameof(genreId))]
        public virtual Genre Genre { get; set; }



        [Display(Name = "Nhà cung cấp")]
        [Required(ErrorMessage = "Vui lòng chọn nhà cung cấp")]
        public int? supplierId { get; set; }
        [ForeignKey(nameof(supplierId))]
        public virtual Supplier Supplier { get; set; }


        [Display(Name = "Phiên bản")]
        [Required(ErrorMessage = "Vui lòng chọn phiên bản sách")]
        public int? EditorId { get; set; }
        [ForeignKey(nameof(EditorId))]
        public virtual Editor Editor { get; set; }



        



        public virtual ICollection<BookAuthor> BookAuthors { get; set; }

        // DÙNG RIÊNG CHO PENDING (không map xuống DB)
        [NotMapped]
        public string TempAuthorName { get; set; }
    }
}