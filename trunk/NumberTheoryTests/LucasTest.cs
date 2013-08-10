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
			Assert.IsTrue(Lucas.LucasPsuedoprimeTest(100000007));
			
			Assert.IsFalse(Lucas.LucasPsuedoprimeTest(4181));
            Assert.IsFalse(Lucas.LucasPsuedoprimeTest(6721));
			Assert.IsFalse(Lucas.LucasPsuedoprimeTest(323));
			Assert.IsFalse(Lucas.LucasPsuedoprimeTest(1891));
            Assert.IsTrue(Lucas.LucasPsuedoprimeTest(521));
            Assert.IsTrue(Lucas.LucasPsuedoprimeTest(523));
            Assert.IsTrue(Lucas.LucasPsuedoprimeTest(541));
            Assert.IsFalse(Lucas.LucasPsuedoprimeTest(20));
            Assert.IsFalse(Lucas.LucasPsuedoprimeTest(525));
            Assert.IsFalse(Lucas.LucasPsuedoprimeTest(527));
		}
    }
}
