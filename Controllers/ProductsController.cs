using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MYSQLStoreAPI.Models;

namespace MYSQLStoreAPI.Controllers
{   
    // [ApiExplorerSettings(IgnoreApi = true)] //ซ่อน path
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController:ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context){
            _context = context;
        }

        [HttpGet]
        [Route("all")]
         public ActionResult<Products> getAll(){
            // var products_all = _context.products
            //                    .Where(p=>p.Category_id != 0)
            //                    .OrderByDescending(p=>p.Product_unitprice)
            //                    .Take(2)
            //                    .ToList();

             var products_all = (from products in _context.products
                                join category in _context.category
                                on products.Category_id equals category.Category_id
                                where category.Category_status == 1
                                orderby products.Product_unitprice descending
                                select new {
                                    products.Product_name,
                                    products.Category_FK.Category_name,
                                    products.Category_FK.Category_status,
                                    products.Product_unitprice,
                                    products.Product_unit_stock,
                                    products.Product_picture,
                                    products.Product_created_Date,
                                    products.ModifiedDate,
                                }
                                ).ToList();
                                
            return Ok(products_all);
        }

        [HttpGet]
        [Route("{id}")]

        public ActionResult GetById(int id) {
            
            
            var products_ById = _context.products.Where(c => c.Product_id == id);
            
            if(products_ById == null)
                return NotFound();

            return Ok(products_ById);
        }


        [HttpPost]
        public ActionResult Create(Products products){

            _context.products.Add(products);
            _context.SaveChanges();

            return Ok(products);
            
        }

        [HttpPut]
        public ActionResult Update(Products products){

            if(products == null)
                return NotFound();

            _context.Update(products);
            _context.SaveChanges();

            return Ok(products);
            
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public ActionResult Delete(int id){

            var products_delete = _context.products.Where(c=>c.Product_id == id).FirstOrDefault();

            if(products_delete == null)
                return NotFound();

            _context.Remove(products_delete);
            _context.SaveChanges();

            return NoContent();
            
        }

        
        
    }
}