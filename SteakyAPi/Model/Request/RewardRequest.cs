using System.ComponentModel.DataAnnotations;

namespace StreakyAPi.Model.Request
{
    public class RewardRequest
    {
        public int Id { get; set; }
        [Required]

        public int StreakId { get; set; }
        [Required]

        public string Description { get; set; }
        [Required]

        public int PointsClaimed { get; set; }
        [Required]

        public int BusinessId { get; set; }
    }
}
