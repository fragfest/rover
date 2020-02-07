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

        public RoverPath(int gridWidth, int gridHeight, PathPoint startPoint)
        {
            var gridWidthMax = imageWidthPx / cellSizePx;
            var gridHeightMax = imageHeightPx / cellSizePx;
            this.gridWidth = gridWidth < gridWidthMax ? gridWidth : gridWidthMax;
            this.gridHeight = gridHeight < gridHeightMax ? gridHeight : gridHeightMax;
            this.PathArr = new List<PathPoint>();
            SetStart(startPoint);
        }

        private void SetStart(PathPoint startPoint) {
            var x = startPoint.X > 0 ? startPoint.X : 0;
            var y = startPoint.Y > 0 ? startPoint.Y : 0;
            var startX = x < gridWidth ? x : gridWidth;
            var startY = y < gridHeight ? y : gridHeight;
            var startDir = startPoint.Dir;
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
                    //nextPoint = this.rotateWest(lastPoint);
                    break;
            }
            if (nextPoint != null) {
                this.PathArr.Add(nextPoint);
            }
        }

        //private PathPoint rotateWest(PathPoint prevPoint) {
        //    return new PathPoint(prevPoint.X, prevPoint.Y, prevPoint.Dir);
        //}
    }

    //TODO move class and enum to new file
    public class PathPoint {
        public int X { private set; get; }
        public int Y { private set; get; }
        public PathDirection Dir { private set; get; }

        public PathPoint(int x, int y, string dir) {
            this.X = x;
            this.Y = y;

            switch (dir.ToUpper())
            {
                case "W":
                    this.Dir = PathDirection.West;
                    break;
                case "E":
                    this.Dir = PathDirection.East;
                    break;
                case "S":
                    this.Dir = PathDirection.South;
                    break;
                case "N":
                default:
                    this.Dir = PathDirection.North;
                    break;
            }
        }

        public PathPoint(int x, int y, PathDirection dir)
        {
            this.X = x;
            this.Y = y;
            this.Dir = dir;
        }
    }

    public enum PathDirection
    {
        West,
        East,
        South,
        North
    }
}
