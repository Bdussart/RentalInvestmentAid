
using RentalInvestmentAid.Caching;
using RentalInvestmentAid.Database;
using RentalInvestmentAid.GeminiAPICaller;
using RentalInvestmentAid.Models.Announcement;
using RentalInvestmentAid.Models.Gemini;

namespace RentalInvestmentAid.Core.Gemini
{
    public class GeminiTreatment : MustInitializeCache
    {
        private IDatabaseFactory _databaseFactory;
        public GeminiTreatment(CachingManager cachingManager, IDatabaseFactory databaseFactory) : base(cachingManager)
        {
            _databaseFactory = databaseFactory;
            base._cachingManager = cachingManager;
        }

        public async Task<string> GetPromptInformation(AnnouncementInformation announcement)
        {
            string result = string.Empty;

            GeminiPromptData promptData = new GeminiPromptData
            {
                Context = _cachingManager.GetMiscPrompt(),
                Informations = $"La ville : {announcement.CityInformations.CityName} code postal  : {announcement.CityInformations.ZipCode} prix : {announcement.Price} metrage : {announcement.Metrage} type : {announcement.RentalType.ToString()} descrption {announcement.Description}"
            };

            GeminiAPICall caller = new GeminiAPICall();
            result = await caller.SendPromptAsync(promptData);


            if (!String.IsNullOrWhiteSpace(result))
                _databaseFactory.InsertInformationProvidedByGeminiForAnAnnouncement(announcement.Id, result);

            return result;
        }
    }
}
