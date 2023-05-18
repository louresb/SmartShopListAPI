using Microsoft.AspNetCore.Mvc;
using ShoppingListApp.Data;
using ShoppingListApp.Models;

namespace ShoppingListApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public ProductController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var products = _dbContext.Products.ToList();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = _dbContext.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        public IActionResult CreateProduct(Product product)
        {
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, Product updatedProduct)
        {
            var product = _dbContext.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound();

            product.Name = updatedProduct.Name;
            product.Price = updatedProduct.Price;

            _dbContext.SaveChanges();

            return Ok("Product updated successfully.");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _dbContext.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound();

            _dbContext.Products.Remove(product);
            _dbContext.SaveChanges();

            return NoContent();
        }
    }
}
