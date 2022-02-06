using MySqlConnector;
using Orderingsystem.Interfaces;
using Orderingsystem.Models;
namespace Orderingsystem.Services;
using ConfigurationManager = System.Configuration.ConfigurationManager;


public class AuthService : IAuthService
{
    public string VerifyCredentials(string user, string pass)
    {
        var token = "";
        
        using var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
        const string commandString = "select token from online_store.credentials where username = @user and password = @pass";
        var command = new MySqlCommand(commandString, connection);
        
        command.Parameters.AddWithValue("@user", user);
        command.Parameters.AddWithValue("@pass", pass);

        connection.Open();
        
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
           token = (string) reader[0];
        }
        
        return token;
    }

    public bool UpdatePass(string user, string pass, string newPass)
    {
        using var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
        const string commandString = "update online_store.credentials set password = @newPass where username = @username and password = @pass";
        var command = new MySqlCommand(commandString, connection);
        
        command.Parameters.AddWithValue("@username", user);
        command.Parameters.AddWithValue("@pass", pass);
        command.Parameters.AddWithValue("@newPass", newPass);

        
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