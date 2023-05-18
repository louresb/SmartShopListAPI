using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingListApp.Data;
using ShoppingListApp.Models;

namespace ShoppingListApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingListController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public ShoppingListController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAllLists()
        {
            var shoppingLists = _dbContext.ShoppingLists
                .Include(list => list.ShoppingCarts)
                .ThenInclude(cart => cart.ShoppingCartProducts)
                .ThenInclude(scp => scp.Product)
                .ToList();

            var shoppingListsWithDetails = shoppingLists.Select(list => new
            {
                list.Id,
                list.Name,
                ShoppingCarts = list.ShoppingCarts.Select(cart => new
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
                }).ToList(),
                TotalPrice = list.CalculateTotalPrice()
            });

            return Ok(shoppingListsWithDetails);
        }

        [HttpGet("{id}")]
        public IActionResult GetList(int id)
        {
            var shoppingList = _dbContext.ShoppingLists
                .Include(list => list.ShoppingCarts)
                .ThenInclude(cart => cart.ShoppingCartProducts)
                .ThenInclude(scp => scp.Product)
                .FirstOrDefault(list => list.Id == id);

            if (shoppingList == null)
                return NotFound();

            var shoppingCart = shoppingList.ShoppingCarts.SingleOrDefault(cart => cart.Id == id);

            var shoppingListWithDetails = new
            {
                shoppingList.Id,
                shoppingList.Name,
                ShoppingCarts = shoppingCart != null ? new List<object>
        {
            new
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
            }
        } : null,
                TotalPrice = shoppingList.CalculateTotalPrice()
            };

            return Ok(shoppingListWithDetails);
        }

        [HttpPost]
        public IActionResult CreateList(ShoppingListModel shoppingList)
        {
            _dbContext.ShoppingLists.Add(shoppingList);
            _dbContext.SaveChanges();

            return CreatedAtAction(nameof(GetList), new { id = shoppingList.Id }, shoppingList);
        }

        [HttpPost("{listId}/addcart")]
        public IActionResult AddCartToList(int listId, [FromBody] int cartId)
        {
            var shoppingList = _dbContext.ShoppingLists
                .Include(list => list.ShoppingCarts)
                .ThenInclude(cart => cart.ShoppingCartProducts)
                .ThenInclude(scp => scp.Product)
                .FirstOrDefault(list => list.Id == listId);

            if (shoppingList == null)
                return NotFound("Shopping list not found.");

            var cart = _dbContext.ShoppingCarts
                .Include(c => c.ShoppingCartProducts)
                .ThenInclude(scp => scp.Product)
                .FirstOrDefault(c => c.Id == cartId);

            if (cart == null)
                return NotFound("Shopping cart not found.");

            shoppingList.ShoppingCarts.Add(cart);
            _dbContext.SaveChanges();

            return Ok("Shopping cart added to the shopping list successfully.");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateList(int id, [FromBody] ShoppingListModel updatedList)
        {
            var shoppingList = _dbContext.ShoppingLists.FirstOrDefault(list => list.Id == id);

            if (shoppingList == null)
                return NotFound("Shopping list not found.");

            if (updatedList == null)
                return BadRequest("Invalid shopping list data.");

            shoppingList.Name = updatedList.Name;


            _dbContext.SaveChanges();

            return Ok("Shopping list updated successfully.");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteList(int id)
        {
            var shoppingList = _dbContext.ShoppingLists.FirstOrDefault(list => list.Id == id);

            if (shoppingList == null)
                return NotFound();

            _dbContext.ShoppingLists.Remove(shoppingList);
            _dbContext.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{listId}/removecart/{cartId}")]
        public IActionResult RemoveCartFromList(int listId, int cartId)
        {
            var shoppingList = _dbContext.ShoppingLists
                .Include(list => list.ShoppingCarts)
                .FirstOrDefault(list => list.Id == listId);

            if (shoppingList == null)
                return NotFound("Shopping list not found.");

            var cart = shoppingList.ShoppingCarts.FirstOrDefault(c => c.Id == cartId);

            if (cart == null)
                return NotFound("Shopping cart not found in the shopping list.");

            shoppingList.ShoppingCarts.Remove(cart);
            _dbContext.SaveChanges();

            return Ok("Shopping cart removed from the shopping list successfully.");
        }
    }
}
