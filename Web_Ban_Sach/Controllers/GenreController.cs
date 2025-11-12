using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Ban_Sach.Models;

namespace Web_Ban_Sach.Controllers
{
    public class GenreController : Controller
    {
        private Books db = new Books();

        // GET: Genre
        public ActionResult Index()
        {
            var genres = db.Genre.ToList();
            return View(genres);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GenreDto model)
        {
            if (ModelState.IsValid)
            {
                var genre = new Genre
                {
                    Name = model.Name,
                    Description = model.Description
                };
                // Lưu thể loại vào danh sách tạm thời trong Session
                var pendingGenres = GetPendingGenres();  // Lấy danh sách thể loại tạm thời từ Session
                pendingGenres.Add(genre);  // Thêm thể loại mới vào danh sách tạm thời

                // Lưu lại danh sách vào Session
                Session[PendingGenresSessionKey] = pendingGenres;

                return RedirectToAction("PendingGenre");
            }

            return View(model);
        }
        private const string PendingGenresSessionKey = "PendingGenre";// dùng key để truy cạp sesioon, thay viết viết PenPendingBooks thì gom thành 1 PendingBooksSessionKey
        private List<Genre> GetPendingGenres()
        {
            var list = Session[PendingGenresSessionKey] as List<Genre>;  // Lấy từ Session
            if (list == null)
            {
                list = new List<Genre>();  // Tạo mới nếu chưa có thể loại nào trong Session
                Session[PendingGenresSessionKey] = list;  // Lưu lại vào Session
            }
            return list;
        }

        public ActionResult PendingGenre()// hiển thị all ds chờ thêm
        {
            var pending = GetPendingGenres();
            return View(pending);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PendingGenre(List<Genre> genresDummy = null)
        {
            var pending = Session[PendingGenresSessionKey] as List<Genre>;

            if (pending == null || !pending.Any())
            {
                return RedirectToAction("PendingGenre");
            }

            foreach (var g in pending)
            {
                db.Genre.Add(g);
            }

            db.SaveChanges();
            Session[PendingGenresSessionKey] = null;

            return RedirectToAction("Index");
        }





        public ActionResult Edit(int id)
        {
            var genre = db.Genre.Find(id);
            if (genre == null) return HttpNotFound();

            var model = new GenreDto
            {
                Name = genre.Name,
                Description = genre.Description
            };
            ViewBag.GenreId = id;
            return View(model);
        }

        // POST: Genre/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, GenreDto model)
        {
            if (ModelState.IsValid)
            {
                var genre = db.Genre.Find(id);
                if (genre == null) return HttpNotFound();

                genre.Name = model.Name;
                genre.Description = model.Description;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(model);
        }

     
        public ActionResult Delete(int? id)
        {
            var genre = db.Genre.Find(id);  
            if (genre == null) return HttpNotFound();  

            return View(genre);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var genre = db.Genre.Find(id);  // tim id
            if (genre == null) return HttpNotFound(); 

            db.Genre.Remove(genre);  // Xóa thể loại khỏi DB
            db.SaveChanges();  // Lưu thay đổi vào cơ sở dữ liệu

            return RedirectToAction("Index");
        }
    }
}