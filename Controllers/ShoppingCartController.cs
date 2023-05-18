using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingListApp.Data;
using ShoppingListApp.DTOs;
using ShoppingListApp.Models;

namespace ShoppingListApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public ShoppingCartController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAllShoppingCarts()
        {
            var shoppingCarts = _dbContext.ShoppingCarts
                .Include(cart => cart.ShoppingCartProducts)
                .ThenInclude(scp => scp.Product)
                .ToList();

            var shoppingCartsWithCartPrice = shoppingCarts.Select(cart => new
            {
                cart.Id,
                cart.Name,
                ShoppingCartProducts = cart.ShoppingCartProducts.Select(scp => new
                {
                    scp.Quantity,
                    scp.Product.Id,
                    scp.Product.Name,
                    scp.Product.Price
                }).ToList(),
                CartPrice = cart.CalculateCartPrice()
            });

            return Ok(shoppingCartsWithCartPrice);
        }

        [HttpGet("{id}")]
        public IActionResult GetShoppingCart(int id)
        {
            var shoppingCart = _dbContext.ShoppingCarts
                .Include(cart => cart.ShoppingCartProducts)
                    .ThenInclude(scp => scp.Product)
                .FirstOrDefault(cart => cart.Id == id);

            if (shoppingCart == null)
                return NotFound();

            var shoppingCartWithDetails = new
            {
                shoppingCart.Id,
                shoppingCart.Name,
                ShoppingCartProducts = shoppingCart.ShoppingCartProducts.Select(scp => new
                {
                    scp.Quantity,
                    scp.Product.Id,
                    scp.Product.Name,
                    scp.Product.Price
                }).ToList(),
                CartPrice = shoppingCart.CalculateCartPrice()
            };

            return Ok(shoppingCartWithDetails);
        }

        [HttpPost]
        public IActionResult CreateShoppingCart([FromBody] ShoppingCartDto shoppingCartDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var shoppingCart = new ShoppingCart
            {
                Name = shoppingCartDto.Name,
                ShoppingListId = shoppingCartDto.ShoppingListId
            };

            _dbContext.ShoppingCarts.Add(shoppingCart);
            _dbContext.SaveChanges();

            return CreatedAtAction(nameof(GetShoppingCart), new { id = shoppingCart.Id }, shoppingCart);
        }

        public class AddProductToCartRequest
        {
            public int Quantity { get; set; }
        }

        [HttpPost("{cartId}/addproduct/{productId}")]
        public IActionResult AddProductToCart(int cartId, int productId, [FromBody] AddProductToCartRequest request)
        {
            int quantity = request.Quantity;

            var shoppingCart = _dbContext.ShoppingCarts
                .Include(sc => sc.ShoppingCartProducts)
                .ThenInclude(scp => scp.Product)
                .FirstOrDefault(sc => sc.Id == cartId);

            if (shoppingCart == null)
                return NotFound("Shopping cart not found.");

            var product = _dbContext.Products.FirstOrDefault(p => p.Id == productId);

            if (product == null)
                return NotFound("Product not found.");

            var existingProduct = shoppingCart.ShoppingCartProducts
                .FirstOrDefault(scp => scp.ProductId == productId);

            if (existingProduct != null)
            {
                existingProduct.Quantity += quantity;
            }
            else
            {
                var shoppingCartProduct = new ShoppingCartProduct
                {
                    ShoppingCartId = cartId,
                    ProductId = productId,
                    Quantity = quantity,
                    Product = product
                };

                shoppingCart.ShoppingCartProducts.Add(shoppingCartProduct);
            }

            _dbContext.SaveChanges();

            return Ok("Product added to the shopping cart successfully.");
        }

        public class UpdateShoppingCartNameRequest
        {
            public string Name { get; set; } = string.Empty;
        }

        public class UpdateProductQuantityRequest
        {
            public int Quantity { get; set; } = 0;
        }

        [HttpPut("{id}")]
        public IActionResult UpdateShoppingCartName(int id, [FromBody] UpdateShoppingCartNameRequest requestBody)
        {
            var shoppingCart = _dbContext.ShoppingCarts.FirstOrDefault(sc => sc.Id == id);

            if (shoppingCart == null)
                return NotFound("Shopping cart not found.");

            if (string.IsNullOrEmpty(requestBody?.Name))
                return BadRequest("Invalid request body.");

            shoppingCart.Name = requestBody.Name;
            _dbContext.SaveChanges();

            return Ok("Shopping cart name updated successfully.");
        }

        [HttpPut("{cartId}/updateproduct/{productId}")]
        public IActionResult UpdateProductQuantity(int cartId, int productId, [FromBody] UpdateProductQuantityRequest requestBody)
        {
            int newQuantity = requestBody?.Quantity ?? 0;

            var shoppingCart = _dbContext.ShoppingCarts
                .Include(sc => sc.ShoppingCartProducts)
                .FirstOrDefault(sc => sc.Id == cartId);

            if (shoppingCart == null)
                return NotFound("Shopping cart not found.");

            var shoppingCartProduct = shoppingCart.ShoppingCartProducts
                .FirstOrDefault(scp => scp.ProductId == productId);

            if (shoppingCartProduct == null)
                return NotFound("Product not found in the shopping cart.");

            shoppingCartProduct.Quantity = newQuantity;
            _dbContext.SaveChanges();

            return Ok("Product quantity updated successfully.");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteShoppingCart(int id)
        {
            var shoppingCart = _dbContext.ShoppingCarts.FirstOrDefault(sc => sc.Id == id);

            if (shoppingCart == null)
                return NotFound();

            _dbContext.ShoppingCarts.Remove(shoppingCart);
            _dbContext.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{cartId}/removeproduct/{productId}")]
        public IActionResult RemoveProductFromCart(int cartId, int productId)
        {
            var shoppingCart = _dbContext.ShoppingCarts
                .Include(sc => sc.ShoppingCartProducts)
                .FirstOrDefault(sc => sc.Id == cartId);

            if (shoppingCart == null)
                return NotFound("Shopping cart not found.");

            var shoppingCartProduct = shoppingCart.ShoppingCartProducts
                .SingleOrDefault(scp => scp.ProductId == productId);

            if (shoppingCartProduct == null)
                return NotFound("Product not found in the shopping cart.");

            _dbContext.ShoppingCartProducts.Remove(shoppingCartProduct);
            _dbContext.SaveChanges();

            return Ok("Product removed from the shopping cart successfully.");
        }
    }
}
