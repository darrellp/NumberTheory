using Microsoft.VisualStudio.TestTools.UnitTesting;

#if BIGINTEGER
using NumberTheoryBig;
using nt = System.Numerics.BigInteger;
#else
using NumberTheoryLong;
using nt = System.Int64;
#endif

namespace NumberTheoryTests
{
    [TestClass]
    public class ChineseRemainderTests
    {
        [TestMethod]
        public void TestChineseRemainder()
        {
            Assert.AreEqual(103, ChineseRemainder.CRT(new nt[] {1,3,5}, new nt[] {3,5,7}));
        }
    }
}
