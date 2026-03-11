using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NumberTheory;

namespace NumberTheoryTests
{
    [TestClass]
    public class ContinuedFractionTests
    {
        [TestMethod]
        public void TestContinuedFraction()
        {
            var cfr = new ContinuedFraction<long>(0, 1);
            Assert.AreEqual(1, cfr.Vals.Count);
            Assert.AreEqual(0L, cfr.Vals[0]);

            cfr = new ContinuedFraction<long>(33, 14);
            Assert.AreEqual(4, cfr.Vals.Count);
            var retval = new long[] { 2, 2, 1, 4 };
            Assert.IsTrue(cfr.Vals.Zip(retval, (a, b) => a == b).All(f => f));

            cfr = new ContinuedFraction<long>(retval);
            Assert.AreEqual(cfr.Val, new Rational<long>(33, 14));
        }

        [TestMethod]
        public void TestConvergents()
        {
            var cfr = new ContinuedFraction<long>(0, 1);
            var cnv = cfr.Convergents();
            Assert.AreEqual(1, cnv.Count);
            Assert.AreEqual(0L, cnv[0].Num);

            cfr = new ContinuedFraction<long>(612, 233);
            cnv = cfr.Convergents();
            Assert.AreEqual(7, cfr.Vals.Count);
            var retval = new Rational<long>[]
            {
                new Rational<long>(2, 1),
                new Rational<long>(3, 1),
                new Rational<long>(5, 2),
                new Rational<long>(8, 3),
                new Rational<long>(21, 8),
                new Rational<long>(197, 75),
                new Rational<long>(612, 233)
            };
            Assert.IsTrue(cnv.Zip(retval, (a, b) => a == b).All(f => f));
        }
    }
}
