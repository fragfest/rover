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

namespace webSvc.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MissionController : ControllerBase
    {
        private readonly ILogger<MissionController> _logger;

        public MissionController(ILogger<MissionController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Post(MissionReq pathReq)
        {
            var input = pathReq.rovers[0].input ?? "";
            var startX = pathReq.rovers[0].startX;
            var startY = pathReq.rovers[0].startY;
            var startDir = pathReq.rovers[0].startDir ?? "";
            var gridWidth = pathReq.gridWidth;
            var gridHeight = pathReq.gridHeight;

            var startPoint = new PathPoint(startX, startY, startDir);
            var roverPath = new RoverPath(gridWidth, gridHeight, startPoint);
            roverPath.createRoverPath(input);
            var pathCount = roverPath.GetNumPositions();

            var path = new List<ResPoint>();
            for (var i = 0; i < pathCount; i++)
            {
                var pathPoint = roverPath.GetPathPoint(i);
                var point = new ResPoint();
                point.x = pathPoint.X;
                point.y = pathPoint.Y;
                point.dir = pathPoint.Dir.ToString();
                path.Add(point);
            }

            var res = new MissionRes();
            res.lastPoint = path[path.Count - 1];
            res.input = input;
            return Ok(res);
        }

    }

    public class MissionRes
    {
        public string input { get; set; }
        public ResPoint lastPoint { get; set; }
    }

    public class MissionReq
    {
        public int gridWidth { get; set; }
        public int gridHeight { get; set; }
        public List<MissionRover> rovers { get; set; }
    }

    public class MissionRover
    {
        public string input { get; set; }
        public int startX { get; set; }
        public int startY { get; set; }
        public string startDir { get; set; }
    }

}
