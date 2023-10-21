using System.ComponentModel.DataAnnotations;

namespace ecommerce.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile ImageFile { get; set; }

        public int CategoryId { get; set; } // Reference to Category
        public Category Category { get; set; }

        [Display(Name = "Stock Quantity")]
        public int StockQuantity { get; set; }
    }
}
