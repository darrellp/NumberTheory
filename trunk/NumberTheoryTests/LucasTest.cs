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
    public class LucasTest
    {
        [TestMethod]
        public void LucasSequenceTest()
        {
            Assert.AreEqual(6616217487, Lucas.LucasU(3, -1, 6616217488, 20));
            Assert.AreEqual(23855111399, Lucas.LucasV(3, -1, 23855111400, 20));
        }

        [TestMethod]
        public void LucasPsuedoprimeTest()
        {
            Assert.IsTrue(Lucas.LucasPsuedoprimeTest(1, -1, 4181));
            Assert.IsTrue(Lucas.LucasPsuedoprimeTest(1, -1, 6721));
            Assert.IsTrue(Lucas.LucasPsuedoprimeTest(1, -1, 323));
            Assert.IsTrue(Lucas.LucasPsuedoprimeTest(1, -1, 1891));
            Assert.IsTrue(Lucas.LucasPsuedoprimeTest(1, -1, 521));
            Assert.IsTrue(Lucas.LucasPsuedoprimeTest(1, -1, 523));
            Assert.IsTrue(Lucas.LucasPsuedoprimeTest(1, -1, 541));
            Assert.IsFalse(Lucas.LucasPsuedoprimeTest(1, -1, 520));
            Assert.IsFalse(Lucas.LucasPsuedoprimeTest(1, -1, 525));
            Assert.IsFalse(Lucas.LucasPsuedoprimeTest(1, -1, 527));

			Assert.IsTrue(Lucas.LucasQuadraticTest(1, -1, 4181));
			Assert.IsTrue(Lucas.LucasQuadraticTest(1, -1, 6721));
			// These two psuedoprimes are caught by the quadratic test but
			// missed by the test above.
			Assert.IsFalse(Lucas.LucasQuadraticTest(1, -1, 323));
			Assert.IsFalse(Lucas.LucasQuadraticTest(1, -1, 1891));

			Assert.IsTrue(Lucas.LucasQuadraticTest(1, -1, 521));
			Assert.IsTrue(Lucas.LucasQuadraticTest(1, -1, 523));
			Assert.IsTrue(Lucas.LucasQuadraticTest(1, -1, 541));
			Assert.IsFalse(Lucas.LucasQuadraticTest(1, -1, 520));
			Assert.IsFalse(Lucas.LucasQuadraticTest(1, -1, 525));
			Assert.IsFalse(Lucas.LucasQuadraticTest(1, -1, 527));
		}
    }
}
