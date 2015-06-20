using System;
using System.Linq;
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
	public class QuadraticSieveTest
	{
		[TestMethod]
		public void TestFactor()
		{
			Assert.AreEqual(2, QuadraticSieve.Factor(10));
			Assert.AreEqual(23, QuadraticSieve.Factor(23));
			Assert.AreEqual(3, QuadraticSieve.Factor(243));
		}
	}
}
