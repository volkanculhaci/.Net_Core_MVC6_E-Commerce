using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ecommerce.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }

        public int CategoryId { get; set; } // Reference to Category
        public Category Category { get; set; }

        [Display(Name = "Stock Quantity")]
        public int StockQuantity { get; set; }
    }
}
