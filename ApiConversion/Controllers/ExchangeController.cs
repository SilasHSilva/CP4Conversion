using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiConversion.Interfaces;
using ApiConversion.Models;

namespace ApiConversion.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ExchangeController : ControllerBase, IExchangeController
    {
        private readonly HttpClient _httpClient;

        public ExchangeController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("rate")]
        [Tags("Consulta de moedas")]
        [ProducesResponseType(typeof(ApiConversionSettings), 200)]
        [ProducesResponseType(404)]
        public async Task<JsonResult> GetExchangeRate()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("https://v6.exchangerate-api.com/v6/a81cef629dc33f7a944273a6/pair/USD/BRL");

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    return new JsonResult(new { success = true, data = responseData });
                }
                else
                {
                    return new JsonResult(new { success = false, error = $"Erro na requisição: {response.StatusCode}" });
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, error = $"Ocorreu um erro: {ex.Message}" });
            }
        }
    }
}
