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
	public class QuadraticTests
	{
		[TestMethod]
		public void TestJacobi()
		{
			var results = new[] {1, 0, 1, 0, 0, 0, -1, 0, 1, 0, -1, 0, 1, 0, 0, 0, -1, 0, -1, 0};
			for (var i = 1; i <= 20; i++)
			{
				Assert.AreEqual(results[i - 1], Quadratic.Jacobi(10, i));
			}
			Assert.AreEqual(1, Quadratic.Jacobi(-7, 100000007));
		}

		[TestMethod]
		public void TestSqrtMod()
		{
			bool fSuccess;

			var i = Quadratic.SqrtMod(1619, 12377, out fSuccess);
			Assert.IsTrue(fSuccess);
			Assert.AreEqual(1619, (i*i) % 12377);
		}
	}
}
