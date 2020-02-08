using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using webSvc.App;

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
        public IActionResult Post(PathReq pathReq)
        {
            var input = pathReq.input ?? "";
            var startX = pathReq.startX;
            var startY = pathReq.startY;
            var startDir = pathReq.startDir ?? "";
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

            var res = new PathRes();
            res.path = path;
            res.input = input;
            return Ok(res);
        }

    }

    public class PathRes
    {
        public string input { get; set; }
        public List<ResPoint> path { get; set; }
    }

    public class ResPoint
    {
        public int x { get; set; }
        public int y { get; set; }
        public string dir { get; set; }
    }

    public class PathReq
    {
        public string input { get; set; }
        public int startX { get; set; }
        public int startY { get; set; }
        public string startDir { get; set; }
        public int gridWidth { get; set; }
        public int gridHeight { get; set; }
    }
}
