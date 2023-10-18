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

        public async Task<IActionResult> Index(int? category, string searchTerm)
        {
            var productsQuery = _context.Products.AsQueryable();

            if (category.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.CategoryId == category);
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                // Filter products based on the search term.
                productsQuery = productsQuery.Where(p =>
                    p.Name.Contains(searchTerm) ||
                    p.Description.Contains(searchTerm));
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

        //// Search feature
        //[Route("products/search")]
        //public IActionResult Index(string searchTerm)
        //{
        //    IQueryable<Product> products = _context.Products;

        //    if (!string.IsNullOrEmpty(searchTerm))
        //    {
        //        searchTerm = searchTerm.ToLower(); // Convert the search term to lowercase

        //        // Filter products based on the search term (case-insensitive search).
        //        products = products.Where(p =>
        //            p.Name.ToLower().Contains(searchTerm) ||
        //            p.Description.ToLower().Contains(searchTerm));
        //    }

        //    var productViewModel = new ProductViewModel
        //    {
        //        Products = products.ToList(),
        //        // Your other model properties...
        //    };

        //    return View(productViewModel);
        //}
    }
}
