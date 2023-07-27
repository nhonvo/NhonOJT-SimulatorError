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
            // Simulate premature response by not awaiting an asynchronous operation
            return Ok("Premature response");
        }
    }
}