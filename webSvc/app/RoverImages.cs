using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using webSvc.App;
using webSvc.Controllers;

namespace webSvc
{
    public class RoverImages
    {
        private readonly string marsGridImage = "mars.png";

        public RoverImages(string missionId) {
            //TODO missionId
        }

        public Bitmap getScreenshot(int gridWidth, int gridHeight, List<MissionResPoint> rovers)
        {
            var roverPaths = new List<RoverPath>();
            foreach (var rover in rovers) {
                var startPoint = new PathPoint(rover.firstX, rover.firstY, dirStrToChar(rover.firstDir));
                var roverPath = new RoverPath(gridWidth, gridHeight, startPoint);
                roverPath.createRoverPath(rover.input);
                roverPaths.Add(roverPath);
            }
            //TODO roverPaths.count == 0 exit

            using (var fileStream = File.OpenRead(marsGridImage))
            {
                var bmp = new Bitmap(fileStream);
                var gridWidthPx = roverPaths[0].gridWidthPx;
                var gridHeightPx = roverPaths[0].gridHeightPx;

                var bmpOut = new Bitmap(bmp, new Size(gridWidthPx, gridHeightPx));
                CreateRoverPathBmp(bmpOut, roverPaths);
                return bmpOut;
            }
        }

        private string dirStrToChar(string dirStr)
        {
            switch (dirStr)
            {
                case "North":
                    return "N";
                case "South":
                    return "S";
                case "East":
                    return "E";
                case "West":
                    return "W";
                default:
                    return "N";
            }
        }

        //public Bitmap getScreenshot(RoverPath roverPath)
        //{
        //    using (var fileStream = File.OpenRead(marsGridImage))
        //    {
        //        var bmp = new Bitmap(fileStream);
        //        var gridWidthPx = roverPath.gridWidthPx;
        //        var gridHeightPx = roverPath.gridHeightPx;

        //        var bmpOut = new Bitmap(bmp, new Size(gridWidthPx, gridHeightPx));
        //        createRoverPathBmp(bmpOut, roverPath);
        //        return bmpOut;
        //    }
        //}

        private void CreateRoverPathBmp(Bitmap bmp, List<RoverPath> roverPaths)
        {
            var cellSizePx = roverPaths[0].cellSizePx;
            var gridHeight = roverPaths[0].gridHeight;
            var gridWidthPx = roverPaths[0].gridWidthPx;
            var gridHeightPx = roverPaths[0].gridHeightPx;

            for (var x = 0; x < gridWidthPx; x++)
            {
                for (var y = 0; y < gridHeightPx; y++)
                {
                    var roverPathPointFound = false;

                    for (var r = 0; r < roverPaths.Count; r++)
                    {
                        var roverPathCount = roverPaths[r].GetNumPositions();

                        for (var p = 0; p < roverPathCount; p++)
                        {
                            var xIndex = getCellIndex(x, cellSizePx);
                            var yIndex = getCellIndex(y, cellSizePx);
                            var xRoverPath = roverPaths[r].GetPathPoint(p).X;
                            var yRoverPath = ConvertRoverPathToBmpYaxis(roverPaths[r].GetPathPoint(p).Y, gridHeight);
                            if (xIndex == xRoverPath && yIndex == 5)
                            {
                                roverPathPointFound = true;
                            }
                        }
                    }

                    if (!roverPathPointFound)
                    {
                        bmp.SetPixel(x, y, Color.Black);
                    }
                }
            }
        }

        private int ConvertRoverPathToBmpYaxis(int yIndexRover, int roverGridHeight)
        {
            //return roverGridHeight - 1 - yIndexRover;
            return yIndexRover;
        }

        private int getCellIndex(int pixelIndex, int cellSizePx)
        {
            return pixelIndex / cellSizePx;
        }
    }
}
