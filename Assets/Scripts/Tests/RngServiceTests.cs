using BusinessLife.Core;
using NUnit.Framework;

namespace BusinessLife.Tests
{
    public class RngServiceTests
    {
        [Test]
        public void DeterministicSequencesMatch()
        {
            var rngA = new RngService(12345);
            var rngB = new RngService(12345);
            for (var i = 0; i < 10; i++)
            {
                Assert.AreEqual(rngA.Next(0, 100), rngB.Next(0, 100));
            }
        }
    }
}
