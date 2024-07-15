using ECommerce.DataAccess.Repository.IRepository;
using eCommerceWebApp.Data;
using eCommerceWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccess.Repository
{
    internal class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product p)
        {
            var formDB = _db.products.FirstOrDefault(u => u.Product_Id == p.Product_Id);
            if (formDB != null)
            {
                formDB.Product_Name = p.Product_Name;
                formDB.Description = p.Description;
                formDB.CategoryId = p.CategoryId;
                formDB.Price = p.Price;
                formDB.StockQuantity = p.StockQuantity;
                formDB.SKU = p.SKU;
                if (p.ImageUrl != null)
                {
                    formDB.ImageUrl = p.ImageUrl;
                }
            }
        }
    }
}
