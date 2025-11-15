
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Ban_Sach.Models;
using System.IO;
using System.Data.Entity;

namespace Web_Ban_Sach.Controllers
{
    public class BookController : Controller
    {
        private Books db = new Books();

        public ActionResult Index(string SortBy, int page = 1, string search = "")
        {

            List<Book> books = db.Book.OrderByDescending(b => b.Id).ToList();
            if (!string.IsNullOrEmpty(search))
            {
                books = db.Book.Where(row => row.Name.Contains(search)).ToList();
            }

            int pageSize = 3;
            int totalBooks = books.Count;
            int totalPages = (int)Math.Ceiling((double)totalBooks / pageSize);
            books = books.Skip((page - 1) * pageSize).Take(pageSize).ToList();// lm tròn
            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = totalPages;// hiện thi số phân trang

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

           

           
            ViewBag.Search = search;
            return View(books);
        }
       






        // them
        // cho nó tcaapj nhatajk trên giao diện ko cho xử lí trong controller. bảng tạm treengiao diện
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
            ViewBag.Genres = db.Genre.ToList();
            ViewBag.Suppliers = db.Supplier.ToList();
            ViewBag.Editors = db.Editor.ToList();
            return View(new BookDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookDto model)
        {
            if (!ModelState.IsValid)
            { 
                ViewBag.Genres = db.Genre.ToList();
                ViewBag.Suppliers = db.Supplier.ToList();
                ViewBag.Editors = db.Editor.ToList();
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
                ViewBag.Genres = db.Genre.ToList();
                ViewBag.Suppliers = db.Supplier.ToList();
                ViewBag.Editors = db.Editor.ToList();
                return View(model);
            }

            var book = new Book
            {
                Name = model.Name,
                PublicationDate = (DateTime)model.PublicationDate,
                Description = model.Description,
                Price = (double)model.Price,
                CoverImageUrl = imagePath,
                CreatedAt = DateTime.Now,
                genreId = model.GenreId,
                supplierId = model.SupplierId,
                EditorId = model.EditorId

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

        // xóa ảnh trogn thư mục
        private const string DefaultImage = "~/Images/anhmacdinh.jpg";
        private void XoaImageThuMuc (string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath) || imagePath.Equals(DefaultImage, StringComparison.OrdinalIgnoreCase))
                return;


            // Chuyển đổi đường dẫn ảo thành đường dẫn vật lý
            var physical = Server.MapPath(imagePath);

            // Kiểm tra nếu file tồn tại và xóa
            if (System.IO.File.Exists(physical))
            {
                System.IO.File.Delete(physical);
            }
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
                Price = b.Price,
                GenreId = b.genreId,
                SupplierId = b.supplierId,
                EditorId = b.EditorId
            };
            ViewBag.Genres = db.Genre.ToList();
            ViewBag.Suppliers = db.Supplier.ToList();
            ViewBag.Editors = db.Editor.ToList();

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
                ViewBag.Genres = db.Genre.ToList();
                ViewBag.Suppliers = db.Supplier.ToList();
                ViewBag.Editors = db.Editor.ToList();
                return View(model);
            }

            var book = db.Book.FirstOrDefault(x => x.Id == id);
            if (book == null) return HttpNotFound();

            if (model.PublicationDate == null)
            {
                ModelState.AddModelError("", "Ngày/tháng/năm xuất bản không hợp lệ.");
                ViewBag.BookId = id;
                ViewBag.ExistingCover = existingCover;
                ViewBag.Genres = db.Genre.ToList();
                ViewBag.Suppliers = db.Supplier.ToList();
                ViewBag.Editors = db.Editor.ToList();
                return View(model);
            }

            try
            {
                if (model.ImageFile != null && model.ImageFile.ContentLength > 0)
                {
                    var oldCover = book.CoverImageUrl; // giữ lại
                    var newCover = model.SaveImage(Server.MapPath("~/")); // lưu ảnh mới

                    book.CoverImageUrl = newCover; // gán mới
                    XoaImageThuMuc(oldCover); // xóa cũ
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(book.CoverImageUrl))
                        book.CoverImageUrl = DefaultImage;
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi lưu ảnh: " + ex.Message);
                ViewBag.BookId = id;
                ViewBag.ExistingCover = existingCover;
                ViewBag.Genres = db.Genre.ToList();
                ViewBag.Suppliers = db.Supplier.ToList();
                ViewBag.Editors = db.Editor.ToList();

                return View(model);
            }

            // cập nhật
            book.Name = model.Name;
            book.PublicationDate = model.PublicationDate.Value;
            book.Price = model.Price.GetValueOrDefault();
            book.Description = model.Description;
            book.genreId = model.GenreId.GetValueOrDefault();
            book.supplierId = model.SupplierId.GetValueOrDefault();
            book.EditorId = model.EditorId.GetValueOrDefault();

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
                var cover = book.CoverImageUrl;
                db.Book.Remove(book);
                db.SaveChanges();          // xóa DB trước cho chắc
                XoaImageThuMuc(cover); // rồi xóa file ảnh
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = (ex.InnerException?.Message ?? ex.Message);
                return RedirectToAction("Index");
            }
        }

        ///////////////////////////////////////////
        [HttpPost]
        public ActionResult DeleteSelected(List<int> selectedBooks)
        {
            if (selectedBooks != null && selectedBooks.Any())
            {
                var booksToDelete = db.Book.Where(b => selectedBooks.Contains(b.Id)).ToList();

                // Lưu ID của các sách bị xóa vào Session
                List<int> deletedBookIds = booksToDelete.Select(b => b.Id).ToList();
                Session["DeletedBooks"] = deletedBookIds;

                // Xóa các sách khỏi cơ sở dữ liệu
                foreach (var book in booksToDelete)
                {
                    db.Book.Remove(book); // Xóa sách khỏi cơ sở dữ liệu
                }

                db.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu

                TempData["Message"] = "Sách đã bị xóa. Bạn có thể hoàn tác trong 1 phút.";

                return RedirectToAction("Index"); // Quay lại danh sách sách
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult UndoDelete()
        {
            // Lấy danh sách các sách bị xóa từ Session
            var deletedBookIds = Session["DeletedBooks"] as List<int>;

            if (deletedBookIds != null && deletedBookIds.Any())
            {
                // Khôi phục sách từ danh sách ID
                var booksToRestore = db.Book.Where(b => deletedBookIds.Contains(b.Id)).ToList();

                foreach (var book in booksToRestore)
                {
                    db.Book.Add(book); // Thêm sách vào cơ sở dữ liệu để phục hồi
                }

                db.SaveChanges();
            }
            Session["DeletedBooks"] = null;

            TempData["Message"] = "Sách đã được phục hồi.";

            return RedirectToAction("Index");
        }

    }
}
