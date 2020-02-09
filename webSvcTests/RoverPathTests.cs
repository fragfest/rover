using NUnit.Framework;
using webSvc.App;

namespace webSvcTests
{
    public class RoverPathTests
    {
        public RoverPath roverPath;

        [SetUp]
        public void Setup()
        {
            var startPoint = new PathPoint(0, 0, "N");
            roverPath = new RoverPath(10, 10, startPoint);
        }

        [Test]
        public void Constructor()
        {
            var startPoint = new PathPoint(20, 2, "UNKOWN");
            roverPath = new RoverPath(8, 10, startPoint);

            Assert.AreEqual(1, roverPath.GetNumPositions());
            Assert.AreEqual(8, roverPath.gridWidth);
            Assert.AreEqual(10, roverPath.gridHeight);
            Assert.AreEqual(7, roverPath.GetPathPoint(0).X);
            Assert.AreEqual(2, roverPath.GetPathPoint(0).Y);
            Assert.AreEqual(PathDirection.North, roverPath.GetPathPoint(0).Dir);

            startPoint = new PathPoint(20, 11, "S");
            roverPath = new RoverPath(8, 10, startPoint);
            Assert.AreEqual(PathDirection.South, roverPath.GetPathPoint(0).Dir);
            Assert.AreEqual(roverPath.GetPathPoint(0).X, 7);
            Assert.AreEqual(roverPath.GetPathPoint(0).Y, 9);
            Assert.AreEqual(roverPath.GetPathPoint(0).Dir, PathDirection.South);
        }

        [Test]
        public void createRoverPath_RotateInPlaceAndMove() {
            roverPath.createRoverPath("LLLRRRRM");
            Assert.AreEqual(2, roverPath.GetNumPositions());
            Assert.AreEqual(0, roverPath.GetPathPoint(0).X);
            Assert.AreEqual(0, roverPath.GetPathPoint(0).Y);
            Assert.AreEqual(PathDirection.East, roverPath.GetPathPoint(0).Dir);
            Assert.AreEqual(1, roverPath.GetPathPoint(1).X);
            Assert.AreEqual(0, roverPath.GetPathPoint(1).Y);
            Assert.AreEqual(PathDirection.East, roverPath.GetPathPoint(1).Dir);
        }

        [Test]
        public void createRoverPath_MoveOutsideGrid()
        {
            roverPath.createRoverPath("LLMRRMLM");
            Assert.AreEqual(2, roverPath.GetNumPositions());
            Assert.AreEqual(0, roverPath.GetPathPoint(1).X);
            Assert.AreEqual(1, roverPath.GetPathPoint(1).Y);
            Assert.AreEqual(PathDirection.West, roverPath.GetPathPoint(1).Dir);

            roverPath.createRoverPath("RRMMMMMMMMMMM");
            Assert.AreEqual(11, roverPath.GetNumPositions());
            Assert.AreEqual(9, roverPath.GetPathPoint(10).X);
            Assert.AreEqual(1, roverPath.GetPathPoint(10).Y);
            Assert.AreEqual(PathDirection.East, roverPath.GetPathPoint(10).Dir);
        }

        [Test]
        public void createRoverPath_TestExamples()
        {
            //RoverPath constructor uses plateau size not upper right coordinates
            //plateau size is 1 unit bigger
            var upperRightX = 5;
            var upperRightY = 5;

            var startX = 1;
            var startY = 2;
            var startDir = "N";
            var instructions = "LMLMLMLMM";

            var startPoint = new PathPoint(startX, startY, startDir);
            roverPath = new RoverPath(upperRightX + 1, upperRightY + 1, startPoint);
            roverPath.createRoverPath(instructions);
            var numPositions = roverPath.GetNumPositions();
            Assert.AreEqual(1, roverPath.GetPathPoint(numPositions - 1).X);
            Assert.AreEqual(3, roverPath.GetPathPoint(numPositions - 1).Y);
            Assert.AreEqual(PathDirection.North, roverPath.GetPathPoint(numPositions - 1).Dir);

            startX = 3;
            startY = 3;
            startDir = "E";
            instructions = "MMRMMRMRRM";

            startPoint = new PathPoint(startX, startY, startDir);
            roverPath = new RoverPath(upperRightX + 1, upperRightY + 1, startPoint);

            roverPath.createRoverPath(instructions);
            numPositions = roverPath.GetNumPositions();
            Assert.AreEqual(5, roverPath.GetPathPoint(numPositions - 1).X);
            Assert.AreEqual(1, roverPath.GetPathPoint(numPositions - 1).Y);
            Assert.AreEqual(PathDirection.East, roverPath.GetPathPoint(numPositions - 1).Dir);
        }

    }
}