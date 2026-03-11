using Microsoft.VisualStudio.TestTools.UnitTesting;
using NumberTheory;

namespace NumberTheoryTests
{
	[TestClass]
	public class QuadraticSieveTest
	{
		[TestMethod]
		public void TestFactor()
		{
			Assert.AreEqual(2L, QuadraticSieve.Factor(10L));
			Assert.AreEqual(23L, QuadraticSieve.Factor(23L));
			Assert.AreEqual(3L, QuadraticSieve.Factor(243L));
		}
	}
}
