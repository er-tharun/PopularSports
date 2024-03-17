using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PopularSports.SurveyApp.WebAPI.Models
{
    public class Survey
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public Sport Sport { get; set; }
        [Required]
        public int CountOfVotes { get; set; }
    }
}
