using MySqlConnector;
using Orderingsystem.Interfaces;
using Orderingsystem.Models;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace Orderingsystem.Services;


public class OrderService : IOrderService
{
    public Order GetOrder(int id)
    {
        var order = new Order();
        
        using var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
        const string commandString = "select * from online_store.orders,online_store.user,online_store.products,online_store.addresses,online_store.postal_places, online_store.credentials where orders.ouid = user.uuid and user.uusername = credentials.username and product_id = products.pid and orders.address_id = addresses.aid and addresses.postal_nr = postal_places.postnr and orders.oid = @id";
        var command = new MySqlCommand(commandString, connection);

        command.Parameters.AddWithValue("@id", id);
        
        connection.Open();
        
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            order.Id = (int) reader["oid"];
            order.User = new User
            {
                Id = (int) reader["uuid"],
                FirstName = (string) reader["first_name"],
                LastName = (string) reader["last_name"],
                Email = (string) reader["email"],
                PhoneNumber = (int) reader["phone_number"],
                Credentials = new Credentials
                {
                    Username = (string) reader["username"],
                    Password = (string) reader["password"],
                    Token = (string) reader["token"],
                    AccessLevel = (int) reader["access_level"]
                }
            };
            order.Product = new Product
            {
                Id = (int) reader["pid"],
                Name = (string) reader["name"],
                Description = (string) reader["description"],
                Price = (float) reader["price"],
                Stock = (int) reader["stock"],
                ImageUrl = (string) reader["image_url"]
            };
            order.Address = new Address
            {
                Id = (int) reader["aid"],
                AddressLine = (string) reader["address_line"],
                PostalNumber = new PostalNumber
                {
                    Number = (int) reader["postnr"],
                    Place = (string) reader["post_place"]
                },
                Country = (string) reader["country"]
            };
            order.TotalPrice = (float) reader["total_price"];
            order.OrderTime = (DateTime) reader["order_time"];
        }

        return order;
    }
}