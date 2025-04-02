namespace CinemaArchiveAPI.Model
{
    public class Cinema
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime Year { get; set; }

        public Director Director { get; set; }
    }
}
