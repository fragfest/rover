using System;
using System.Drawing;
using System.IO;
using webSvc.App;

namespace webSvc
{
    public class RoverImages
    {
        public RoverImages(string missionId)
        {
            //TODO get saved values from mission.json
            var gridWidth = 10;
            var gridHeight = 10;

            //hardcoded
            var path = "logo.png";

            using (var stream = File.OpenRead(path))
            {
                var bmp = new Bitmap(stream);

                var startPoint = new PathPoint(0, 0, "N");
                var roverPath = new RoverPath(gridWidth, gridHeight, startPoint);
                roverPath.createRoverPath("MRM");
                var gridWidthPx = roverPath.imageWidthPx;
                var gridHeightPx = roverPath.imageHeightPx;

                var bmpOut = new Bitmap(bmp, new Size(gridWidthPx, gridHeightPx));
                createRoverPathBmp(bmpOut, gridWidthPx, gridHeightPx, roverPath);

                bmpOut.Save(missionId + ".png");
            }
        }

        private void createRoverPathBmp(Bitmap bmp, int gridWidthPx, int gridHeightPx, RoverPath roverPath)
        {
            var cellSizePx = roverPath.cellSizePx;
            var gridHeight = roverPath.gridHeight;
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
            return roverGridHeight - yIndexRover;
        }

        private int getCellIndex(int pixelIndex, int cellSizePx)
        {
            return pixelIndex / cellSizePx;
        }
    }
}
