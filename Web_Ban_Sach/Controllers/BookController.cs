
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Ban_Sach.Models;

namespace Web_Ban_Sach.Controllers
{
    public class BookController : Controller
    {
        private Books db = new Books();
        public ActionResult Index(string SortBy, int page = 1, string search = "")
        {

            List<Book> books = db.Book.OrderByDescending(b => b.Id).ToList();
            switch (SortBy)
            {
                case "Name":
                    books = books.OrderBy(e => e.Name).ToList();
                    break;
                case "Price":
                    books = books.OrderBy(e => e.Price).ToList();
                    break;
                case "PublicationDate":
                    books = books.OrderBy(e => e.PublicationDate).ToList();
                    break;
                default:
                    break;

            }

            int pageSize = 3;
            int totalBooks = books.Count;
            int totalPages = (int)Math.Ceiling((double)totalBooks / pageSize);
            books = books.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = totalPages;

            if (!string.IsNullOrEmpty(search))
            {
                books = db.Book.Where(row => row.Name.Contains(search)).ToList();
            }
            ViewBag.Search = search;
            return View(books);
        }

        // them
        private const string PendingBooksSessionKey = "PendingBooks";// dùng key để truy cạp sesioon, thay viết viết PenPendingBooks thì gom thành 1 PendingBooksSessionKey
        // kho tạm để lưu sách chờ thêm vào db
        private List<Book> GetPendingBooks()
        {
            var list = Session[PendingBooksSessionKey] as List<Book>;// lấy dưới dạng PendingBooksSessionKey chuyển qua dạng  List<Book>
            if (list == null)
            {
                list = new List<Book>();
                Session[PendingBooksSessionKey] = list;// gán lại vào session
            }
            return list;
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View(new BookDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.PublicationDate == null)
            {
                ModelState.AddModelError("", "Ngày/tháng/năm xuất bản không hợp lệ.");
                return View(model);
            }


            string imagePath;
            try
            {
                imagePath = model.SaveImage(Server.MapPath("~/"));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi lưu ảnh" + ex.Message);
                return View(model);
            }

            var book = new Book
            {
                Name = model.Name,
                PublicationDate = (DateTime)model.PublicationDate,
                Description = model.Description,
                Price = (double)model.Price,
                CoverImageUrl = imagePath,
                CreatedAt = DateTime.Now
            };

            var pending = GetPendingBooks();
            pending.Add(book);

            return RedirectToAction("PendingBooks");
        }

        // list them sach
        public ActionResult PendingBooks()// hiển thị all ds chờ thêm
        {
            var pending = GetPendingBooks();
            return View(pending);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PendingBooks(List<Book> booksDummy = null)
        {
            var pending = Session[PendingBooksSessionKey] as List<Book>;

            if (pending == null || !pending.Any())
            {
                return RedirectToAction("PendingBooks");
            }

            foreach (var b in pending)
            {
                db.Book.Add(b);
            }

            db.SaveChanges();
            Session[PendingBooksSessionKey] = null;

            return RedirectToAction("Index");
        }




        // update
        [HttpGet]
        public ActionResult Update(int id)
        {
            var b = db.Book.FirstOrDefault(x => x.Id == id);
            if (b == null) return HttpNotFound();

            var dto = new BookDto
            {
                Name = b.Name,
                Day = b.PublicationDate.Day,
                Month = b.PublicationDate.Month,
                Year = b.PublicationDate.Year,
                Description = b.Description,
                Price = b.Price
            };

            ViewBag.BookId = id; // giữ id riêng
            ViewBag.ExistingCover = b.CoverImageUrl;  // ảnh hiện tại để fallback
            return View(dto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(int id, BookDto model, string existingCover)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.BookId = id;
                ViewBag.ExistingCover = existingCover;
                return View(model);
            }

            var book = db.Book.FirstOrDefault(x => x.Id == id);
            if (book == null) return HttpNotFound();

            if (model.PublicationDate == null)
            {
                ModelState.AddModelError("", "Ngày/tháng/năm xuất bản không hợp lệ.");
                ViewBag.BookId = id;
                ViewBag.ExistingCover = existingCover;
                return View(model);
            }

            string imagePath;
            try
            {
                if (model.ImageFile != null && model.ImageFile.ContentLength > 0)
                    imagePath = model.SaveImage(Server.MapPath("~/"));
                else
                    imagePath = string.IsNullOrWhiteSpace(existingCover) ? "~/Images/anhmacdinh.jpg" : existingCover;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi lưu ảnh: " + ex.Message);
                ViewBag.BookId = id;
                ViewBag.ExistingCover = existingCover;
                return View(model);
            }

            // cập nhật
            book.Name = model.Name;
            book.PublicationDate = model.PublicationDate.Value;
            book.Price = model.Price.GetValueOrDefault();
            book.Description = model.Description;
            book.CoverImageUrl = imagePath;

            db.SaveChanges();
            return RedirectToAction("Index");
        }


        //delete
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var book = db.Book.Find(id);
            if (book == null)
                return HttpNotFound();

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var book = db.Book.Find(id);
            if (book == null)
                return HttpNotFound();

            try
            {
                db.Book.Remove(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = (ex.InnerException?.Message ?? ex.Message);
                return RedirectToAction("Index");
            }
        }

    }
}
