using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NumberTheory;
using static NumberTheory.Primes;

namespace NumberTheoryTests
{
	[TestClass]
	public class PrimesTest
	{
		[TestMethod]
		public void StrongPsuedoPrimeTestTest()
		{
			Assert.IsTrue(StrongPsuedoPrimeTest(2047L, 2L));
			Assert.IsFalse(StrongPsuedoPrimeTest(2049L, 2L));
			Assert.IsFalse(StrongPsuedoPrimeTest(2051L, 2L));
			Assert.IsTrue(StrongPsuedoPrimeTest(2053L, 2L));
		}

		[TestMethod]
		public void CompositeByDivisionTest()
		{
			// This value is actually composite but has prime factors above where we're testing.
			Assert.IsFalse(1022117L.CompositeByDivision());
			// The rest of these should have prime factors within our range
			Assert.IsTrue(304680L.CompositeByDivision());
			Assert.IsTrue(304681L.CompositeByDivision());
			Assert.IsTrue(304682L.CompositeByDivision());

			Assert.IsFalse(191L.CompositeByDivision());
			Assert.IsTrue(192L.CompositeByDivision());
			Assert.IsFalse(193L.CompositeByDivision());
		}

		private bool[] Seive(int n)
		{
			var ret = new bool[n + 1];
			var sqrt = (int)Math.Sqrt(n) + 1;
			ret[1] = true;

			for (var i = 2; i <= sqrt; i++)
			{
				var cmp = 2 * i;
				while (cmp < n)
				{
					ret[cmp] = true;
					cmp += i;
				}
			}
			return ret;
		}

		[TestMethod]
		public void IsPrimeTest()
		{
			var sv = Seive(250000);
			for (int i = 2; i < 250000; i++)
			{
				Assert.IsTrue(((long)i).IsPrime() != sv[i]);
			}
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
					Where(n => ((long)(n + 100000000)).IsPrime()).ToArray();
			Assert.IsTrue(poffsComputed.Length == poffs.Length && !poffsComputed.Except(poffs).Any());

			Assert.IsTrue(3L.IsPrime());
			Assert.IsTrue(2L.IsPrime());
			Assert.IsTrue(17L.IsPrime());
		}
	}
}
