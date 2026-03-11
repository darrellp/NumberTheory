using Microsoft.VisualStudio.TestTools.UnitTesting;
using NumberTheory;
using static NumberTheory.Lucas;

namespace NumberTheoryTests
{
    [TestClass]
    public class LucasTest
    {
        [TestMethod]
        public void LucasSequenceTest()
        {
            Assert.AreEqual(6616217487L, LucasU(3, -1, 6616217488L, 20L));
            Assert.AreEqual(23855111399L, LucasV(3, -1, 23855111400L, 20L));
            var p = 1;
            var q = 2;
            var ui = 0L;
            var uip1 = 1L;
            var vi = 2L;
            var vip1 = (long)p;
            long bigmod = 10000000L;
            for (var i = 0; i < 10; i++)
            {
                Assert.AreEqual(ui.Normalize(bigmod), LucasU(p, q, bigmod, (long)i));
                Assert.AreEqual(vi.Normalize(bigmod), LucasV(p, q, bigmod, (long)i));
                var uip1New = p * uip1 - q * ui;
                var vip1New = p * vip1 - q * vi;
                ui = uip1;
                vi = vip1;
                uip1 = uip1New;
                vip1 = vip1New;
            }
        }

        [TestMethod]
        public void TestLucasPsuedoprime()
        {
            Assert.IsTrue(LucasPsuedoprimeTest(100000007L));

            Assert.IsFalse(LucasPsuedoprimeTest(4181L));
            Assert.IsFalse(LucasPsuedoprimeTest(6721L));
            Assert.IsFalse(LucasPsuedoprimeTest(323L));
            Assert.IsFalse(LucasPsuedoprimeTest(1891L));
            Assert.IsTrue(LucasPsuedoprimeTest(521L));
            Assert.IsTrue(LucasPsuedoprimeTest(523L));
            Assert.IsTrue(LucasPsuedoprimeTest(541L));
            Assert.IsFalse(LucasPsuedoprimeTest(20L));
            Assert.IsFalse(LucasPsuedoprimeTest(525L));
            Assert.IsFalse(LucasPsuedoprimeTest(527L));
        }
    }
}
