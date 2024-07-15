using eCommerceWebApp.Data;
using eCommerceWebApp.Models;
using eCommerceWebApp.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.SqlServer.Server;

namespace eCommerceWebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> products = _db.products.Include(p => p.Category).ToList();
            return View(products);
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productvm = new ProductVM()
            {
                CategoryList = _db.categories.Select(
                    u=> new SelectListItem
                    {
                        Text = u.CategoryName,
                        Value = u.Category_Id.ToString()
                    }
                    ).ToList(),
                Product = new Product()
            };

            if (id == null || id == 0)
            {
                //create
                return View(productvm);
            }
            else
            {
                //update
                productvm.Product = _db.products.FirstOrDefault(u => u.Product_Id == id);
                return View(productvm);
            }
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM p,IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images/product");
                    if (!string.IsNullOrEmpty(p.Product.ImageUrl))
                    {
                        //delete the old image 
                        var oldimagepath = Path.Combine(wwwRootPath, p.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldimagepath))
                        {
                            System.IO.File.Delete(oldimagepath);
                        }

                    }

                    using (var filestream = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    p.Product.ImageUrl = @"\images\product\" + filename;
                }

                if (p.Product.Product_Id == 0)
                {
                    p.Product.CreatedDate = DateTime.Now;
                    _db.products.Add(p.Product);
                }
                else
                {
                   
                    p.Product.UpdatedDate = DateTime.Now;
                    _db.products.Update(p.Product);
                    
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                p.CategoryList = _db.categories.Select(
               u => new SelectListItem
               {
                   Text = u.CategoryName,
                   Value = u.Category_Id.ToString()
               }
               ).ToList();
                return View(p);
            }
            
        }

       

        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                var objFromDb = _db.products.FirstOrDefault(u => u.Product_Id == id);
                return View(objFromDb);
            }
            return View();
        }

        [HttpPost,ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
                var obj = _db.products.FirstOrDefault(u=>u.Product_Id == id);
                _db.products.Remove(obj);
                _db.SaveChanges();
            return RedirectToAction("Index");
            
        }
    }
}
