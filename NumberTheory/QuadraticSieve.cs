using System;
using System.Collections.Generic;
using System.Linq;
#if BIGINTEGER
using nt = System.Numerics.BigInteger;
#elif LONG
using nt = System.Int64;
#endif


// ReSharper disable CheckNamespace
#if BIGINTEGER
namespace NumberTheoryBig
#elif LONG
namespace NumberTheoryLong
#endif
// ReSharper restore CheckNamespace
{
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Class to hold the static routine implementing the quadratic sieve. </summary>
	///
	/// <remarks>	Darrellp, 2/16/2011. </remarks>
	////////////////////////////////////////////////////////////////////////////////////////////////////

	public static class QuadraticSieve
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Factors n using a quadratic sieve</summary>
		///
		/// <remarks>	Darrellp, 2/16/2011. </remarks>
		///
		/// <param name="n">	The number to be factored. </param>
		///
		/// <returns>	A factor of the number if one exists. </returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		public static nt Factor(nt n)
		{
			if ((n & 1) == 0)
			{
				return 2;
			}

			//// Verify that n is not a power
			//var fctPower = CheckPower(n);

			//// If it is
			//if (fctPower > 0)
			//{
			//	// Return it's root
			//	return fctPower;
			//}
			//return 0;

			// Determine B for B-smooth number base
			var b = DetermineB(n);

			// Find odd primes <= B which are quadratic residues
			var primeList = GetPrimeList(n, b);

			// for each prime found, find the square root of n mod that prime
			bool fSuccess;
			var sqrtList = primeList.Select(p => ((nt) p).SqrtMod(n, out fSuccess)).ToList();

			// Compute our Candidate list
			var candidates = SieveCandidates(primeList, n);

			// Find a set of rows that add to 0 mod 2
			// TODO: Make this work

			// Determine y from the solution found above
			nt y = 0;

			// Determine x as the product of values represented by the rows in the solution
			nt x = 0;

			// Return GCD(x - y, n) and hope for the best
			return n.GCD(x - y);
		}

		//private static long CheckPower(nt n)
		//{
		//	return Enumerable.Range(3, n.BitCount()).Select(k => CheckSinglePower(n, k)).FirstOrDefault(k => k >= 0);
		//}

		//private static nt CheckSinglePower(nt n, nt k)
		//{
		//	if (n < 4)
		//	{
		//		return -1;
		//	}
		//	var xCur = n;
		//	nt xPrev = xCur / 2;

		//	while (xPrev != xCur && xPrev != xCur + 1)
		//	{
		//		var t = xCur;
		//		var pow = PowerMod.Power(xPrev, k - 1);
		//		xCur = (n + (k - 1) * xPrev * pow) / (k * pow);
		//		xPrev = t;
		//	}
		//	return xPrev == xCur && PowerMod.Power(xCur, k) == n ? xCur : -1;
		//}

		private static IEnumerable<int[]> SieveCandidates(int[] primeList, nt n)
		{
			// Set sqrtN to the integer square root of n
			var sqrtN = n.IntegerSqrt() + 1;

			// Sieve our values out of primeList
			return Enumerable.
				// Get an "infinite" range of values
				Range(0, int.MaxValue).
				// Offset them by the square root of n
				Select(indx => sqrtN + indx).
				// Factor them over our factor base
				Select(cand => CheckBSmooth(cand * cand - n, primeList)).
				// Toss the ones that aren't  B-smooth
				Where(lst => lst != null).
				// Only keep K of the rest
				Take(primeList.Length);
		}

		private static int CountFactors(ref nt cand, int p)
		{
			var nCount = 0;
			while (cand % p == 0)
			{
				cand /= p;
				nCount++;
			}
			return nCount;
		}

		private static int[] CheckBSmooth(nt cand, int[] primeList)
		{
			//!+ TODO: Optimize this!
			// Turn each prime in our factor base into the exponent for that prime in the candidate
			var expList = primeList.
				Select(fct => CountFactors(ref cand, fct)).
				ToArray();

			// If we whittled the candidate down to 1, then it's completely factored
			return cand == 1 ? expList : null;
		}

		private static int[] GetPrimeList(nt n, int b)
		{
			// Set sp for convenience
			var sp = Primes.SmallPrimes;

			// Have we got enough primes in our small primes array?
			if (b < sp[sp.Length - 1])
			{
				// Filter out the non-residues and return the rest of the primes smaller than B
				return sp.
					Take(Array.BinarySearch(sp, b)).
					Where(cand => Quadratic.Jacobi(n, cand) == 1).
					Select(bi => (int) bi).
					ToArray();
			}
			//!+TODO: Implement case where we need more primes!!!
			throw new NotImplementedException();
		}

		static private readonly double BExp = Math.Sqrt(2) / 4;

		private static int DetermineB(nt n)
		{
#if BIGINTEGER
			return (int) Math.Pow(Math.Exp(Math.Sqrt(nt.Log(n)*Math.Log(nt.Log(n)))), BExp);
#else
			return (int) Math.Pow(Math.Exp(Math.Sqrt(Math.Log(n) * Math.Log(Math.Log(n)))), BExp);
#endif
		}
	}
}
