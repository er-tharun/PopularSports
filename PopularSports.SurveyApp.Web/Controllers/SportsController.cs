using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PopularSports.SurveyApp.WebAPI.Models;
using System.Net;

namespace PopularSports.SurveyApp.Web.Controllers
{
    public class SportsController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string GET_SPORT = "/api/PopularSports/GetSport";
        private readonly string ADD_SURVEY = "/api/Survey/AddSurvey";
        private readonly string GET_LEADINGSURVEY = "/api/Survey/LeadingSurvey";
        private readonly string GET_SURVEYS = "/api/Survey/GetSurveys";

        public SportsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            HttpClient client = new HttpClient();
            if(Request.Cookies.TryGetValue("token", out var bearerToken))
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + bearerToken);
                try
                {
                    var data = await client.GetFromJsonAsync<Survey>($"{_configuration["APIBaseAddress"]}{GET_LEADINGSURVEY}");
                    return View(data);
                }
                catch(HttpRequestException ex)
                {
                    if (ex.StatusCode == HttpStatusCode.Forbidden)
                        return RedirectToAction("AccessDenied");
                    else if (ex.StatusCode == HttpStatusCode.Unauthorized)
                        return RedirectToAction("Login", "Account");
                }
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> GetSport()
        {
            HttpClient client = new HttpClient();

            if (Request.Cookies.TryGetValue("token", out var bearerToken))
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + bearerToken);
                try
                {
                    var data = await client
                    .GetFromJsonAsync<IEnumerable<Sport>>
                    ($"{_configuration["APIBaseAddress"]}{GET_SPORT}");
                    if (data is not null)
                    {
                        return View(data);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                catch (HttpRequestException ex)
                {
                    if (ex.StatusCode == HttpStatusCode.Forbidden)
                        return RedirectToAction("AccessDenied");
                    else if (ex.StatusCode == HttpStatusCode.Unauthorized)
                        return RedirectToAction("Login", "Account");
                }
                return RedirectToAction("Login", "Account");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpGet]
        public IActionResult AddSport()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddSport(Sport sport)
        {
            HttpClient httpClient = new HttpClient();
            if(Request.Cookies.TryGetValue("token",out var bearerToken))
            {
                try
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + bearerToken);
                    var data = await httpClient.PostAsJsonAsync("", sport);
                    if(data.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
                catch(HttpRequestException ex)
                {
                    return RedirectToAction("AccessDenied");
                }
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> CastVoteToSport()
        {
            HttpClient client = new HttpClient();

            if (Request.Cookies.TryGetValue("token", out var bearerToken))
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + bearerToken);
                try
                {
                    var data = await client
                    .GetFromJsonAsync<IEnumerable<Sport>>
                    ($"{_configuration["APIBaseAddress"]}{GET_SPORT}");
                    if (data is not null)
                    {
                        //var sports = data.Select(a => a.Name);
                        var sportsList = new List<SelectListItem>();
                        foreach(var sport in data)
                        {
                            sportsList.Add(new SelectListItem() { Text = sport.Name, Value = sport.Id.ToString() });
                        }
                        ViewBag.Sports = sportsList;
                        return View();
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                catch(HttpRequestException ex)
                {
                    if (ex.StatusCode == HttpStatusCode.Forbidden)
                        return RedirectToAction("AccessDenied");
                    else if (ex.StatusCode == HttpStatusCode.Unauthorized)
                        return RedirectToAction("Login", "Home");
                }
                return RedirectToAction("Login", "Account");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CastVoteToSport(IFormCollection sportId)
        {
            HttpClient client = new HttpClient();
            if(Request.Cookies.TryGetValue("token", out string bearerToken))
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + bearerToken);
                try
                {
                    var data = await client.PostAsJsonAsync($"{_configuration["APIBaseAddress"]}{ADD_SURVEY}?sportId={Convert.ToInt16(sportId["Sport.Name"])}", sportId);
                    if(!data.IsSuccessStatusCode)
                    {
                        return RedirectToAction("AccessDenied");
                    }
                }
                catch(HttpRequestException ex)
                {
                    if (ex.StatusCode == HttpStatusCode.Forbidden)
                        return RedirectToAction("AccessDenied");
                    else if(ex.StatusCode == HttpStatusCode.Unauthorized)
                        return RedirectToAction("Login", "Account");
                }
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> GetReports()
        {
            HttpClient httpClient = new HttpClient();
            if( Request.Cookies.TryGetValue("token",out var bearerToken))
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + bearerToken);
                try
                {
                    var data = await httpClient.GetFromJsonAsync<List<Survey>>($"{_configuration["APIBaseAddress"]}{GET_SURVEYS}");
                    return View(data);
                }
                catch(HttpRequestException ex)
                {
                    if (ex.StatusCode == HttpStatusCode.Forbidden)
                        return RedirectToAction("AccessDenied");
                    else if (ex.StatusCode == HttpStatusCode.Unauthorized)
                        return RedirectToAction("Login", "Account");
                }
            }
            return RedirectToAction("Login", "Account");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
