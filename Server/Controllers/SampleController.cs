using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SampleController : ControllerBase
    {
        [HttpGet]
        public IActionResult SimulatePrematureResponse()
        {
            string filePath = @"C:\Users\Truong Nhon\Desktop\simulator\Server\MOCK_DATA.json";
            try
            {
                // Read the JSON data from the file
                string jsonData = System.IO.File.ReadAllText(filePath);

                // Return the JSON data as the response
                return Content(jsonData, "application/json");
            }
            catch (IOException ex)
            {
                // Handle the exception if the file read fails
                return StatusCode(500, $"Error reading JSON file: {ex.Message}");
            }
        }
    }
}