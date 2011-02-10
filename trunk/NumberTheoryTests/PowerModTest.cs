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
            Assert.AreEqual(3115, PowerMod.Power(2357, 2357, 3599));
            // TODO: This doesn't work for BigInteger
            //Assert.AreEqual(5,PowerMod.Power(7,-1,17));
        }
    }
}
