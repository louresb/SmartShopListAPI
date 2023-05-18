namespace ShoppingListApp.DTOs
{
    public class ShoppingCartDto
    {
        public string Name { get; set; } = string.Empty;
        public int ShoppingListId { get; set; } = 0;
        public decimal CartPrice { get; set; } = 0;
    }
}

