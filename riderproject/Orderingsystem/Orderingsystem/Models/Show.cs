namespace ShowAPI.Models
{
    public class Show
    {
        public int ShowId { get; set; }
        public string Title { get; set; } = null!;
        public int ReleaseYear { get; set; }
        public string Link { get; set; } = null!;
        public int Rating { get; set; }
    }
}