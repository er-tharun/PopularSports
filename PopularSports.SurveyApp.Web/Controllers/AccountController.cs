using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Versioning;
using PopularSports.SurveyApp.Web.Models.Account;
using PopularSports.SurveyApp.WebAPI.Models;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Runtime.InteropServices;

namespace PopularSports.SurveyApp.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string register = "/register";
        private readonly string login = "/login";
        private readonly string MAP_USER_TO_ROLE = "/api/Roles/MapUserToRole";
        private readonly string GET_ROLES = "/api/Roles/GetRoles";
        private readonly string GET_USERS = "/api/Roles/GetUsers";
        //private readonly string "api/Roles/MapUserToRole";

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Register registerData)
        {
            HttpClient client = new HttpClient();
            try
            {
                var data = await client
                .PostAsJsonAsync($"{_configuration["APIBaseAddress"]}{register}", registerData);
                if (data.IsSuccessStatusCode)
                {
                    return RedirectToAction("Login");
                }
                return View();
            }
            catch(HttpRequestException ex)
            {
                return RedirectToAction("AccessDenied", "Home");
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> MapRoleToUser()
        {
            HttpClient client = new HttpClient();
            if (Request.Cookies.TryGetValue("token", out var bearerToken))
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + bearerToken);

                var usersList = await client
                    .GetFromJsonAsync<IList<IdentityUser>>($"{_configuration["APIBaseAddress"]}{GET_USERS}");
                var userEmails = usersList.Select(a => a.Email).AsEnumerable();
                var userList = new List<SelectListItem>();
                foreach (var userEmail in userEmails)
                {
                    userList.Add(new SelectListItem() { Text = userEmail, Value = userEmail });
                }

                var rolesList = await client
                    .GetFromJsonAsync<IList<IdentityRole>>($"{_configuration["APIBaseAddress"]}{GET_ROLES}");
                var roles = rolesList.Select(a => a.Name).AsEnumerable();
                var roleList = new List<SelectListItem>();
                foreach (var role in roles)
                {
                    roleList.Add(new SelectListItem() { Text = role, Value = role });
                }
                ViewBag.Users = userList;
                ViewBag.Roles = roleList;


                return View();
            }
            else { return View("Login", "Account"); }
        }
        //[HttpGet]
        //public IActionResult MapRoleToUser()
        //{
        //    return View();
        //}
        [HttpPost]
        public async Task<IActionResult> MapRoleToUser(Roles roles)
        {
            HttpClient client = new HttpClient();
            var data = await client
                .PostAsJsonAsync($"{_configuration["APIBaseAddress"]}{MAP_USER_TO_ROLE}", roles);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(Account account)
        {
            if (ModelState.IsValid)
            {
                HttpClient client = new HttpClient();
                var data = client.PostAsJsonAsync($"{_configuration["APIBaseAddress"]}{login}", account);
                var result = await data.Result.Content.ReadFromJsonAsync<LoginResponse>();
                if (result is not null)
                {
                    Response.Cookies.Append("token", result.AccessToken,
                        new CookieOptions()
                        {
                            Expires = DateTime.Now.AddSeconds(result.ExpiresIn)
                        });
                    Response.Cookies.Append("userName", account.Email,
                        new CookieOptions()
                        {
                            Expires = DateTime.Now.AddSeconds(result.ExpiresIn)
                        });
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("token");
            Response.Cookies.Delete("userName");
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
