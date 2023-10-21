using ecommerce.Models;

namespace ecommerce.Controllers
{
    public interface IOrderRepository
    {
        IEnumerable<Product> Orders { get; }

        IEnumerable<Product> GetOrderById(int orderId);
    }
}