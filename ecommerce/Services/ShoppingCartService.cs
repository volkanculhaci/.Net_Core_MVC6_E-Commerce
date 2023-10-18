using ecommerce.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

public interface IShoppingCartService
{
    List<CartItem> GetCartItems();
    void AddItem(Product product, int quantity);
    void RemoveItem(int productId);
    decimal CalculateTotal();

}

public class ShoppingCartService : IShoppingCartService
{
    private List<CartItem> cartItems;

    public ShoppingCartService()
    {
        // Initialize the cartItems list in the constructor
        cartItems = new List<CartItem>();
    }

    public List<CartItem> GetCartItems()
    {
        // Implement the logic to fetch cart items from your data source.
        return new List<CartItem>(); // Replace with actual implementation
    }

    public void AddItem(Product product, int quantity)
    {
        var existingCartItem = cartItems.FirstOrDefault(item => item.Product.ProductId == product.ProductId);

        if (existingCartItem != null)
        {
            // If the product is already in the cart, update the quantity
            existingCartItem.Quantity += quantity;
        }
        else
        {
            // If the product is not in the cart, add a new cart item
            cartItems.Add(new CartItem
            {
                Product = product,
                Quantity = quantity
            });
        }
    }

    public void RemoveItem(int productId)
    {
        var cartItemToRemove = cartItems.FirstOrDefault(item => item.Product.ProductId == productId);

        if (cartItemToRemove != null)
        {
            cartItems.Remove(cartItemToRemove);
        }
    }

    public decimal CalculateTotal()
    {
        return cartItems.Sum(item => item.Product.Price * item.Quantity);
    }
}
