namespace StreakyAPi.Model.Streak
{
   
        public class SecretExperience
        {
            public int Id { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public int StreakClaimed { get; set; }
            public int BusinessId { get; set; }

            public Business Business { get; set; }
        }
    }

