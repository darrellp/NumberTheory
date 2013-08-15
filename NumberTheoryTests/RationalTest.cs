using System.Linq;
using System.Text.RegularExpressions;
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
    public class RationalTests
    {
	    [TestMethod]
	    public void TestEquality()
	    {
			Assert.IsTrue((Rational)5 == 5);
			Assert.IsTrue(new Rational(10, 20) == new Rational(2, 4));
	    }

	    [TestMethod]
	    public void TestOperators()
	    {
		    var a = new Rational(3, 4);
			var b = new Rational(2, 3);

			Assert.AreEqual(a + b, new Rational(17, 12));
			Assert.AreEqual(a*b, new Rational(1,2));
			Assert.AreEqual(a - b, new Rational(1, 12));
			Assert.AreEqual(a/b, new Rational(9, 8));
	    }
    }
}
