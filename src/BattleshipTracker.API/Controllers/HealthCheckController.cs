using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BattleshipTracker.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        // GET api to return health check message of the API
        [HttpGet]
        public ActionResult<string> Get()
        {
            return Ok($"Battleship tracker api is running!\nTimestamp: {DateTime.Now.ToString()}");
        }
    }
}
