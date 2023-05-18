using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingListApp.Models
{

    public class ShoppingCart
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        [Required]
        public int ShoppingListId { get; set; }

        [ForeignKey("ShoppingListId")]
        public ShoppingListModel ShoppingList { get; set; } = null!;

        [InverseProperty("ShoppingCart")]
        public List<ShoppingCartProduct> ShoppingCartProducts { get; set; } = new List<ShoppingCartProduct>();

        public decimal CalculateCartPrice()
        {
            return ShoppingCartProducts?.Sum(scp => scp.Product.Price * scp.Quantity) ?? 0;
        }
    }
}