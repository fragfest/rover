using System;
using System.Drawing;
using System.IO;
using webSvc.App;

namespace webSvc
{
    public class RoverImages
    {
        private readonly string marsGridImage = "mars.png";

        public RoverImages(string missionId) {
            //TODO missionId
        }

        public Bitmap getScreenshot(RoverPath roverPath)
        {
            using (var fileStream = File.OpenRead(marsGridImage))
            {
                var bmp = new Bitmap(fileStream);
                var gridWidthPx = roverPath.gridWidthPx;
                var gridHeightPx = roverPath.gridHeightPx;

                var bmpOut = new Bitmap(bmp, new Size(gridWidthPx, gridHeightPx));
                createRoverPathBmp(bmpOut, roverPath);
                return bmpOut;
            }
        }

        private void createRoverPathBmp(Bitmap bmp, RoverPath roverPath)
        {
            var cellSizePx = roverPath.cellSizePx;
            var gridHeight = roverPath.gridHeight;
            var gridWidthPx = roverPath.gridWidthPx;
            var gridHeightPx = roverPath.gridHeightPx;
            var roverPathCount = roverPath.GetNumPositions();

            for (var x = 0; x < gridWidthPx; x++)
            {
                for (var y = 0; y < gridHeightPx; y++)
                {
                    var roverPathPointFound = false;
                    for (var p = 0; p < roverPathCount; p++)
                    {
                        var xIndex = getCellIndex(x, cellSizePx);
                        var yIndex = getCellIndex(y, cellSizePx);
                        var xRoverPath = roverPath.GetPathPoint(p).X;
                        var yRoverPath = ConvertRoverPathToBmpYaxis(roverPath.GetPathPoint(p).Y, gridHeight);
                        if (xIndex == xRoverPath && yIndex == yRoverPath)
                        {
                            roverPathPointFound = true;
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
            return roverGridHeight - 1 - yIndexRover;
        }

        private int getCellIndex(int pixelIndex, int cellSizePx)
        {
            return pixelIndex / cellSizePx;
        }
    }
}
