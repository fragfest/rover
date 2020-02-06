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
            roverPath = new RoverPath(10, 10);
        }

        [Test]
        public void RoverPath()
        {
            roverPath.createRoverPath("L");
            Assert.AreEqual(1, roverPath.PathArr.Count);
        }
    }
}