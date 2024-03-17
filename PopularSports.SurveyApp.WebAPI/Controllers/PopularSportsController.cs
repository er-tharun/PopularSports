using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PopularSports.SurveyApp.WebAPI.Models;
using PopularSports.SurveyApp.WebAPI.Services;

namespace PopularSports.SurveyApp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PopularSportsController : ControllerBase
    {
        private readonly IPopularSportsService _popularSportsService;

        public PopularSportsController(IPopularSportsService popularSportsService)
        {
            _popularSportsService = popularSportsService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        [Route("GetSport")]
        public IActionResult GetActiveSports()
        {
            return new OkObjectResult(_popularSportsService.GetAllActiveSports());
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("CreateSport")]
        public IActionResult CreateSport([FromBody] Sport sport)
        {
            var isSuccess = _popularSportsService.AddSport(sport);
            return isSuccess ? Created() : BadRequest();
        }
        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("UpdateSport")]
        public IActionResult UpdateSport([FromBody] Sport sport)
        {
            var isSuccess = _popularSportsService.UpdateSport(sport, sport.Id);
            return isSuccess ? Ok() : BadRequest();
        }
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("DeleteSport")]
        public IActionResult DeleteSport([FromQuery] int id)
        {
            var isSuccess = _popularSportsService.RemoveSport(id);
            return isSuccess ? Ok() : BadRequest();
        }
    }
}
