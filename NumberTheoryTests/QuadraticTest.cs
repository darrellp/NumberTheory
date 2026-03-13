using Microsoft.VisualStudio.TestTools.UnitTesting;
using NumberTheory;
using static NumberTheory.Quadratic;

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
				Assert.AreEqual(results[i - 1], Jacobi(10L, (long)i));
			}
			Assert.AreEqual(1, Jacobi(-7L, 100000007L));
		}

		[TestMethod]
		public void TestSqrtMod()
		{
			bool fSuccess;

			TestSqrtModCase(1620L, 12377L);
			TestSqrtModCase(15347L, 17L);
			TestSqrtModCase(15347L, 29L);
			TestSqrtModCase(1619L, 12377L);
			TestSqrtModCase(1621L, 12377L);
			TestSqrtModCase(1622L, 12377L);
			TestSqrtModCase(1623L, 12377L);
			TestSqrtModCase(1624L, 12377L);
			TestSqrtModCase(1625L, 12377L);
			TestSqrtModCase(1626L, 12377L);
		}

		private void TestSqrtModCase(long a, long p) 
		{
			Assert.IsTrue(p.IsPrime());
			var i = SqrtMod(a, p, out var fSuccess);
			if (!fSuccess) return;
			Assert.AreEqual(a % p, (i * i) % p);
		}
	}
}
