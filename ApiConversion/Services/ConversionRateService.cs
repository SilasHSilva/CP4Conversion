using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ApiConversion.Interfaces;
using ApiConversion.Models;  
using Microsoft.Extensions.Options;



namespace ApiConversion.Services
{
    public class ConversionRateService : IConversionRate
    {
        public double BRL { get; set; }
        private readonly HttpClient _httpClient;
        private readonly string ApiKey;

        public ConversionRateService(HttpClient httpClient, IOptions<ApiConversionSettings> settings)
        {
            _httpClient = httpClient;
            ApiKey = settings.Value.ApiKey;
        }

        public async Task GetBrlRateAsync()
        {
            var url = $"https://v6.exchangerate-api.com/v6/{ApiKey}/pair/USD/BRL";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ExchangeRateResponse>();

                if (result != null && result.Result == "success")
                {
                    BRL = result.ConversionRate;
                }
                else
                {
                    throw new Exception("Failed to retrieve the conversion rate.");
                }
            }
            else
            {
                throw new Exception($"API call failed: {response.StatusCode}");
            }
        }

        private class ExchangeRateResponse
        {
            public string Result { get; set; } = string.Empty;
            public double ConversionRate { get; set; }
        }
    }
}
