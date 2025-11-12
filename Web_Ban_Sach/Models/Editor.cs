using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Web_Ban_Sach.Models
{
    public class Editor
    {
        [Key]
        public int Id { get; set; }                     // mã phiên bản

        [Range(1, int.MaxValue)]
        public int EditionNumber { get; set; }          // lần tái bản thứ ...

        [StringLength(50)]
        public string Language { get; set; }            // ngôn ngữ

        [Range(1, int.MaxValue)]
        public int Pages { get; set; }                  // số trang

        [StringLength(200)]
        public string Publisher { get; set; }
    }
}