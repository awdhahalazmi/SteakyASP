using StreakyAPi.Model.Streak;

namespace StreakyAPi.Model
{
    public class Business
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string Image { get; set; }
        public string Question { get; set; }
        public string CorrectAnswer { get; set; }
        public string WrongAnswer1 { get; set; }
        public string WrongAnswer2 { get; set; }
        public ICollection<Location> Locations { get; set; } = new List<Location>();
        public ICollection<Streaks> Streaks { get; set; } = new List<Streaks>();

        public Category Category { get; set; }
    }
}
