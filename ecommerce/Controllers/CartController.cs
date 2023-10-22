using ecommerce.Data;
using ecommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

public class CartController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IShoppingCartService _shoppingCartService;

    public CartController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    private List<CartItemViewModel> GetCartItems(List<CartItem> cartItems)
    {
        var cartItemViewModels = new List<CartItemViewModel>();

        foreach (var cartItem in cartItems)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == cartItem.ProductId);

            if (product != null)
            {
                cartItemViewModels.Add(new CartItemViewModel
                {
                    ProductId = cartItem.ProductId,
                    ProductName = cartItem.ProductName,
                    Quantity = cartItem.Quantity,
                    Price = product.Price,

                });
            }
        }

        return cartItemViewModels;
    }


    [Authorize]
    public IActionResult Index()
    {
        // User is authenticated, proceed with cart creation and retrieval
        var userId = _userManager.GetUserId(User); // Get the user's ID

        // Check if the user has a cart
        var userCart = _context.Carts.FirstOrDefault(c => c.UserId == userId);

        if (userCart == null)
        {
            // Create a new cart for the user
            userCart = new Cart { UserId = userId };
            _context.Carts.Add(userCart);
            _context.SaveChanges();
        }

        // Fetch the user's cart items and display them
        var cartItems = _context.CartItems.Where(ci => ci.CartId == userCart.CartId).ToList();

        // Use the GetCartItems method to convert the CartItems to CartItemViewModel
        var cartItemViewModels = GetCartItems(cartItems);

        return View(cartItemViewModels);

    }

    public IActionResult gotocheckout()
    {

        return RedirectToAction("Checkout");

    }

    [Authorize]
    [HttpPost]
    public IActionResult AddItem(int id, int quantity)
    {
        var product = _context.Products.FirstOrDefault(p => p.ProductId == id);

        if (product == null)
        {
            return NotFound();
        }

        var userId = _userManager.GetUserId(User);
        var userCart = _context.Carts.FirstOrDefault(c => c.UserId == userId);

        if (userCart == null)
        {
            userCart = new Cart { UserId = userId };
            _context.Carts.Add(userCart);
            _context.SaveChanges();
        }

        // Check if the product is already in the user's cart
        var existingCartItem = _context.CartItems.FirstOrDefault(ci => ci.CartId == userCart.CartId && ci.ProductId == id);

        if (existingCartItem != null)
        {
            existingCartItem.Quantity += quantity;
        }
        else
        {
            _context.CartItems.Add(new CartItem
            {
                CartId = userCart.CartId,
                ProductId = product.ProductId,
                ProductName = product.Name, // Set the ProductName from the product
                Quantity = quantity,

                // You may also copy other properties from 'product' as needed
            });
        }

        _context.SaveChanges();

        return RedirectToAction("Index");
    }

    public IActionResult Checkout()
    {
        // User is authenticated, proceed with cart creation and retrieval
        var userId = _userManager.GetUserId(User); // Get the user's ID

        // Check if the user has a cart
        var userCart = _context.Carts.FirstOrDefault(c => c.UserId == userId);

        if (userCart == null)
        {
            // Create a new cart for the user
            userCart = new Cart { UserId = userId };
            _context.Carts.Add(userCart);
            _context.SaveChanges();
        }

        // Fetch the user's cart items and display them
        var cartItems = _context.CartItems.Where(ci => ci.CartId == userCart.CartId).ToList();

        // Use the GetCartItems method to convert the CartItems to CartItemViewModel
        var cartItemViewModels = GetCartItems(cartItems);

        return View(cartItemViewModels);


    }

    [HttpPost]
    public IActionResult UpdateQuantity(int productId, int quantity)
    {
        // Retrieve the cart item and update the quantity in your database
        var cartItem = _context.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

        if (cartItem != null)
        {
            cartItem.Quantity = quantity;
            _context.SaveChanges();
        }

        // Redirect back to the cart view
        return RedirectToAction("Index");
    }

    [Authorize]
    public IActionResult PlaceOrder()
    {
        var userId = _userManager.GetUserId(User);

        // Fetch the user's cart items
        var cartItems = _context.CartItems
            .Include(ci => ci.Product)
            .Where(ci => ci.Cart.UserId == userId)
            .ToList();

        if (cartItems.Count == 0)
        {
            // Handle the case where the cart is empty
            return RedirectToAction("Index");
        }

        // Create an order record
        var order = new Order
        {
            UserId = userId,
            OrderDate = DateTime.Now, // You may customize the order date as needed
            OrderItems = new List<OrderItem>() // Initialize the list of order items
        };

        foreach (var cartItem in cartItems)
        {
            // Create order item records
            var orderItem = new OrderItem
            {
                ProductId = cartItem.ProductId,
                Quantity = cartItem.Quantity,
                UnitPrice = cartItem.Product.Price
                // Add other order item details as needed
            };

            // Add the order item to the order's order items
            order.OrderItems.Add(orderItem);

            // Update product quantities
            cartItem.Product.StockQuantity -= cartItem.Quantity;
        }

        // Clear the cart
        _context.CartItems.RemoveRange(cartItems);

        // Add the order to the database
        _context.Orders.Add(order);

        // Save changes to the database
        _context.SaveChanges();

        return RedirectToAction("OrderConfirmation");

    }


    public IActionResult OrderConfirmation()
    {
        return View();
    }




}
