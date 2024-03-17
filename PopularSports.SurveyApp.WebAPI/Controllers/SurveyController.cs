using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PopularSports.SurveyApp.WebAPI.Services;

namespace PopularSports.SurveyApp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyController : ControllerBase
    {
        private readonly IPopularSportsService _popularSportsService;

        public SurveyController(IPopularSportsService popularSportsService)
        {
            _popularSportsService = popularSportsService;
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        [Route("AddSurvey")]
        public IActionResult AddSurvey([FromQuery] int sportId)
        {
            var isSuccess = _popularSportsService.AddVoteForASport(sportId);
            return isSuccess ? Created() : BadRequest();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("LeadingSurvey")]
        public IActionResult GetLeadingSurvey()
        {
            var data = _popularSportsService.GetLeadingSport();
            return Ok(data);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("GetSurveys")]
        public async Task<IActionResult> GetSurveys()
        {
            return new OkObjectResult(_popularSportsService.GetAllSurveys());
        }
    }
}
