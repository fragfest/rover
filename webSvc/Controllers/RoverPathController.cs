using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Drawing.Imaging;

namespace webSvc.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoverPathController : ControllerBase
    {
        private readonly ILogger<RoverPathController> _logger;

        public RoverPathController(ILogger<RoverPathController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public ActionResult Post(Path path)
        {
            var input = path.input;
            int[] test = { 1, 2, 3 };
            return Ok(test);
        }

    }

    public class Path {
        public string input { get; set; }
    }
}
