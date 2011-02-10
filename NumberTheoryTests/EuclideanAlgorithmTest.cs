using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using NumberTheory;
using nt = NumberTheory.Euclidean;

namespace NumberTheoryTests
{
    [TestClass]
    public class EuclideanAlgorithmTest
    {
        [TestMethod]
        public void TestGCD()
        {
            Assert.AreEqual(1, nt.GCD(97, 18));
            Assert.AreEqual(6, nt.GCD(102, 18));
            Assert.AreEqual(6, nt.GCD(18, 102));
        }

        [TestMethod]
        public void TestEuclideanExt()
        {
            EuclideanExt ext = nt.EuclideanExt(97, 18);
            Assert.AreEqual(1,ext.GCD);
            Assert.AreEqual(-5, ext.Coeff1);
            Assert.AreEqual(27, ext.Coeff2);
            ext = nt.EuclideanExt(541,7919);
            Assert.AreEqual(1, ext.GCD);
            Assert.AreEqual(-1010, ext.Coeff1);
            Assert.AreEqual(69, ext.Coeff2);
        }
    }
}
