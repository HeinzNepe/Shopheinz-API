using Orderingsystem.Models;

namespace Orderingsystem.Interfaces;

public interface IProductService
{
    public IEnumerable<Product> GetAllProducts();
}