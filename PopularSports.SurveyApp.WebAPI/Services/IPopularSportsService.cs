using PopularSports.SurveyApp.WebAPI.Models;

namespace PopularSports.SurveyApp.WebAPI.Services
{
    public interface IPopularSportsService
    {
        bool AddSport(Sport sport);
        bool RemoveSport(int id);
        bool UpdateSport(Sport sport, int id);
        Sport? GetSport(int id);
        IEnumerable<Sport> GetAllSports();
        IEnumerable<Sport> GetAllActiveSports();
        bool AddVoteForASport(int sportId);
        bool RemoveVoteForASport(int sportid);
        Survey? GetLeadingSport();
        Survey? GetSurveyForSport(int sportId);
        IEnumerable<Survey> GetAllSurveys();

    }
}
