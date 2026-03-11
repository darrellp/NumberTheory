using Microsoft.VisualStudio.TestTools.UnitTesting;
using static NumberTheory.ChineseRemainder;

namespace NumberTheoryTests
{
    [TestClass]
    public class ChineseRemainderTests
    {
        [TestMethod]
        public void TestChineseRemainder()
        {
            Assert.AreEqual(103L, CRT(new long[] { 1, 3, 5 }, new long[] { 3, 5, 7 }));
        }
    }
}
