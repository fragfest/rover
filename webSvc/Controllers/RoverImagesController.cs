using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Drawing.Imaging;
using System.Text;
using webSvc.App;
using Newtonsoft.Json;

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
        public ActionResult Get()
        {
            var json = System.IO.File.ReadAllText("mission.json");
            var missionRes = JsonConvert.DeserializeObject<MissionRes>(json);

            if (missionRes.rovers.Count == 0) {
                return BadRequest();
            }
            var roverImages = new RoverImages();

            var bitmap = roverImages.getScreenshot(missionRes.gridWidth, missionRes.gridHeight, missionRes.rovers);
            using (var outStream = new MemoryStream())
            {
                bitmap.Save(outStream, ImageFormat.Png);
                var bytes = outStream.ToArray();
                var base64 = Convert.ToBase64String(bytes);

                return new FileStreamResult(new MemoryStream(Encoding.UTF8.GetBytes(base64)), "image/png;base64");
            }
        }

    }
}
