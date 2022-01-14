using ShowAPI.Models;

namespace ShowAPI.Interfaces;

public interface IShowService
{
    public Show GetShow(int id);
    public IEnumerable<Show> GetAllShows();
}