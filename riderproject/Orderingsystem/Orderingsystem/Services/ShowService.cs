using MySqlConnector;
using ShowAPI.Interfaces;
using ShowAPI.Models;

namespace ShowAPI.Services;

public class ShowService : IShowService
{
    public Show GetShow(int id)
    {
        var show = new Show();

        // Define connection
        using var connection = new MySqlConnection("server=192.168.1.62;uid=api;pwd=SuperSecure123;database=api_database_stuff");
        //Define command
        var commandString = "SELECT * FROM api_database_stuff.shows WHERE show_id = @id";
        //What command where
        var command = new MySqlCommand(commandString, connection);

        //Lets the id thing do the thing (more safe than doing a + at the sql statement
        command.Parameters.AddWithValue("@id", id);
        
        //Opens connection
        connection.Open();

        //Execute reader on command
        using var reader = command.ExecuteReader();
        //While the reader is reading, do 
        while (reader.Read())
        {
            //Sets the thing to what it reads in the database
            show.ShowId = (int) reader["show_id"];
            show.Title = (string) reader["title"];
            show.ReleaseYear = (int) reader["release_year"];
            show.Link = (string) reader["link"];
            show.Rating = (int) reader["rating"];
        }
        
        return show;
    }

    public IEnumerable<Show> GetAllShows()
    {
        var shows = new List<Show>();

        // Define connection
        using var connection = new MySqlConnection("server=192.168.1.62;uid=api;pwd=SuperSecure123;database=api_database_stuff");
        //Define command
        var commandString = "SELECT * FROM api_database_stuff.shows";
        //What command where
        var command = new MySqlCommand(commandString, connection);

        //Opens connection
        connection.Open();

        
        //Execute reader on command
        using var reader = command.ExecuteReader();
        
        //While the reader is reading, do 
        while (reader.Read())
        {
            shows.Add(new Show
            {
                //Sets the thing to what it reads in the database
                ShowId = (int) reader["show_id"],
                Title = (string) reader["title"],
                ReleaseYear = (int) reader["release_year"],
                Link = (string) reader["link"],
                Rating = (int) reader["rating"]
            });
        }

        
        return shows;
    }
}