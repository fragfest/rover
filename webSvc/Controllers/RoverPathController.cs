﻿using System;
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

            var path = new List<PathPoint>();
            for (var i = 0; i < pathCount; i++)
            {
                path.Add(roverPath.GetPathPoint(i));
            }

            var res = new PathRes();
            res.path = path;
            return Ok(res);
        }

    }

    public class PathRes
    {
        public List<PathPoint> path { get; set; }
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
