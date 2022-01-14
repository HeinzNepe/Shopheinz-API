using Orderingsystem.Models;

namespace Orderingsystem.Interfaces;

public interface IProductService
{
    public Product GetProduct(int id);

    public IEnumerable<Product> GetAllProducts();
}