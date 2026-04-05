using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NumberTheory;
using static NumberTheory.Factoring;

namespace NumberTheoryTests
{
	[TestClass]
	public class FactoringTest
	{
		[TestMethod]
		public void PollardRhoTest()
		{
			var n = PollardRho(49L);
			Assert.AreEqual(7L, n);
			n = PollardRho(10505681L, 1000);
			Assert.IsTrue(n == 977 || n == 10753);
			n = PollardRho(7907L * 7919L);
			Assert.IsTrue(n == 7907 || n == 7919);
		}

		bool FindFactor(long prime, int exp, List<PrimeFactor<long>> factorization)
		{
			return factorization.Any(primeFactor => primeFactor.Prime == prime && primeFactor.Exp == exp);
		}

		bool ValidateFactors(List<PrimeFactor<long>> factorization, params long[] factors)
		{
			if (factorization.Count != factors.Length / 2)
			{
				return false;
			}
			for (int i = 0; i < factors.Length / 2; i++)
			{
				if (!FindFactor(factors[2 * i], (int)factors[2 * i + 1], factorization))
				{
					return false;
				}
			}
			return true;
		}

		[TestMethod]
		public void DoFactoringTest()
		{
			Assert.IsTrue(ValidateFactors(Factor(381151L), 563, 1, 677, 1));
			Assert.IsTrue(ValidateFactors(Factor(10L), 5, 1, 2, 1));
			Assert.IsTrue(ValidateFactors(Factor(345119L), 563, 1, 613, 1));
			long p1 = 11593;
			long p2 = 12907;
			Assert.IsTrue(ValidateFactors(Factor(p1*p1*p2, 0, 100000), p1, 2, p2, 1));
		}
	}
}
