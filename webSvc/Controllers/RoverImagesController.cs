using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace webSvc.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoverImagesController : ControllerBase
    {
        private readonly ILogger<RoverImagesController> _logger;

        public RoverImagesController(ILogger<RoverImagesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ArrayList Get()
        {
            var roverImages = new RoverImages("1");
            return new ArrayList();
        }
    }
}
