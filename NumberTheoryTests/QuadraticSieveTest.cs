using Microsoft.VisualStudio.TestTools.UnitTesting;
using NumberTheory;
using System;
using System.Linq;
using System.Numerics;
using static System.Math;

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
			var factor = QuadraticSieve.Factor(7907L * 7919L);
			Assert.IsTrue(factor == 7907L || factor == 7919L);
			var f1Big = BigInteger.Parse("12345678921");
			var f2Big = f1Big + 12;
			var factorBig = QuadraticSieve.Factor(f1Big * f2Big);			// Factoring 152415788168571871293
            var otherFactorBig = (f1Big * f2Big) / factorBig;
			Assert.IsTrue(factorBig * otherFactorBig == f1Big * f2Big);		// 17106995774827 * 8909559
        }
    }
}
