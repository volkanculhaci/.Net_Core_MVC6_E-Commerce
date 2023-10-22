using Microsoft.AspNetCore.Mvc.Rendering;

namespace ecommerce.Models
{
    public class AdminViewModel
    {
        public List<Product> Products { get; set; }
        public SelectList Categories { get; set; }
        public IFormFile ImageFile { get; set; }
        public Product NewProduct { get; set; }

    }


}
