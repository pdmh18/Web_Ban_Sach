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
        public ActionResult Index()
        {
            List<Book> books = db.Book.OrderByDescending(b => b.Id).ToList();
            return View(books);
        }

        // them
        private const string PendingBooksSessionKey = "PendingBooks";

        private List<Book> GetPendingBooks()
        {
            var list = Session[PendingBooksSessionKey] as List<Book>;
            if (list == null)
            {
                list = new List<Book>();
                Session[PendingBooksSessionKey] = list;
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

            string imagePath;
            try
            {
                imagePath = model.SaveImage(Server.MapPath("~/"));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("","Lỗi lưu ảnh" + ex.Message);
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

            TempData["Message"] = "Đã thêm sách vào danh sách chờ.";
            return RedirectToAction("PendingBooks");
        }

        // list them sach
        public ActionResult PendingBooks()
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
                TempData["Error"] = "Không có sách nào trong danh sách chờ.";
                return RedirectToAction("PendingBooks");
            }

            foreach (var b in pending)
            {
                db.Book.Add(b);
            }

            db.SaveChanges();
            Session[PendingBooksSessionKey] = null;

            TempData["Message"] = "Đã thêm tất cả sách vào hệ thống.";
            return RedirectToAction("Index");
        }




        // update
        public ActionResult Update(int id) // tìm sachhs trong bảng db
        {
            Book existingBook = db.Book.FirstOrDefault(b => b.Id == id);
            if (existingBook == null)
            {
                return HttpNotFound();
            }

            return View(existingBook);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Book model, HttpPostedFileBase ImageFile)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Book existingBook = db.Book.FirstOrDefault(b => b.Id == model.Id);
            if (existingBook == null)
            {
                return HttpNotFound();
            }

            // Gán dữ liệu mới
            existingBook.Name = model.Name;
            existingBook.PublicationDate = model.PublicationDate;
            existingBook.Price = model.Price;
            existingBook.Description = model.Description;

            // Nếu người dùng chọn ảnh mới, thì lưu ảnh và cập nhật đường dẫn
            if (ImageFile != null && ImageFile.ContentLength > 0)
            {
                var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var ext = Path.GetExtension(ImageFile.FileName)?.ToLower();

                if (ext == null || !allowed.Contains(ext))
                {
                    ModelState.AddModelError("ImageFile", "Chỉ cho phép file ảnh (.jpg, .png, .jpeg, .gif)");
                    return View(model);
                }

                // Tạo tên file duy nhất
                var fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                var safeFileName = fileName.Length > 40 ? fileName.Substring(0, 40) : fileName;
                var uniqueName = safeFileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ext;

                var folder = Server.MapPath("~/Images/");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                var fullPath = Path.Combine(folder, uniqueName);
                ImageFile.SaveAs(fullPath);

                existingBook.CoverImageUrl = "~/Images/" + uniqueName;
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //delete
        // GET: Book/Delete/5
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
                TempData["Error"] = "Lỗi khi xóa sách: " + (ex.InnerException?.Message ?? ex.Message);
                return RedirectToAction("Index");
            }
        }

    }
}