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
	public class FactoringTest
	{
		[TestMethod]
		public void PollardRhoTest()
		{
			#if BIGINTEGER
			var n = Factoring.PollardRho(20584996606709, 1000);
			Assert.IsTrue(n == 1316717 || n == 15633577);
			n = Factoring.PollardRho(30871180313527, 1000);
			Assert.IsTrue(n == 5555611 || n == 5556757);
			n = Factoring.PollardRho(111, 1000);
			Assert.IsTrue(n == 3 || n == 37);
			#else
			var n = Factoring.PollardRho(10505681, 1000);
			Assert.IsTrue(n == 977 || n == 10753);
			#endif
		}
	}
}
