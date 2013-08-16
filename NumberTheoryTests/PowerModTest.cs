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
    public class PowerModTests
    {
        [TestMethod]
        public void PowerModTest()
        {
            Assert.AreEqual(3115, PowerMod.Power(2357, 2357, 3599));
            Assert.AreEqual(5,PowerMod.Power(7,-1,17));
        }

	    [TestMethod]
	    public void MatrixPowerTest()
	    {
		    var mtx = new nt[] {18, 5, 65, 18};
		    var res = PowerMod.MatrixPower(5, mtx);
		    Assert.AreEqual(30349818, res[0]);
			Assert.AreEqual(8417525, res[1]);
			Assert.AreEqual(109427825, res[2]);
			Assert.AreEqual(30349818, res[3]);
		}
    }
}
