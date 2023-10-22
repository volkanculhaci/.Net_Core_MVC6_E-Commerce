using ecommerce.Data;
using ecommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using System.Linq;

namespace ecommerce.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products.ToList(); // Assuming Products is a DbSet in your ApplicationDbContext
            var viewModel = new AdminViewModel
            {
                Products = products
            };
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            // Provide any necessary data to the view, such as categories for dropdowns
            var categories = _context.Categories.ToList();
            var viewModel = new ProductViewModel
            {
                Categories = new SelectList(categories, "CategoryId", "Name")
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AdminViewModel viewModel)
        {
            if (viewModel.NewProduct != null)
            {
                if (viewModel.NewProduct.ImageFile != null)
                {
                    string productIdString = viewModel.NewProduct.ProductId.ToString("D12"); // Format product ID as 12 digits
                    string productName = viewModel.NewProduct.Name.Replace(" ", ""); // Remove spaces from the product name
                    string uniqueFileName = "0" + productIdString + productName + "_" + viewModel.NewProduct.ImageFile.FileName;
                    string filePath = Path.Combine("wwwroot/images", uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        viewModel.NewProduct.ImageFile.CopyTo(stream);
                    }

                    viewModel.NewProduct.ImagePath = uniqueFileName;
                }

                _context.Products.Add(viewModel.NewProduct);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            var allCategories = _context.Categories.ToList();
            viewModel.Categories = new SelectList(allCategories, "CategoryId", "Name");
            return View(viewModel);
        }




        [HttpGet]
        public IActionResult Details(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound(); // Return a 404 status code if the product is not found.
            }

            return View(product);
        }



        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            var allCategories = _context.Categories.ToList();

            var viewModel = new AdminViewModel
            {
                Products = new List<Product> { product },
                Categories = new SelectList(allCategories, "CategoryId", "Name", product.CategoryId)
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Product updatedProduct)
        {
            if (id != updatedProduct.ProductId)
            {
                return NotFound();
            }

            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            product.Name = updatedProduct.Name;
            product.Description = updatedProduct.Description;
            //product.CategoryId = updatedProduct.CategoryId; // Set the new category ID

            // Get a list of categories
            var allCategories = _context.Categories.ToList();

            // Set categories in ViewBag
            ViewBag.Categories = new SelectList(allCategories, "CategoryId", "Name");

            product.Price = updatedProduct.Price;
            product.StockQuantity = updatedProduct.StockQuantity;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            // Retrieve the product to delete from the database by its ID
            var product = _context.Products.Find(id);

            if (product == null)
            {
                return NotFound(); // Return a 404 response if the product is not found
            }

            return View(product); // Return the "Delete" view with the product details
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = _context.Products.Find(id);

            if (product == null)
            {
                return NotFound(); // Return a 404 response if the product is not found
            }

            _context.Products.Remove(product);
            _context.SaveChanges();

            return RedirectToAction("Index"); // Redirect to the list of products
        }



    }
}