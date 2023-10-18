using ecommerce.Data;
using ecommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                    Price = product.Price, // Set the correct product price
                                           // Add other property mappings here
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

    //private List<CartItemViewModel> GetCartItems(List<CartItem> cartItems)
    //{
    //    return cartItems.Select(cartItem => new CartItemViewModel
    //    {
    //        ProductId = cartItem.ProductId, // Map properties accordingly
    //        ProductName = cartItem.ProductName,
    //        Quantity = cartItem.Quantity,
    //        Price = cartItem.Price,
    //        // Add other property mappings here
    //    }).ToList();
    //}

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


}
