using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExternalCallController : ControllerBase
    {
        private readonly string apiUrl = "http://localhost:5225/api/Sample";
        // [HttpGet]
        // public async Task<IActionResult> CallExternalEndpoint()
        // {
        //     using (var httpClient = new HttpClient())
        //     {
        //         try
        //         {
        //             // Number of tasks to create
        //             int numberOfTasks = 10;

        //             // Create a list to store the tasks
        //             var tasks = new List<Task<HttpResponseMessage>>();

        //             // Start multiple asynchronous HTTP requests
        //             for (int i = 0; i < numberOfTasks; i++)
        //             {
        //                 tasks.Add(httpClient.GetAsync(apiUrl));
        //             }

        //             // Wait for all tasks to complete
        //             await Task.WhenAll(tasks);

        //             // Process the responses
        //             var results = new List<string>();
        //             foreach (var task in tasks)
        //             {
        //                 var response = await task;
        //                 response.EnsureSuccessStatusCode();
        //                 string result = await response.Content.ReadAsStringAsync();
        //                 results.Add(result);
        //             }

        //             return Ok(results);
        //         }
        //         catch (Exception ex)
        //         {
        //             throw new Exception($"HttpRequestException: {ex.Message}");
        //         }
        //     }
        // }
        [HttpGet]
        public async Task<IActionResult> CallExternalEndpoint()
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    // Number of tasks to create
                    int numberOfTasks = 20;

                    // Create a list to store the tasks
                    var tasks = new List<Task<HttpResponseMessage>>();

                    // Start multiple asynchronous HTTP requests
                    for (int i = 0; i < numberOfTasks; i++)
                    {
                        tasks.Add(httpClient.GetAsync(apiUrl));
                        System.Console.WriteLine("Done: Add loop " + i);
                    }

                    // Wait for all tasks to complete
                    await Task.WhenAll(tasks);

                    // Process the responses
                    var results = new List<string>();
                    foreach (var task in tasks)
                    {
                        try
                        {
                            var response = await task;
                            response.EnsureSuccessStatusCode();
                            string result = await response.Content.ReadAsStringAsync();
                            results.Add(result);
                            System.Console.WriteLine("Done: Add result" + task.ToString());
                        }
                        catch (HttpRequestException ex) 
                        when (ex.Message.Contains("The response ended prematurely"))
                        {
                            return StatusCode(500, "An error occurred while processing the response from the external API.");
                        }
                    }

                    Console.WriteLine("All requests completed.");

                    return Ok(results);
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