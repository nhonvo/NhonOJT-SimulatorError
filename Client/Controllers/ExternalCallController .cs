using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExternalCallController : ControllerBase
    {
        private readonly string apiUrl = "http://localhost:5225/api/Sample";
        [HttpGet]
        public async Task<IActionResult> CallExternalEndpoint()
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();
                    string result = await response.Content.ReadAsStringAsync();
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    throw new Exception($"HttpRequestException: {ex.Message}");
                }
            }
        }
        [HttpGet("simulate-error")]
        public async Task<IActionResult> SimulateError()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                    // Add an artificial delay to simulate a slow response.
                    await Task.Delay(TimeSpan.FromSeconds(10));

                    // Read the response content asynchronously.
                    string responseContent = await response.Content.ReadAsStringAsync();

                    return Ok(responseContent);
                }
            }
            catch (HttpRequestException ex)
            {
                // Log the exception or handle it appropriately.
                return StatusCode(500, ex.Message);
            }
        }
    }
}