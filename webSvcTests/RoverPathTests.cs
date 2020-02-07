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
            Assert.AreEqual(8, roverPath.GetPathPoint(0).X);
            Assert.AreEqual(2, roverPath.GetPathPoint(0).Y);
            Assert.AreEqual(PathDirection.North, roverPath.GetPathPoint(0).Dir);

            startPoint = new PathPoint(20, 2, "S");
            roverPath = new RoverPath(8, 10, startPoint);
            Assert.AreEqual(PathDirection.South, roverPath.GetPathPoint(0).Dir);
            Assert.AreEqual(roverPath.GetPathPoint(0).X, 8);
            Assert.AreEqual(roverPath.GetPathPoint(0).Y, 2);
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
            Assert.AreEqual(12, roverPath.GetNumPositions());
            Assert.AreEqual(10, roverPath.GetPathPoint(11).X);
            Assert.AreEqual(1, roverPath.GetPathPoint(11).Y);
            Assert.AreEqual(PathDirection.East, roverPath.GetPathPoint(11).Dir);
        }

    }
}