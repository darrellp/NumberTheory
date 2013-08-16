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
    public class ContinuedFractionTests
    {
	    [TestMethod]
	    public void TestContinuedFraction()
	    {
		    var cfr = new ContinuedFraction(0, 1);
		    Assert.AreEqual(1, cfr.Vals.Count);
		    Assert.AreEqual(0, cfr.Vals[0]);

		    cfr = new ContinuedFraction(33, 14);
		    Assert.AreEqual(4, cfr.Vals.Count);
		    var retval = new nt[] {2, 2, 1, 4};
		    Assert.IsTrue(cfr.Vals.Zip(retval, (a, b) => a == b).All(f => f));

			cfr = new ContinuedFraction(retval);
			Assert.AreEqual(cfr.Val, new Rational(33, 14));
	    }

	    [TestMethod]
	    public void TestConvergents()
	    {
		    var cfr = new ContinuedFraction(0, 1);
		    var cnv = cfr.Convergents();
			Assert.AreEqual(1, cnv.Count);
			Assert.AreEqual(0, cnv[0].Num);

		    cfr = new ContinuedFraction(612, 233);
			cnv = cfr.Convergents();
			Assert.AreEqual(7, cfr.Vals.Count);
		    var retval = new Rational[]
		    {
			    new Rational(2,1),
				new Rational(3,1), 
				new Rational(5,2), 
				new Rational(8,3), 
				new Rational(21,8), 
				new Rational(197,75), 
				new Rational(612,233)
		    };
		    Assert.IsTrue(cnv.Zip(retval, (a, b) => a == b).All(f=>f));
	    }
    }
}
