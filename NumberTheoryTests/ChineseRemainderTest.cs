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
        
        [TestMethod]
        public void TestStrongChineseRemainder()
        {
            var (val, mod) = CRTStrong([3, 5], [10, 12], out bool success);
            Assert.IsTrue(success);
            Assert.AreEqual(53, val);
            Assert.AreEqual(60, mod);

            (val, mod) = CRTStrong([2, 2, 6, 11], [6, 8, 10, 15], out success);
            Assert.IsTrue(success);
            Assert.AreEqual(26, val);
            Assert.AreEqual(120, mod);

            CRTStrong([3, 6], [10, 12], out success);
            Assert.IsFalse(success);
        }
    }
}
