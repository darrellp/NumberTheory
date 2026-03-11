using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NumberTheory;

namespace NumberTheoryTests
{
    [TestClass]
    public class RationalTests
    {
        [TestMethod]
        public void TestEquality()
        {
            Assert.IsTrue((Rational<long>)5L == 5L);
            Assert.IsTrue(new Rational<long>(10, 20) == new Rational<long>(2, 4));
        }

        [TestMethod]
        public void TestOperators()
        {
            var a = new Rational<long>(3, 4);
            var b = new Rational<long>(2, 3);

            Assert.AreEqual(a + b, new Rational<long>(17, 12));
            Assert.AreEqual(a * b, new Rational<long>(1, 2));
            Assert.AreEqual(a - b, new Rational<long>(1, 12));
            Assert.AreEqual(a / b, new Rational<long>(9, 8));
        }
    }
}
