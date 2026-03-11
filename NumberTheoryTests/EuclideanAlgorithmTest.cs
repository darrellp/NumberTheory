using Microsoft.VisualStudio.TestTools.UnitTesting;
using NumberTheory;

namespace NumberTheoryTests
{
    [TestClass]
    public class EuclideanAlgorithmTest
    {
        [TestMethod]
        public void TestGCD()
        {
            Assert.AreEqual(1L, Euclidean.GCD(97L, 18L));
            Assert.AreEqual(6L, Euclidean.GCD(102L, 18L));
            Assert.AreEqual(6L, Euclidean.GCD(18L, 102L));
        }

        [TestMethod]
        public void TestEuclideanExt()
        {
            var ext = Euclidean.EuclideanExt(97L, 18L);
            Assert.AreEqual(1L, ext.GCD);
            Assert.AreEqual(-5L, ext.Coeff1);
            Assert.AreEqual(27L, ext.Coeff2);
            ext = Euclidean.EuclideanExt(541L, 7919L);
            Assert.AreEqual(1L, ext.GCD);
            Assert.AreEqual(-1010L, ext.Coeff1);
            Assert.AreEqual(69L, ext.Coeff2);
        }

        [TestMethod]
        public void TestDiophantineSolve()
        {
            var fn = Euclidean.DiophantineSolve(13L, 51L, 500L);

            for (var i = 1; i < 6; i++)
            {
                var sln = fn(i);
                Assert.AreEqual(500L, sln[0] * 13 + sln[1] * 51);
            }
        }

        [TestMethod]
        public void TestLinearCongruenceSolve()
        {
            var a = 123L;
            var b = 9123L;
            var mod = 321123L;
            var solns = Euclidean.LinearCongruenceSolve(a, b, mod);
            Assert.AreEqual(3, solns.Length);
            foreach (var isoln in solns)
            {
                Assert.AreEqual(b, a * isoln % mod);
            }
        }
    }
}
