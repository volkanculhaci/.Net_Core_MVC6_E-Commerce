using ecommerce.Data;
using ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using System.Linq;

namespace ecommerce.Controllers
{
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
            var viewModel = new AdminViewModel
            {
                Products = new List<Product> { new Product() }, // Initialize a list with an empty product
                Categories = new SelectList(_context.Categories, "CategoryId", "Name")
            };

            return View(viewModel);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product newProduct)
        {

            // Save the new product to the database
            _context.Products.Add(newProduct);
            _context.SaveChanges();

            return RedirectToAction("Index");


            // If the model is not valid, return to the create form with validation errors
            var allCategories = _context.Categories.ToList();
            var viewModel = new AdminViewModel();
            //{
            //    Categories = new SelectList(allCategories, "CategoryId", "Name")
            //};

            return View(viewModel);
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


    }
}
