using ecommerce.Models;

public class CartItemViewModel
{
    public int ProductId { get; set; } // Product ID associated with the item
    public string ProductName { get; set; }

    public string ImagePath { get; set; }

    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Total => Quantity * Price; // Calculate the total price for the item
                                              // Add other properties as needed for your cart view
    public Product Product { get; set; }

}
