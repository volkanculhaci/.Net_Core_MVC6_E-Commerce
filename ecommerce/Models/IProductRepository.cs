using System.Collections.Generic;

namespace ecommerce.Models
{
    public interface IProductRepository
    {
        IEnumerable<Product> Products { get; }

        Product GetProductById(int productId);
    }
}
