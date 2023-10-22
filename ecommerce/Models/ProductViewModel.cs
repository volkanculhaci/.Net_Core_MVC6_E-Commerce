using Microsoft.AspNetCore.Mvc.Rendering;

namespace ecommerce.Models
{
    public class ProductViewModel
    {
        public Product NewProduct { get; set; }
        public SelectList Categories { get; set; }
    }

}
