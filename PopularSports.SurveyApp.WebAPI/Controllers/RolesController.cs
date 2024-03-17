using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PopularSports.SurveyApp.WebAPI.Data;
using PopularSports.SurveyApp.WebAPI.Models;

namespace PopularSports.SurveyApp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _applicationDbContext;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, ApplicationDbContext applicationDbContext)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _applicationDbContext = applicationDbContext;
        }

        [HttpPost]
        [Route("AddRole")]
        public IActionResult CreateRole([FromBody] Roles roles)
        {
            IdentityRole role = new IdentityRole() { Name = roles.RoleName };
            var data = _roleManager.CreateAsync(role);
            if (data.Result.Succeeded)
                return Ok();
            else
                return BadRequest();
        }
        [HttpGet]
        [Route("GetRoles")]
        public IActionResult GetRoles()
        {
            return Ok(_roleManager.Roles.ToList());
        }

        [HttpGet]
        [Route("GetUsers")]
        public IActionResult GetUsers()
        {
            return Ok(_userManager.Users.ToList());
        }

        [HttpGet]
        public async Task<IActionResult> GetUserRole(string user)
        {
            var data = _applicationDbContext.Users.Where(a => a.Email.Equals(user)).FirstOrDefault();
            if (data is not null)
            {
                var userRoles = await _userManager.GetRolesAsync(data);
                var isAdminRole = userRoles.FirstOrDefault(a => a.Equals("Admin")) is not null ? true : false ;
                return Ok(new { isAdmin = isAdminRole });
            }
            return Ok(new { isAdmin = "false" });

        }

        [HttpPost]
        [Route("MapUserToRole")]
        public async Task<IActionResult> MapUserToRoles(Roles obj)
        {
            bool status = await _roleManager.RoleExistsAsync(obj.RoleName);
            if(status)
            {
                var user = _applicationDbContext
                    .Users
                    .Where(a => a.UserName.Equals(obj.UserName))
                    .FirstOrDefault();
                if(user is not null)
                {
                    await _userManager.AddToRoleAsync(user, obj.RoleName.ToString());
                }
            }
            else
            {
                return new BadRequestObjectResult("Roles Not exist");
            }
            return Ok();
        }
    }
}
