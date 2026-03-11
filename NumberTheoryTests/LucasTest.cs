using Microsoft.VisualStudio.TestTools.UnitTesting;
using NumberTheory;

namespace NumberTheoryTests
{
    [TestClass]
    public class LucasTest
    {
        [TestMethod]
        public void LucasSequenceTest()
        {
            Assert.AreEqual(6616217487L, Lucas.LucasU(3, -1, 6616217488L, 20L));
            Assert.AreEqual(23855111399L, Lucas.LucasV(3, -1, 23855111400L, 20L));
            var p = 1;
            var q = 2;
            var ui = 0L;
            var uip1 = 1L;
            var vi = 2L;
            var vip1 = (long)p;
            long bigmod = 10000000L;
            for (var i = 0; i < 10; i++)
            {
                Assert.AreEqual(ui.Normalize(bigmod), Lucas.LucasU(p, q, bigmod, (long)i));
                Assert.AreEqual(vi.Normalize(bigmod), Lucas.LucasV(p, q, bigmod, (long)i));
                var uip1New = p * uip1 - q * ui;
                var vip1New = p * vip1 - q * vi;
                ui = uip1;
                vi = vip1;
                uip1 = uip1New;
                vip1 = vip1New;
            }
        }

        [TestMethod]
        public void LucasPsuedoprimeTest()
        {
            Assert.IsTrue(Lucas.LucasPsuedoprimeTest(100000007L));

            Assert.IsFalse(Lucas.LucasPsuedoprimeTest(4181L));
            Assert.IsFalse(Lucas.LucasPsuedoprimeTest(6721L));
            Assert.IsFalse(Lucas.LucasPsuedoprimeTest(323L));
            Assert.IsFalse(Lucas.LucasPsuedoprimeTest(1891L));
            Assert.IsTrue(Lucas.LucasPsuedoprimeTest(521L));
            Assert.IsTrue(Lucas.LucasPsuedoprimeTest(523L));
            Assert.IsTrue(Lucas.LucasPsuedoprimeTest(541L));
            Assert.IsFalse(Lucas.LucasPsuedoprimeTest(20L));
            Assert.IsFalse(Lucas.LucasPsuedoprimeTest(525L));
            Assert.IsFalse(Lucas.LucasPsuedoprimeTest(527L));
        }
    }
}
