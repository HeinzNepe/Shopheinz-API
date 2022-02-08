using MySqlConnector;
using Orderingsystem.Interfaces;
using Orderingsystem.Models;
namespace Orderingsystem.Services;
using ConfigurationManager = System.Configuration.ConfigurationManager;

public class UserService : IUserService
{
    
    public User GetUser(int id)
    {
        var user = new User();
        
        using var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
        const string commandString = "select * from online_store.user, online_store.credentials where user.uusername = credentials.username and uuid = @id";
        var command = new MySqlCommand(commandString, connection);
        command.Parameters.AddWithValue("@id", id);
        


        
        
        connection.Open();
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            // ID in the User table
            user.Id = (int) reader["uuid"];
            user.FirstName = (string) reader["first_name"];
            user.LastName = (string) reader["last_name"];
            user.Email = (string) reader["email"];
            user.PhoneNumber = (int) reader["phone_number"];
            user.pfp = (string) reader["pfp"];
            user.Credentials = new Credentials
            {
                Username = (string) reader["username"],
                Password = (string) reader["password"],
                Token = (string) reader["token"],
                AccessLevel = (int) reader["access_level"]
            };
        }

        return user;

    }

    
    
    
    
    
    private static readonly Random Random = new Random();

    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[Random.Next(s.Length)]).ToArray());
    }
    
   
    public bool CreateUser(string firstName, string lastName, string username, string email, int phoneNumber, string pass, string pfp, int accessLevel)
    {
        
        using var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
        const string credentialsString = "insert into online_store.credentials (username, password, token, access_level) values (@username, @pass, @token, @access_level)";
        const string usersString = "insert into online_store.user (email, phone_number, first_name, last_name,pfp, uusername) values (@email, @phoneNumber, @firstName, @lastName, @pfp, @username)";
        var credentialsCommand = new MySqlCommand(credentialsString, connection);
        var userCommand = new MySqlCommand(usersString, connection);
        credentialsCommand.Parameters.AddWithValue("@username", username);
        credentialsCommand.Parameters.AddWithValue("@pass", pass);
        credentialsCommand.Parameters.AddWithValue("@token", RandomString(64));
        credentialsCommand.Parameters.AddWithValue("@access_level", accessLevel);
        userCommand.Parameters.AddWithValue("@username", username);
        userCommand.Parameters.AddWithValue("@email", email);
        userCommand.Parameters.AddWithValue("@phoneNumber", phoneNumber);
        userCommand.Parameters.AddWithValue("@firstName", firstName);
        userCommand.Parameters.AddWithValue("@lastName", lastName);
        userCommand.Parameters.AddWithValue("@pfp", pfp);
        
        

        try
        {
            connection.Open();
            credentialsCommand.ExecuteNonQuery();
            userCommand.ExecuteNonQuery();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
        return true;
    }

    public bool DeleteUser(string username)
    {
        using var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
        const string commandString = "delete from online_store.user where uusername = @username; delete from online_store.credentials where username = @username";
        var command = new MySqlCommand(commandString, connection);
        command.Parameters.AddWithValue("@username", username);
        



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