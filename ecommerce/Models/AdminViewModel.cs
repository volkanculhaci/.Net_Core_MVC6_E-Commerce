using Microsoft.AspNetCore.Mvc.Rendering;

namespace ecommerce.Models
{
    public class AdminViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public SelectList Categories { get; set; }
    }
}
