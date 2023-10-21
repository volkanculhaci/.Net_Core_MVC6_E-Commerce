using System.Collections.Generic;

namespace ecommerce.Models
{
    public interface IProductRepository
    {
        IEnumerable<Product> Products { get; }

        void DeleteProduct(int productId);
        Product GetProductById(int productId);
    }
}
