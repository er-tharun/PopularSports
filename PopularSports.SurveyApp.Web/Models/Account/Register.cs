using System.ComponentModel.DataAnnotations;

namespace PopularSports.SurveyApp.Web.Models.Account
{
    public class Register
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
