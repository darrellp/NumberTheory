using System.Collections.Generic;
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
	public class FactoringTest
	{
		[TestMethod]
		public void PollardRhoTest()
		{
			#if BIGINTEGER
			var n = Factoring.PollardRho(49);
			Assert.AreEqual(7,n);
			n = Factoring.PollardRho(20584996606709, 1000);
			Assert.IsTrue(n == 1316717 || n == 15633577);
			n = Factoring.PollardRho(30871180313527, 1000);
			Assert.IsTrue(n == 5555611 || n == 5556757);
			n = Factoring.PollardRho(111, 1000);
			Assert.IsTrue(n == 3 || n == 37);
			n = Factoring.PollardRhoSafe(113, 1000);
			Assert.IsTrue(n == 113);
			#else
			var n = Factoring.PollardRho(49);
			Assert.AreEqual(7,n);
			n = Factoring.PollardRho(10505681, 1000);
			Assert.IsTrue(n == 977 || n == 10753);
			#endif
		}

		bool FindFactor(nt prime, int exp, List<PrimeFactor> factorization)
		{
			return factorization.Any(primeFactor => primeFactor.Prime == prime && primeFactor.Exp == exp);
		}

		bool ValidateFactors(List<PrimeFactor> factorization, params nt[] factors )
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
			Assert.IsTrue(ValidateFactors(Factoring.Factor(381151), 563, 1, 677, 1));
			Assert.IsTrue(ValidateFactors(Factoring.Factor(10), 5, 1, 2, 1));
			Assert.IsTrue(ValidateFactors(Factoring.Factor(345119), 563,1,613,1));
#if BIGINTEGER
			Assert.IsTrue(ValidateFactors(Factoring.Factor(297049858030), 2357,2,5347,1,5,1,2,1));
#endif
		}
	}
}
