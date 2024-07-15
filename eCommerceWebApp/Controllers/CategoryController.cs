using eCommerceWebApp.Data;
using eCommerceWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace eCommerceWebApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Category> categories = _db.categories.ToList();
            return View(categories);
        }

        public IActionResult Upsert(int? id)
        {
            Category c = new Category();
            if (id == null || id == 0)
            {
                //create
                return View(c);
            }
            else
            {
                //update
                Category? categoryID = _db.categories.FirstOrDefault(u => u.Category_Id == id);
                return View(categoryID);
            }
        }
        [HttpPost]
        public IActionResult Upsert(Category c)
        {
            if (ModelState.IsValid)
            {
                if (c.Category_Id == 0)
                {
                    _db.categories.Add(c);
                }
                else
                {
                    _db.categories.Update(c);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(c);
            }
            
        }

       

        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                var objFromDb = _db.categories.FirstOrDefault(u => u.Category_Id == id);
                return View(objFromDb);
            }
            return View();
        }

        [HttpPost,ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
                var obj = _db.categories.FirstOrDefault(u=>u.Category_Id == id);
                _db.categories.Remove(obj);
                _db.SaveChanges();
            return RedirectToAction("Index");
            
        }
    }
}
