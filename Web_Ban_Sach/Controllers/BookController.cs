using System;
using System.Collections.Generic;
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
            List<Book> books = db.Book.ToList();
            return View(books);
        }

        // them
        [HttpGet]
        public ActionResult Create()
        {
            return View(new BookDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookDto model)
        {
            if(!ModelState.IsValid)
            {   
                return View(model);
            }
            if (model.PublicationDate == null)
            {
                ModelState.AddModelError("", "Ngày xuất bản không hợp lệ!");
                return View(model);
            }
            string savedRelativePath = null;
            if (model.ImageFile != null && model.ImageFile.ContentLength > 0)
            {
                // kiểm tra extension (chỉ cho phép ảnh)
                var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var ext = Path.GetExtension(model.ImageFile.FileName)?.ToLower();
                if (ext == null || Array.IndexOf(allowed, ext) < 0)
                {
                    ModelState.AddModelError("ImageFile", "Chỉ cho phép file ảnh (.jpg, .png, .jpeg, .gif).");
                    return View(model);
                }

                var fileName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
                var safeFileName = fileName.Length > 40 ? fileName.Substring(0, 40) : fileName; // tránh tên quá dài
                var unique = safeFileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ext;

                var folderPath = Server.MapPath("~/Images/");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var fullPath = Path.Combine(folderPath, unique);
                model.ImageFile.SaveAs(fullPath);

                savedRelativePath = "~/Images/" + unique;
            }

            var book = new Book
            {
                Name = model.Name,
                PublicationDate = model.PublicationDate.Value, 
                Description = model.Description,
                Price = model.Price,
                // nếu DB cột CoverImageUrl không cho NULL, đặt giá trị mặc định:
                CoverImageUrl = savedRelativePath ?? "~/Images/default.jpg",
                CreatedAt = DateTime.Now
            };


            try
            {
                db.Book.Add(book);
                db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException vex)
            {
                foreach (var eve in vex.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        ModelState.AddModelError(ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException?.InnerException?.Message ?? ex.Message;
                ModelState.AddModelError("", "Lỗi khi lưu dữ liệu: " + inner);
                return View(model);
            }
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