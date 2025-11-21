using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Ban_Sach.Models;
using System.Data.Entity;

namespace Web_Ban_Sach.Controllers
{
    public class HomeController : Controller
    {
        private Books db = new Books();
        public ActionResult Index(string SortBy, int page = 1, string search = "", int? genreId = null)
        {
            var books = db.Book.AsQueryable();
            if (genreId.HasValue)
            {
                books = books.Where(b => b.genreId == genreId.Value);
            }

            // Tìm kiếm
            if (!string.IsNullOrEmpty(search))
            {
                books = books.Where(b => b.Name.Contains(search));
            }

            // Sắp xếp
            switch (SortBy)
            {
                case "IdAsc":
                    books = books.OrderBy(b => b.Id);
                    break;
                case "IdDesc":
                    books = books.OrderByDescending(b => b.Id);
                    break;
                case "Name":
                    books = books.OrderBy(b => b.Name);
                    break;
                case "Price":
                    books = books.OrderBy(b => b.Price);
                    break;
                case "PublicationDate":
                    books = books.OrderBy(b => b.PublicationDate);
                    break;
                default:
                    books = books.OrderByDescending(b => b.Id);
                    break;
            }

            int pageSize = 9;
            int totalBooks = books.Count();
            int totalPages = (int)Math.Ceiling((double)totalBooks / pageSize);

            var BOOK = books
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = totalPages;
            ViewBag.PageSize = pageSize;
            ViewBag.Search = search;
            ViewBag.CurrentSort = SortBy;
            ViewBag.CurrentGenreId = genreId;
            return View(BOOK);
        }

        [ChildActionOnly]
        public PartialViewResult GenreSidebar()
        {
            var genres = db.Genre
                   .OrderBy(g => g.Name)
                   .ToList();

            return PartialView("GenreSidebar", genres);
        }

        public ActionResult Details(int id)
        {
            var book = db.Book
                         .Include(b => b.Genre)
                         .Include(b => b.Supplier)
                         .Include(b => b.Editor)
                         .Include(b => b.BookAuthors.Select(ba => ba.Author)) 
                         .FirstOrDefault(b => b.Id == id);

            if (book == null) return HttpNotFound();

            return View(book);
        }

    }
}