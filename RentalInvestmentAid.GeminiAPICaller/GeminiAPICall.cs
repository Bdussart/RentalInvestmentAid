using System.Net.Http.Json;
using System.Runtime.ExceptionServices;
using System.Text.Json.Serialization;
using System.Text;
using RentalInvestmentAid.Models.Gemini;
using RentalInvestmentAid.Logger;
using Newtonsoft.Json;
using RentalInvestmentAid.Settings;

namespace RentalInvestmentAid.GeminiAPICaller
{
    public class GeminiAPICall
    {
        private readonly HttpClient _httpClient;
        private readonly String _completeUrl = SettingsManager.GeminiAPIUrl;

        public GeminiAPICall()
        {
            _httpClient = new HttpClient();
        }

        async public Task<string> SendPromptAsync(GeminiPromptData data)
        {
            string result = null;
            try
            {
                HttpClient client = new HttpClient();
                string content = JsonConvert.SerializeObject(data);

                HttpResponseMessage response = await client.PostAsync($"{_completeUrl}", new StringContent(content, Encoding.UTF8, "application/json"));
                result = await HandleResponse<string>(response);
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, objectInfo: $"Url : {_completeUrl}, Message : {data}");
                ExceptionDispatchInfo.Capture(ex).Throw();
            }
            return result;
        }

        async private Task<T> HandleResponse<T>(HttpResponseMessage response)
        {
            string result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(result);
            return JsonConvert.DeserializeObject<T>(result) ?? throw new Exception("Cannot deserialize");
        }
    }
}
