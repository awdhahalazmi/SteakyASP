using System.ComponentModel.DataAnnotations;

namespace StreakyAPi.Model.Request
{
    public class StreakRequest
    {
        public int Id { get; set; }

        [Required]

        public string Title { get; set; }
        [Required]

        public string Description { get; set; }
        [Required]

        public DateTime StartDate { get; set; }
        [Required]

        public DateTime EndDate { get; set; }
        [Required]

        public List<int> BusinessIds { get; set; }
    }
}
