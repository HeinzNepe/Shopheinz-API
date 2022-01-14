using Microsoft.AspNetCore.Mvc;
using Orderingsystem.Interfaces;
using Orderingsystem.Models;

namespace Orderingsystem.Controllers;

[ApiController]
[Route("products")]
public class ProductController : Controller
{

    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }
    
    
    
    
    [HttpGet("all")]
    public IEnumerable<Product> GetAllProducts()
    {
        return _productService.GetAllProducts();
    }
}