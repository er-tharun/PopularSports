using Microsoft.AspNetCore.Identity;

namespace PopularSports.SurveyApp.WebAPI.Models
{
    public class Roles:IdentityRole
    {
        public string RoleName { get; set; }
        public string UserName { get; set; }
    }
}
