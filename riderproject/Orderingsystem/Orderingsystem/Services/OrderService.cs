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
        const string commandString = "select * from online_store.orders,online_store.user,online_store.addresses,online_store.postal_places, online_store.credentials where orders.ouid = user.uuid and user.uusername = credentials.username and orders.address_id = addresses.aid and addresses.postal_nr = postal_places.postnr and orders.oid = @id";
        var command = new MySqlCommand(commandString, connection);
        const string productsCommandString = "select products.*, quantity from products, orders_products, orders where orders.oid = orders_products.orders_order_id and products.pid = orders_products.products_product_id and orders.oid = @id";
        var productsCommand = new MySqlCommand(productsCommandString, connection);
        command.Parameters.AddWithValue("@id", id);
        productsCommand.Parameters.AddWithValue("@id", id);
        
        connection.Open();
        
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            order.Id = (int) reader["oid"];
            order.User = new User
            {
                // ID in the User table
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
            
            order.Address = new Address
            {
                // ID in the adress table
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

        reader.Close();
        
        // Makes a list for all the products in the order
        var products = new List<OrderProduct>();
        // Makes a new reader
        using var productsReader = productsCommand.ExecuteReader();
        while (productsReader.Read())
        {
            // Adds object to list 
            products.Add(new OrderProduct
            {
                Product = new Product
                {
                    Id = (int) productsReader["pid"],
                    Name = (string) productsReader["name"],
                    Description = (string) productsReader["description"],
                    Stock = (int) productsReader["stock"],
                    Price = (float) productsReader["price"],
                    ImageUrl = (string) productsReader["image_url"]
                },
                Quantity = (int) productsReader["quantity"]
            });
        }

        order.Products = products;
        
        return order;
    }

    public bool CreateOrder(int userId, int addressId, float totalPrice)
    {
        var order = new Order();
        
        using var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
        const string commandString = "insert into online_store.orders (ouid, address_id, total_price) values (@userId, @addressId, @totalPrice)";
        var command = new MySqlCommand(commandString, connection);
        command.Parameters.AddWithValue("@userId", userId);
        command.Parameters.AddWithValue("@addressId", addressId);
        command.Parameters.AddWithValue("@totalPrice", totalPrice);



        try
        {
            connection.Open();
            command.ExecuteNonQuery();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }

    }

    public bool AddProductToOrder(int orderId, int productId, int quantity)
    {
        var order = new Order();
        
        using var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
        const string commandString = "insert into online_store.orders_products (orders_order_id, products_product_id, quantity) values (@orderId, @productId, @quantity)";
        var command = new MySqlCommand(commandString, connection);
        command.Parameters.AddWithValue("@orderId", orderId);
        command.Parameters.AddWithValue("@productId", productId);
        command.Parameters.AddWithValue("@quantity", quantity);



        try
        {
            connection.Open();
            command.ExecuteNonQuery();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
}