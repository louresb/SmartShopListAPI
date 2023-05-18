namespace ShoppingListApp.Models
{
    public class ShoppingListModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<ShoppingCart> ShoppingCarts { get; set; } = new List<ShoppingCart>();

        public decimal CalculateTotalPrice()
        {
            return ShoppingCarts?.Sum(cart => cart.CalculateCartPrice()) ?? 0;
        }
    }
}



