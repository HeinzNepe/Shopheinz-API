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

        using var connection =
            new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
        const string commandString =
            "select * from online_store.orders,online_store.user,online_store.addresses,online_store.postal_places, online_store.credentials where orders.ouid = user.uuid and user.uusername = credentials.username and orders.address_id = addresses.aid and addresses.postal_nr = postal_places.postnr and orders.oid = @id";
        var command = new MySqlCommand(commandString, connection);
        const string productsCommandString =
            "select products.*, quantity from products, orders_products, orders where orders.oid = orders_products.orders_order_id and products.pid = orders_products.products_product_id and orders.oid = @id";
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
                // ID in the address table
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
            order.Status = (string) reader["status"];
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
                OrderId = order.Id,
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








    public IEnumerable<Order> GetUserOrders(int id)
    {
        var products = new List<OrderProduct>();
        var orders = new List<Order>();

        using var connection =
            new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);

        const string selectProduct =
            "select products.*, quantity, oid from products, orders_products, orders where orders.oid = orders_products.orders_order_id and products.pid = orders_products.products_product_id and ouid = @id";
        var productCommand = new MySqlCommand(selectProduct, connection);
        productCommand.Parameters.AddWithValue("@id", id);

        const string selectOrder =
            "select * from online_store.orders,online_store.user,online_store.addresses,online_store.postal_places,online_store.credentials where orders.ouid = user.uuid and user.uusername = credentials.username and orders.address_id = addresses.aid and addresses.postal_nr = postal_places.postnr and user.uuid = @id";
        var orderCommand = new MySqlCommand(selectOrder, connection);
        orderCommand.Parameters.AddWithValue("@id", id);

        connection.Open();

        using var productsReader = productCommand.ExecuteReader();
        while (productsReader.Read())
        {
            products.Add(new OrderProduct()
            {
                OrderId = (int) productsReader["oid"],
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

        productsReader.Close();
        using var ordersReader = orderCommand.ExecuteReader();
        while (ordersReader.Read())
        {
            var orderId = (int) ordersReader["oid"];
            orders.Add(new Order
            {
                Id = orderId,
                User = new User
                {
                    // ID in the User table
                    Id = (int) ordersReader["uuid"],
                    FirstName = (string) ordersReader["first_name"],
                    LastName = (string) ordersReader["last_name"],
                    Email = (string) ordersReader["email"],
                    PhoneNumber = (int) ordersReader["phone_number"],
                    Credentials = new Credentials
                    {
                        Username = (string) ordersReader["username"],
                        Password = (string) ordersReader["password"],
                        Token = (string) ordersReader["token"],
                        AccessLevel = (int) ordersReader["access_level"]
                    }
                },

                Products = products.Where(p => p.OrderId == orderId),

                Address = new Address
                {
                    // ID in the address table
                    Id = (int) ordersReader["aid"],
                    AddressLine = (string) ordersReader["address_line"],
                    PostalNumber = new PostalNumber
                    {
                        Number = (int) ordersReader["postnr"],
                        Place = (string) ordersReader["post_place"]
                    },
                    Country = (string) ordersReader["country"]
                },
                TotalPrice = (float) ordersReader["total_price"],
                OrderTime = (DateTime) ordersReader["order_time"],
                Status = (string) ordersReader["status"],
            });
        }

        return orders;
    }

    public int CreateAddress(string addressLine, int postalNumber, string country)
    {
        var aid = 0;

        using var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);        
        const string commandString =
         "insert into addresses (address_line, postal_nr, country) values (@addressLine, @postalNumber, @country)";
        const string idCommandString = "select aid from addresses where address_line = @addressLine";
        var command = new MySqlCommand(commandString, connection);
        var idCommand = new MySqlCommand(idCommandString, connection);
        idCommand.Parameters.AddWithValue("@addressLine", addressLine);
        command.Parameters.AddWithValue("@addressLine", addressLine);
        command.Parameters.AddWithValue("@postalNumber", postalNumber);
        command.Parameters.AddWithValue("@country", country);
        
        connection.Open();
        command.ExecuteNonQuery();
        connection.Close();
        connection.Open();
        using var Reader = idCommand.ExecuteReader();
        while (Reader.Read())
        {
            aid = (int) Reader["aid"];
        }
        
        
        return aid;
    }


    public bool CreateOrder(int userId, int addressId, float totalPrice)
    {
        var order = new Order();

        using var connection =
            new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
        const string commandString =
            "insert into online_store.orders (ouid, address_id, total_price, status) values (@userId, @addressId, @totalPrice, 0)";
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

        using var connection =
            new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
        const string commandString =
            "insert into online_store.orders_products (orders_order_id, products_product_id, quantity) values (@orderId, @productId, @quantity)";
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

    public bool UpdateStatus(int id, string updateStatus)
    {
        using var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
        const string commandString = "update online_store.orders set status = @Status where oid = @id";
        var command = new MySqlCommand(commandString, connection);
        command.Parameters.AddWithValue("@Status", updateStatus);
        command.Parameters.AddWithValue("@id", id);
        
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
    
    

    public bool DeleteOrder(int orderId)
    {
        using var connection =
            new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
        const string commandString =
            "delete from online_store.orders_products where orders_order_id = @orderId;delete from online_store.orders where oid = @orderId;";
        var command = new MySqlCommand(commandString, connection);
        command.Parameters.AddWithValue("@orderId", orderId);
        

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