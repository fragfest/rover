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
            var gridWidth = pathReq.gridWidth;
            var gridHeight = pathReq.gridHeight;

            var lastPoints = new List<MissionResPoint>();
            foreach (var rover in pathReq.rovers)
            {
                var startPoint = new PathPoint(rover.startX, rover.startY, rover.startDirChar);
                var roverPath = new RoverPath(gridWidth, gridHeight, startPoint);
                roverPath.createRoverPath(rover.input);
                var lastIndex = roverPath.GetNumPositions() - 1;
                var firstPoint = roverPath.GetPathPoint(0);
                var lastPoint = roverPath.GetPathPoint(lastIndex);

                var resPoint = new MissionResPoint();
                resPoint.firstX = firstPoint.X;
                resPoint.firstY = firstPoint.Y;
                resPoint.firstDir = firstPoint.Dir.ToString();
                resPoint.lastX = lastPoint.X;
                resPoint.lastY = lastPoint.Y;
                resPoint.lastDir = lastPoint.Dir.ToString();
                resPoint.input = rover.input;
                lastPoints.Add(resPoint);
            }

            var res = new MissionRes();
            res.gridWidth = gridWidth;
            res.gridHeight = gridHeight;
            res.rovers = lastPoints;
            return Ok(res);
        }

    }

    public class MissionResPoint
    {
        public int firstX { get; set; }
        public int firstY { get; set; }
        public string firstDir { get; set; }
        public int lastX { get; set; }
        public int lastY { get; set; }
        public string lastDir { get; set; }
        public string input { get; set; }
    }

    public class MissionRes
    {
        public int gridWidth { get; set; }
        public int gridHeight { get; set; }
        public List<MissionResPoint> rovers { get; set; }
    }

    public class MissionReq
    {
        public int gridWidth { get; set; }
        public int gridHeight { get; set; }
        public List<MissionReqRover> rovers { get; set; }
    }

    public class MissionReqRover
    {
        public string input { get; set; }
        public int startX { get; set; }
        public int startY { get; set; }
        public string startDirChar { get; set; }
    }

}
