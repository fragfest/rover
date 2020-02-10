using System;
using System.Collections.Generic;

namespace webSvc.App
{

    public class RoverPath
    {
        public readonly int cellSizePx = 30;
        public readonly int imageWidthPx = 965;
        public readonly int imageHeightPx = 543;

        public readonly int gridWidth;
        public readonly int gridHeight;
        public readonly int gridWidthPx;
        public readonly int gridHeightPx;
        private IList<PathPoint> PathArr;

        /// <summary>
        /// <para>Grid for a rover to create a path on. Note: origin (0,0) is in the lower left corner of the grid.</para>
        /// </summary>
        public RoverPath(int gridWidth, int gridHeight, PathPoint startPoint)
        {
            var gridWidthMax = imageWidthPx / cellSizePx;
            var gridHeightMax = imageHeightPx / cellSizePx;
            this.gridWidth = gridWidth < gridWidthMax ? gridWidth : gridWidthMax;
            this.gridWidthPx = this.gridWidth * cellSizePx;
            this.gridHeight = gridHeight < gridHeightMax ? gridHeight : gridHeightMax;
            this.gridHeightPx = this.gridHeight * cellSizePx;
            PathArr = new List<PathPoint>();
            SetStart(startPoint);
        }

        private void SetStart(PathPoint startPoint)
        {
            var x = startPoint.X > 0 ? startPoint.X : 0;
            var y = startPoint.Y > 0 ? startPoint.Y : 0;
            var startX = x < gridWidth ? x : gridWidth - 1;
            var startY = y < gridHeight ? y : gridHeight - 1;
            var startDir = startPoint.Dir;
            PathArr.Add(
                new PathPoint(startX, startY, startDir)
            );
        }

        /// <summary>
        /// <para>Get number of grid positions rover has explored.</para>
        /// </summary>
        public int GetNumPositions()
        {
            return PathArr.Count;
        }

        /// <summary>
        /// <para>Get grid point that rover has explored using zero-based index.</para>
        /// </summary>
        public PathPoint GetPathPoint(int index)
        {
            var point = PathArr[index];
            return new PathPoint(
                point.X,
                point.Y,
                point.Dir
            );
        }

        /// <summary>
        /// <para>create a rover path using an input string.</para>
        /// </summary>
        public void createRoverPath(string inputStr)
        {

            var controlChars = inputStr.ToCharArray();
            foreach (var c in controlChars)
            {
                addToPath(c);
            }
        }

        private void addToPath(char command)
        {
            var count = PathArr.Count;
            var index = count - 1;
            var lastPoint = PathArr[index];

            switch (char.ToUpper(command))
            {
                case 'L':
                    var dirLeft = rotateLeft(lastPoint);
                    PathArr[index] = new PathPoint(lastPoint.X, lastPoint.Y, dirLeft);
                    break;
                case 'R':
                    var dirRight = rotateRight(lastPoint);
                    PathArr[index] = new PathPoint(lastPoint.X, lastPoint.Y, dirRight);
                    break;
                case 'M':
                    var newPoint = moveForward(lastPoint);
                    if (newPoint != null) {
                        PathArr.Add(newPoint);
                    }
                    break;
            }
        }

        private PathPoint moveForward(PathPoint prevPoint) {
            var newX = prevPoint.X;
            var newY = prevPoint.Y;
            switch (prevPoint.Dir) {
                case PathDirection.North:
                    newY = prevPoint.Y + 1;
                    break;
                case PathDirection.South:
                    newY = prevPoint.Y - 1;
                    break;
                case PathDirection.East:
                    newX = prevPoint.X + 1;
                    break;
                case PathDirection.West:
                    newX = prevPoint.X - 1;
                    break;
            }

            var gridWidthOutOfBounds = false;
            var gridHeightOutOfBounds = false;
            if (newX < 0 || newX > gridWidth - 1) {
                gridWidthOutOfBounds = true;
            }
            if(newY < 0 || newY > gridHeight - 1) {
                gridHeightOutOfBounds = true;
            }

            if (gridWidthOutOfBounds || gridHeightOutOfBounds)
            {
                return null;
            }

            return new PathPoint(newX, newY, prevPoint.Dir);
        }

        private PathDirection rotateRight(PathPoint prevPoint)
        {
            var dirIntMax = getPathDirectionIntMax();
            var dirInt = (int)prevPoint.Dir;
            var dirIntRotated = dirInt + 1;
            var dirIntNew = dirIntRotated <= dirIntMax ? dirIntRotated : 0;
            return (PathDirection)dirIntNew;
        }

        private PathDirection rotateLeft(PathPoint prevPoint)
        {
            var dirIntMax = getPathDirectionIntMax();
            var dirInt = (int)prevPoint.Dir;
            var dirIntRotated = dirInt - 1;
            var dirIntNew = dirIntRotated >= 0 ? dirIntRotated : dirIntMax;
            return (PathDirection)dirIntNew;
        }

        private int getPathDirectionIntMax() {
            var dirIntArr = (int[])Enum.GetValues(typeof(PathDirection));
            return dirIntArr[dirIntArr.Length - 1];
        }
    }


    public class PathPoint
    {
        public int X { private set; get; }
        public int Y { private set; get; }
        public PathDirection Dir { private set; get; }

        public PathPoint(int x, int y, string dir)
        {
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
        North = 0,
        East,
        South,
        West
    }
}
