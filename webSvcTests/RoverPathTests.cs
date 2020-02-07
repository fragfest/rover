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

            Assert.AreEqual(0, roverPath.PathArr.Count);
            Assert.AreEqual(8, roverPath.gridWidth);
            Assert.AreEqual(10, roverPath.gridHeight);
            Assert.AreEqual(8, roverPath.startPoint.X);
            Assert.AreEqual(2, roverPath.startPoint.Y);
            Assert.AreEqual(PathDirection.North, roverPath.startPoint.Dir);
            Assert.AreEqual(0, roverPath.PathArr.Count);

            startPoint = new PathPoint(20, 2, "S");
            roverPath = new RoverPath(8, 10, startPoint);
            Assert.AreEqual(PathDirection.South, roverPath.startPoint.Dir);
        }
    }
}