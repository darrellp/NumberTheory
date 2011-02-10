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
        }
    }
}
