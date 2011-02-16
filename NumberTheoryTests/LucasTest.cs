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
        	var p = 1;
        	var q = 2;
        	var ui = (nt)0;
        	var uip1 = (nt)1;
        	var vi = (nt)2;
        	var vip1 = (nt)p;
			nt bigmod = (nt)10000000;
			for (var i = 0; i < 10; i++)
			{
				Assert.AreEqual(ui.Normalize(bigmod), Lucas.LucasU(p, q, bigmod, i));
				Assert.AreEqual(vi.Normalize(bigmod), Lucas.LucasV(p, q, bigmod, i));
				var uip1New = p*uip1 - q*ui;
				var vip1New = p*vip1 - q*vi;
				ui = uip1;
				vi = vip1;
				uip1 = uip1New;
				vip1 = vip1New;
			}
        }

        [TestMethod]
        public void LucasPsuedoprimeTest()
        {
			Assert.IsTrue(Lucas.LucasPsuedoprimeTest(1,2,100000007));
			
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
