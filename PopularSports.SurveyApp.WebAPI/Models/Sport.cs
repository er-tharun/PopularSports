using System.ComponentModel.DataAnnotations;

namespace PopularSports.SurveyApp.WebAPI.Models
{
    public class Sport
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}
