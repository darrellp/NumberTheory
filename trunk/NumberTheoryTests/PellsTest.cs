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
    public class PellsTests
    {
	    [TestMethod]
	    public void TestPells()
	    {
		    nt x, y;
		    Pells.SolvePells(13, 1, out x, out y);
	    }
    }
}
