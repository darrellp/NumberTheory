using Microsoft.VisualStudio.TestTools.UnitTesting;
using NumberTheory;

namespace NumberTheoryTests
{
	[TestClass]
	public class QuadraticTests
	{
		[TestMethod]
		public void TestJacobi()
		{
			var results = new[] { 1, 0, 1, 0, 0, 0, -1, 0, 1, 0, -1, 0, 1, 0, 0, 0, -1, 0, -1, 0 };
			for (var i = 1; i <= 20; i++)
			{
				Assert.AreEqual(results[i - 1], Quadratic.Jacobi(10L, (long)i));
			}
			Assert.AreEqual(1, Quadratic.Jacobi(-7L, 100000007L));
		}

		[TestMethod]
		public void TestSqrtMod()
		{
			bool fSuccess;

			var i = Quadratic.SqrtMod(1619L, 12377L, out fSuccess);
			Assert.IsTrue(fSuccess);
			Assert.AreEqual(1619L, (i * i) % 12377);
		}
	}
}
