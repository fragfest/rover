using System;
using System.Drawing;
using System.IO;

namespace webSvc
{
    public class RoverImages
    {
        private int cellSizePx = 30;

        public RoverImages(string missionId){

            //TODO get rover input string from missionIndex.json
            var gridWidth = 10;
            var gridHeight = 10;
            var roverStart = "1 2 N";
            var roverInputs = "LMR";

            //hardcoded
            var path = "logo.png";
            var gridWidthPx = cellSizePx * gridWidth;
            var gridHeightPx = cellSizePx * gridHeight;

            using (var stream = File.OpenRead(path))
            {
                var bmp = new Bitmap(stream);
                var bmpOut = new Bitmap(bmp, new Size(gridWidthPx, gridHeightPx));

                var gridStartX = 0; ;
                var gridStartY = 0; ;
                var gridStartDirection = "N";
                createRoverPath(bmpOut, gridWidthPx, gridHeightPx, gridStartX, gridStartY, gridStartDirection);
                bmpOut.Save(missionId + ".png");
            }
        }

        private void createRoverPath(Bitmap bmp, int gridWidthPx, int gridHeightPx, int startX, int startY, string gridStartDirection)
        {
            for (var x = 0; x < gridWidthPx; x++)
            {
                for (var y = 0; y < gridHeightPx; y++)
                {
                    var xIndex = getCellIndex(x);
                    var yIndex = getCellIndex(y);
                    if (xIndex == startX && yIndex == startY)
                    {
                        Console.WriteLine("startX");
                    }
                    else {
                       bmp.SetPixel(x, y, Color.Black);
                    }
                }
            }
        }

        private int getCellIndex(int pixelIndex) {
            return pixelIndex / cellSizePx;
        }
    }
}
