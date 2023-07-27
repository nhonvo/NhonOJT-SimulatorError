using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public WeatherController()
    {
        _httpClient = new HttpClient();
        // You can configure the base address of the external API here
        _httpClient.BaseAddress = new Uri("http://localhost:5225/");
    }

    [HttpGet]
    [Route("api/weather")]
    public async Task<IActionResult> GetWeatherData()
    {
        try
        {
            // Call the external API and get the response
            HttpResponseMessage response = await _httpClient.GetAsync("WeatherForecast");
            response.EnsureSuccessStatusCode();

            // Read the JSON data from the response
            string jsonData = await response.Content.ReadAsStringAsync();

            // Return the JSON data as the response
            return Content(jsonData, "application/json");
        }
        catch (HttpRequestException ex)
        {
            // Handle the exception if there is an issue with the external API
            return StatusCode(500, $"Error calling external API: {ex.Message}");
        }
    }
}
