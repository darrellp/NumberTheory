using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#if BIGINTEGER
using NumberTheoryBig;
#else
using NumberTheoryLong;
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
		    var mtx = new BigInteger[]
		    {
			    new BigInteger(18),
				new BigInteger(5), 
				new BigInteger(65), 
				new BigInteger(18)
		    };

		    var res = PowerMod.MatrixPower(5, mtx);
		    Assert.AreEqual(30349818, res[0]);
			Assert.AreEqual(8417525, res[1]);
			Assert.AreEqual(109427825, res[2]);
			Assert.AreEqual(30349818, res[3]);
		}
    }
}
