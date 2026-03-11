using Microsoft.VisualStudio.TestTools.UnitTesting;
using NumberTheory;

namespace NumberTheoryTests
{
    [TestClass]
    public class PowerModTests
    {
        [TestMethod]
        public void PowerModTest()
        {
            Assert.AreEqual(3115L, PowerMod.Power(2357L, 2357L, 3599L));
            Assert.AreEqual(5L, PowerMod.Power(7L, -1L, 17L));
        }

        [TestMethod]
        public void MatrixPowerTest()
        {
            var mtx = new long[] { 18, 5, 65, 18 };
            var res = PowerMod.MatrixPower(5L, mtx);
            Assert.AreEqual(30349818L, res[0]);
            Assert.AreEqual(8417525L, res[1]);
            Assert.AreEqual(109427825L, res[2]);
            Assert.AreEqual(30349818L, res[3]);
        }
    }
}
