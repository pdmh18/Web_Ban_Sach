using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Ban_Sach.Models;

namespace Web_Ban_Sach.Controllers
{
    public class SupplierController : Controller
    {
        private Books db = new Books();

        // GET: Supplier
        public ActionResult Index()
        {
            var suppliers = db.Supplier.ToList();
            return View(suppliers);
        }

        // GET: Supplier/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Supplier/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SupplierDto model)
        {
            if (ModelState.IsValid)
            {
                var supplier = new Supplier
                {
                    Name = model.Name,
                    ContactPhone = model.ContactPhone,
                    Address = model.Address
                };
                // Lưu thể loại vào danh sách tạm thời trong Session
                var pendingSupplier = GetPendingSuppliers();  // Lấy danh sách thể loại tạm thời từ Session
                pendingSupplier.Add(supplier);  // Thêm thể loại mới vào danh sách tạm thời

                // Lưu lại danh sách vào Session
                Session[PendingSuppliersSessionKey] = pendingSupplier;
                return RedirectToAction("PendingSupplier");
            }

            return View(model);
        }

        private const string PendingSuppliersSessionKey = "PendingSupplier";
        private List<Supplier> GetPendingSuppliers()
        {
            var list = Session[PendingSuppliersSessionKey] as List<Supplier>;
            if (list == null)
            {
                list = new List<Supplier>();
                Session[PendingSuppliersSessionKey] = list;
            }
            return list;
        }

        public ActionResult PendingSupplier()
        {
            var pendingSuppliers = GetPendingSuppliers();
            return View(pendingSuppliers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PendingSupplier(List<Supplier> suppliersDummy = null)
        {
            var supplier = GetPendingSuppliers();
            if (supplier == null || !supplier.Any())
            {
                return RedirectToAction("PendingSupplier");
            }
            foreach (var s in supplier)
            {
                db.Supplier.Add(s);
            }

            db.SaveChanges();
            Session[PendingSuppliersSessionKey] = null; // Xóa danh sách tạm thời sau khi lưu
            return RedirectToAction("Index");
        }





        public ActionResult Edit(int id)
        {
            var supplier = db.Supplier.Find(id);
            if (supplier == null) return HttpNotFound();

            var model = new SupplierDto
            {
                Name = supplier.Name,
                ContactPhone = supplier.ContactPhone,
                Address = supplier.Address
            };

            return View(model);
        }

        // POST: Supplier/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, SupplierDto model)
        {
            if (ModelState.IsValid)
            {
                var supplier = db.Supplier.Find(id);
                if (supplier == null) return HttpNotFound();

                supplier.Name = model.Name;
                supplier.ContactPhone = model.ContactPhone;
                supplier.Address = model.Address;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(model);
        }

  
 
        public ActionResult Delete(int? id)
        {
            var supplier = db.Supplier.Find(id); 
            if (supplier == null) return HttpNotFound();  

            return View(supplier);  
        }

        // POST: Supplier/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var supplier = db.Supplier.Find(id);  
            if (supplier == null) return HttpNotFound(); 

            db.Supplier.Remove(supplier); 
            db.SaveChanges();  

            return RedirectToAction("Index");  
        }

    }
}