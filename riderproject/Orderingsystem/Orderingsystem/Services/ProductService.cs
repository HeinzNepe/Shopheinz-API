using MySqlConnector;
using Orderingsystem.Interfaces;
using Orderingsystem.Models;

namespace Orderingsystem.Services;

public class ProductService : IProductService
{
    public IEnumerable<Product> GetAllProducts()
    {
        var list = new List<Product>();

        using var connection = new MySqlConnection("server=192.168.1.62;uid=api;pwd=SuperSecure123;database=api_database_stuff");
        const string commandString = "select * from online_store.products";
        
        var command = new MySqlCommand(commandString, connection);

        connection.Open();

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new Product
            {
                Id = (int) reader["product_id"],
                Name = (string) reader["name"],
                Price = (float) reader["price"],
                Stock = (int) reader["stock"],
                ImageUrl = (string) reader["image_url"]
            });
        }

        return list;
    }
}