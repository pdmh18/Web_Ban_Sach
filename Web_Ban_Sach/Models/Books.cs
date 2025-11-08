using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Web_Ban_Sach.Models
{
    public class Books : DbContext
    {
        public Books() : base("My_QLS")
        {
        }
        public DbSet<Book> Book { get; set; }
        
    }
}