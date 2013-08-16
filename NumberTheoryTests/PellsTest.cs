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
    public class PellsTests
    {
	    [TestMethod]
	    public void TestPells()
	    {
		    nt x, y;
		    Pells.SolvePells(19, 1, 1, out x, out y);
			Assert.AreEqual(170, x);
			Assert.AreEqual(39, y);
			Pells.SolvePells(19, 1, 2, out x, out y);
		    Assert.AreEqual(57799, x);
			Assert.AreEqual(13260, y);
			Assert.IsFalse(Pells.SolvePells(19, -1, 1, out x, out y));
			Assert.IsFalse(Pells.SolvePells(19, 7, 1, out x, out y));
			Pells.SolvePells(13, -1, 1, out x, out y);
			Assert.AreEqual(18, x);
			Assert.AreEqual(5, y);
			Pells.SolvePells(13, -1, 2, out x, out y);
			Assert.AreEqual(23382, x);
			Assert.AreEqual(6485, y);
			Pells.SolvePells(13, 1, 1, out x, out y);
			Assert.AreEqual(649, x);
			Assert.AreEqual(180, y);
			Pells.SolvePells(13, 1, 2, out x, out y);
			Assert.AreEqual(842401, x);
			Assert.AreEqual(233640, y);
		}
    }
}
