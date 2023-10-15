using ecommerce.Data;
using ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ecommerce.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Display a list of products
        public async Task<IActionResult> Index(int? category)
        {
            var productsQuery = _context.Products.AsQueryable();

            if (category.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.CategoryId == category);
            }

            var products = await productsQuery.ToListAsync();

            // Fetch the list of categories for the dropdown
            var categories = await _context.Categories.ToListAsync();
            ViewBag.Categories = categories;

            return View(products);
        }


        // Display details of a specific product
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productDetails = await _context.Products
                .Join(_context.Categories,
                    product => product.CategoryId,
                    category => category.CategoryId,
                    (product, category) => new ProductDetailsViewModel
                    {
                        Product = product,
                        CategoryName = category.Name
                    })
                .FirstOrDefaultAsync(result => result.Product.ProductId == id);

            if (productDetails == null)
            {
                return NotFound();
            }

            return View(productDetails);
        }
    }
}
