using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Ban_Sach.Models;

namespace Web_Ban_Sach.Controllers
{
    public class EditorController : Controller
    {
        private Books db = new Books();

        // GET: Editor
        public ActionResult Index()
        {
            var editors = db.Editor.ToList();
            return View(editors);
        }

        // GET: Editor/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EditorDto model)
        {
            if (ModelState.IsValid)
            {
                var editor = new Editor
                {
                    EditionNumber = model.EditionNumber,
                    Language = model.Language,
                    Pages = model.Pages,
                    Publisher = model.Publisher
                };
                var pendingEditor = GetPendingEditor();  // Lấy danh sách thể loại tạm thời từ Session
                pendingEditor.Add(editor);  // Thêm thể loại mới vào danh sách tạm thời

                // Lưu lại danh sách vào Session
                Session[PendingEditorsSessionKey] = pendingEditor;
                return RedirectToAction("PendingEditor");
            }

            return View(model);
        }


        private const string PendingEditorsSessionKey = "PendingEditor";
        private List<Editor> GetPendingEditor()
        {
            var list = Session[PendingEditorsSessionKey] as List<Editor>;
            if (list == null)
            {
                list = new List<Editor>();
                Session[PendingEditorsSessionKey] = list;
            }
            return list;
        }

        public ActionResult PendingEditor()
        {
            var pendingEditors = GetPendingEditor();
            return View(pendingEditors);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PendingEditor(List<Editor> editorsDummy = null)
        {
            var editors = GetPendingEditor();
            if (editors == null || !editors.Any())
            {
                return RedirectToAction("PendingEditor");
            }
            foreach (var e in editors)
            {
                db.Editor.Add(e);
            }

            db.SaveChanges();
            Session[PendingEditorsSessionKey] = null; // Xóa danh sách tạm thời sau khi lưu
            return RedirectToAction("Index");
        }

















        // GET: Editor/Edit/5
        public ActionResult Edit(int id)
        {
            var editor = db.Editor.Find(id);
            if (editor == null) return HttpNotFound();

            var model = new EditorDto
            {
                EditionNumber = editor.EditionNumber,
                Language = editor.Language,
                Pages = editor.Pages,
                Publisher = editor.Publisher
            };

            return View(model);
        }

        // POST: Editor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EditorDto model)
        {
            if (ModelState.IsValid)
            {
                var editor = db.Editor.Find(id);
                if (editor == null) return HttpNotFound();

                editor.EditionNumber = model.EditionNumber;
                editor.Language = model.Language;
                editor.Pages = model.Pages;
                editor.Publisher = model.Publisher;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(model);
        }


        public ActionResult Delete(int? id)
        {
            var editor = db.Editor.Find(id);
            if (editor == null) return HttpNotFound();

            return View(editor);
        }

        // POST: Supplier/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var editor = db.Editor.Find(id);
            if (editor == null) return HttpNotFound();

            db.Editor.Remove(editor);
            db.SaveChanges();

            return RedirectToAction("Index");
        }


    }
}