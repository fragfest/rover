using System;
using System.Collections.Generic;

namespace webSvc.App
{
    //TODO origin of RoverPath grid is in lower left corner. annotate?
    public class RoverPath
    {
        public readonly int cellSizePx = 30;
        public readonly int imageWidthPx = 640;
        public readonly int imageHeightPx = 480;

        public readonly int gridWidth;
        public readonly int gridHeight;
        public PathPoint startPoint { private set; get; }
        public List<PathPoint> PathArr { private set; get; }

        public RoverPath(int gridWidth, int gridHeight)
        {
            var gridWidthMax = imageWidthPx / cellSizePx;
            var gridHeightMax = imageHeightPx / cellSizePx;
            this.gridWidth = gridWidth < gridWidthMax ? gridWidth : gridWidthMax;
            this.gridHeight = gridHeight < gridHeightMax ? gridHeight : gridHeightMax;
            this.PathArr = new List<PathPoint>();
        }

        //TODO only or also accept a string with raw: 1 2 N
        public void SetStart(int x, int y, string direction) {
            var startX = x < gridWidth ? x : gridWidth;
            var startY = y < gridHeight ? y : gridHeight;
            var startDir = "N";

            switch (direction.ToUpper()) {
                case "W":
                    startDir = "W";
                    break;
                case "E":
                    startDir = "E";
                    break;
                case "S":
                    startDir = "S";
                    break;
                case "N":
                    startDir = "N";
                    break;
            }
            this.startPoint = new PathPoint(startX, startY, startDir);
        }

        public void createRoverPath(string controlStr) {
            //TODO init start fields if null

            var controlChars = controlStr.ToCharArray();
            foreach (var c in controlChars) {
                this.addPath(c);
            }
        }

        private void addPath(char command) {
            PathPoint nextPoint = null;
            var startPoint = this.startPoint ?? new PathPoint(0, 0, "N");
            var count = this.PathArr.Count;
            var lastPoint = count > 0 ? this.PathArr[count - 1] : startPoint ;

            switch (char.ToUpper(command)) {
                case 'L':
                    nextPoint = this.rotateWest(lastPoint);
                    break;
            }
            if (nextPoint != null) {
                this.PathArr.Add(nextPoint);
            }
        }

        private PathPoint rotateWest(PathPoint prevPoint) {
            return new PathPoint(prevPoint.X, prevPoint.Y, prevPoint.Dir);
        }
    }

    public class PathPoint {
        public int X { private set; get; }
        public int Y { private set; get; }
        public string Dir { private set; get; }

        public PathPoint(int x, int y, string dir) {
            this.X = x;
            this.Y = y;
            this.Dir = dir;
        }
    }
}
