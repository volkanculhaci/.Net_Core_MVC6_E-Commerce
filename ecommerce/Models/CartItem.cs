using ecommerce.Models;

public class CartItem
{
    public int CartItemId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    //public string ImagePath { get; set; }

    public int Quantity { get; set; }
    public int CartId { get; set; } // Add this property

    public Product Product { get; set; }
    public Cart Cart { get; set; } // Reference to the Cart

}
