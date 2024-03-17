using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PopularSports.SurveyApp.WebAPI.Data;
using PopularSports.SurveyApp.WebAPI.Models;

namespace PopularSports.SurveyApp.WebAPI.Services
{
    public class PopularSportsService : IPopularSportsService
    {
        private readonly ApplicationDbContext _context;

        public PopularSportsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool AddSport(Sport sport)
        {
            try
            {
                _context.Sports.Add(sport);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool AddVoteForASport(int sportId)
        {
            var data = GetSport(sportId);
            if(data is not null)
            {
                var surveyData = GetSurveyForSport(data.Id);
                if(surveyData is not null)
                {
                    surveyData.CountOfVotes++;
                    _context.Survey.Update(surveyData);
                }
                else
                {
                    surveyData = new Survey { Sport = data, CountOfVotes = 1 };
                    _context.Survey.Add(surveyData);
                }
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public IEnumerable<Sport> GetAllActiveSports()
        {
            return _context.Sports.Where(a => a.IsActive == true);
        }

        public IEnumerable<Sport> GetAllSports()
        {
            return _context.Sports.AsEnumerable();
        }

        public IEnumerable<Survey> GetAllSurveys()
        {
            return _context.Survey.Include(a=>a.Sport).ToList();
        }

        public Survey? GetLeadingSport()
        {
            return _context.Survey
                .Include(b => b.Sport)
                .ToList().MaxBy(a => a.CountOfVotes);
        }

        public Sport? GetSport(int id)
        {
            return _context.Sports.Where(a => a.Id == id).FirstOrDefault();
        }

        public Survey? GetSurveyForSport(int sportId)
        {
            return _context.Survey.Where(a => a.Sport.Id == sportId).FirstOrDefault();
        }

        public bool RemoveSport(int id)
        {
            var data = GetSport(id);
            if(data is not null)
            {
                _context.Sports.Remove(data);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool RemoveVoteForASport(int sportid)
        {
            var surveyData = GetSurveyForSport(sportid);
            if(surveyData is not null)
            {
                surveyData.CountOfVotes--;
                _context.Survey.Update(surveyData);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool UpdateSport(Sport sport, int id)
        {
            var sportData = GetSport(id);
            if (sportData is not null)
            {
                var dataToUpdate = new Sport
                {
                    Name = sport.Name,
                    Description = sport.Description,
                    IsActive = sport.IsActive
                };
                _context.Sports.Update(dataToUpdate);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
