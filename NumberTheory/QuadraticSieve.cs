using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using static System.Math;

namespace NumberTheory;

/// <summary>	Class to hold the static routine implementing the quadratic sieve. </summary>
public static class QuadraticSieve
{
	/// <summary>	Factors n using a quadratic sieve</summary>
	/// <param name="n">	The number to be factored. </param>
	/// <returns>	A factor of the number if one exists. </returns>
	public static T Factor<T>(T n) where T : IBinaryInteger<T>
	{
		var two = T.One + T.One;

		if ((n & T.One) == T.Zero)
		{
			return two;
		}

		if (n.IsPrime())
		{
			return n;
		}

		// Verify that n is not a power
		var fctPower = CheckPower(n);

		// If it is
		if (fctPower > T.Zero)
		{
			// Return it's root
			return fctPower;
		}

		// Determine B for B-smooth number base
		var b = DetermineB(n);

		// Find odd primes <= B which are quadratic residues
		var primeList = GetPrimeList(n, b);

        // for each prime found, find the square root of n mod that prime
        var sqrtList = primeList.Select(p => Quadratic.SqrtMod(n, T.CreateChecked(p), out _)).ToList();

        // Compute our Candidate list
        var candidates = SieveCandidates(primeList, n);

		// Find a set of rows that add to 0 mod 2
		// TODO: Make this work

		// Determine y from the solution found above
		T y = T.Zero;

		// Determine x as the product of values represented by the rows in the solution
		T x = T.Zero;

		// Return GCD(x - y, n) and hope for the best
		return n.GCD(x - y);
	}

	private static T CheckPower<T>(T n) where T : IBinaryInteger<T>
	{
		return Enumerable.Range(3, n.BitCount()).Select(k => CheckSinglePower(n, k)).FirstOrDefault(k => k >= T.Zero);
	}

	private static T CheckSinglePower<T>(T n, int k) where T : IBinaryInteger<T>
	{
		var four = T.CreateChecked(4);
		if (n < four)
		{
			return -T.One;
		}
		var kT = T.CreateChecked(k);
		var kRoot = Utilities.IntegerRoot(n, kT);
		return PowerMod.Power(kRoot, kT) == n ? kRoot : -T.One;
	}

	private static IEnumerable<int[]> SieveCandidates<T>(int[] primeList, T n) where T : IBinaryInteger<T>
	{
		// Set sqrtN to the integer square root of n
		var sqrtN = n.IntegerSqrt() + T.One;

		// Sieve our values out of primeList
		return Enumerable.
			// Get an "infinite" range of values
			Range(0, int.MaxValue).
			// Offset them by the square root of n
			Select(indx => sqrtN + T.CreateChecked(indx)).
			// Factor them over our factor base
			Select(cand => CheckBSmooth(cand * cand - n, primeList)).
			// Toss the ones that aren't B-smooth
			Where(lst => lst != null).
			// Only keep K of the rest
			Take(primeList.Length);
	}

	private static int CountFactors<T>(ref T cand, int p) where T : IBinaryInteger<T>
	{
		var pT = T.CreateChecked(p);
		var nCount = 0;
		while (cand % pT == T.Zero)
		{
			cand /= pT;
			nCount++;
		}
		return nCount;
	}

	private static int[] CheckBSmooth<T>(T cand, int[] primeList) where T : IBinaryInteger<T>
	{
		// Turn each prime in our factor base into the exponent for that prime in the candidate
		var expList = primeList.
			Select(fct => CountFactors(ref cand, fct)).
			ToArray();

		// If we whittled the candidate down to 1, then it's completely factored
		return cand == T.One ? expList : null;
	}

	private static int[] GetPrimeList<T>(T n, int b) where T : IBinaryInteger<T>
	{
		// Set sp for convenience
		var sp = Primes.SmallPrimes;

		// Have we got enough primes in our small primes array?
		if (b < sp[sp.Length - 1])
		{
			var bIndex = Array.BinarySearch(sp, (long)b);
			if (bIndex < 0)
			{
				// Binary search returns the next LARGEST value
				// when we want the next smallest which introduces an offset of 1.
				bIndex = ~bIndex - 1;
			}

			// Filter out the non-residues and return the rest of the primes smaller than B
			return sp.
				Take(bIndex + 1).
				Where(cand => Quadratic.Jacobi(n, T.CreateChecked(cand)) == 1).
				Select(bi => (int)bi).
				ToArray();
		}
		//!+TODO: Implement case where we need more primes!!!
		throw new NotImplementedException();
	}

	static private readonly double BExp = 3 * Sqrt(2) / 4;

	private static int DetermineB<T>(T n) where T : IBinaryInteger<T>
	{
		var logN = Log(double.CreateChecked(n));
		return (int)Pow(Exp(Sqrt(logN * Log(logN))), BExp);
	}
}
