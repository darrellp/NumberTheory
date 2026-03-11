using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NumberTheory;

namespace NumberTheoryTests
{
	[TestClass]
	public class FactoringTest
	{
		[TestMethod]
		public void PollardRhoTest()
		{
			var n = Factoring.PollardRho(49L);
			Assert.AreEqual(7L, n);
			n = Factoring.PollardRho(10505681L, 1000);
			Assert.IsTrue(n == 977 || n == 10753);
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
			Assert.IsTrue(ValidateFactors(Factoring.Factor(381151L), 563, 1, 677, 1));
			Assert.IsTrue(ValidateFactors(Factoring.Factor(10L), 5, 1, 2, 1));
			Assert.IsTrue(ValidateFactors(Factoring.Factor(345119L), 563, 1, 613, 1));
		}
	}
}
