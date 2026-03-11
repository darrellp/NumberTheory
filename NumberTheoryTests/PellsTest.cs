using Microsoft.VisualStudio.TestTools.UnitTesting;
using static NumberTheory.Pells;

namespace NumberTheoryTests
{
    [TestClass]
    public class PellsTests
    {
        [TestMethod]
        public void TestPells()
        {
            long x, y;
            SolvePells(19L, 1, 1L, out x, out y);
            Assert.AreEqual(170L, x);
            Assert.AreEqual(39L, y);
            SolvePells(19L, 1, 2L, out x, out y);
            Assert.AreEqual(57799L, x);
            Assert.AreEqual(13260L, y);
            Assert.IsFalse(SolvePells(19L, -1, 1L, out x, out y));
            Assert.IsFalse(SolvePells(19L, 7, 1L, out x, out y));
            SolvePells(13L, -1, 1L, out x, out y);
            Assert.AreEqual(18L, x);
            Assert.AreEqual(5L, y);
            SolvePells(13L, -1, 2L, out x, out y);
            Assert.AreEqual(23382L, x);
            Assert.AreEqual(6485L, y);
            SolvePells(13L, 1, 1L, out x, out y);
            Assert.AreEqual(649L, x);
            Assert.AreEqual(180L, y);
            SolvePells(13L, 1, 2L, out x, out y);
            Assert.AreEqual(842401L, x);
            Assert.AreEqual(233640L, y);
        }
    }
}
