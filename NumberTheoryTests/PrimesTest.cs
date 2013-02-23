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
	public class PrimesTest
	{
		[TestMethod]
		public void StrongPsuedoPrimeTest()
		{
			Assert.IsTrue(Primes.StrongPsuedoPrimeTest(2047, 2));
			Assert.IsFalse(Primes.StrongPsuedoPrimeTest(2049, 2));
			Assert.IsFalse(Primes.StrongPsuedoPrimeTest(2051, 2));
			Assert.IsTrue(Primes.StrongPsuedoPrimeTest(2053, 2));
#if BIGINTEGER
			for (var i = 2; i < 13; i++)
			{
				if (i == 11)
				{
					Assert.IsFalse(Primes.StrongPsuedoPrimeTest(3215031751, i));
				}
				else
				{
					Assert.IsTrue(Primes.StrongPsuedoPrimeTest(3215031751, i));
				}
			}
#endif
		}

		[TestMethod]
		public void CompositeByDivisionTest()
		{
			// This value is actually composite but has prime factors above where we're testing.
			Assert.IsFalse(((nt)304679).CompositeByDivision());
			// The rest of these should have prime factors within our range
			Assert.IsTrue(((nt)304680).CompositeByDivision());
			Assert.IsTrue(((nt)304681).CompositeByDivision());
			Assert.IsTrue(((nt)304682).CompositeByDivision());
			
			Assert.IsFalse(((nt)191).CompositeByDivision());
			Assert.IsTrue(((nt)192).CompositeByDivision());
			Assert.IsFalse(((nt)193).CompositeByDivision());
		}

		[TestMethod]
		public void IsPrimeTest()
		{
			var poffs = new int[]
				{
					7, 37, 39, 49, 73, 81, 123, 127, 193, 213, 217, 223, 231, 237, 259,
					267, 279, 357, 379, 393, 399, 421, 429, 463, 469, 471, 493, 541, 543,
					561, 567, 577, 609, 627, 643, 651, 661, 669, 673, 687, 717, 721, 793,
					799, 801, 837, 841, 853, 891, 921, 937, 939, 963, 969
				};
			// Identify the first 1000 primes past 100,000,000
			var poffsComputed = 
				Enumerable.
					Range(0, 1000).
					Where(n => ((nt) (n + 100000000)).IsPrime()).ToArray();
			Assert.IsTrue(poffsComputed.Length == poffs.Length && !poffsComputed.Except(poffs).Any());

			Assert.IsTrue(((nt)3).IsPrime());
			Assert.IsTrue(((nt)2).IsPrime());
			Assert.IsTrue(((nt)17).IsPrime());
		}
	}
}
