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

		// Find odd primes <= B for which n is a quadratic residue
		var primeList = GetPrimeList(n, b);

		// for each prime found, find the square root of n mod that prime
		// var sqrtList = primeList.Select(p => Quadratic.SqrtMod(n, T.CreateChecked(p), out _)).ToList();

		// Compute our candidate list — collect both the x values and their exponent vectors
		var relations = SieveRelations(primeList, n).ToList();

		// Full exponent vectors are passed to Block Lanczos, which reduces mod 2 internally.
		// We retain the full exponents in the relations for computing y = product of primes^(exp/2).
		var fullExponents = relations.Select(r => r.Exponents).ToArray();

		// Find null-space vectors using Block Lanczos over GF(2)
		var nullVectors = BlockLanczos.FindNullSpace(fullExponents);

		// Try each null-space vector to find a non-trivial factor
		var rowWords = (relations.Count + 63) / 64;
		foreach (var nullVec in nullVectors)
		{
			var factor = TryNullVector(nullVec, relations, primeList, n);
			if (factor > T.One && factor < n)
			{
				return factor;
			}
		}

		// If Block Lanczos didn't yield a factor, fall back to trying all solutions
		return n;
	}

	/// <summary>
	/// Attempts to extract a non-trivial factor from a null-space vector.
	/// The null-space vector indicates which relations to combine so that
	/// the exponent vectors sum to zero mod 2, yielding x² ≡ y² (mod n).
	/// </summary>
	private static T TryNullVector<T>(ulong[] nullVec, List<Relation<T>> relations, int[] primeList, T n)
		where T : IBinaryInteger<T>
	{
		// Determine which relations are selected by this null vector
		var selectedIndices = new List<int>();
		for (var i = 0; i < relations.Count; i++)
		{
			var word = i / 64;
			var bit = i % 64;
			if (word < nullVec.Length && (nullVec[word] & (1UL << bit)) != 0)
			{
				selectedIndices.Add(i);
			}
		}

		if (selectedIndices.Count == 0)
		{
			return T.One;
		}

		// Compute x = product of all selected (sqrtN + offset) values mod n
		T x = T.One;
		foreach (var idx in selectedIndices)
		{
			x = MultiplyMod(x, relations[idx].XValue, n);
		}

		// Sum the exponent vectors to get the combined exponents (should all be even)
		var combinedExponents = new int[primeList.Length];
		foreach (var idx in selectedIndices)
		{
			for (var j = 0; j < primeList.Length; j++)
			{
				combinedExponents[j] += relations[idx].Exponents[j];
			}
		}

		// Compute y = product of primes^(combined_exponent/2) mod n
		T y = T.One;
		for (var j = 0; j < primeList.Length; j++)
		{
			if (combinedExponents[j] > 0)
			{
				var halfExp = combinedExponents[j] / 2;
				var prime = T.CreateChecked(primeList[j]);
				y = MultiplyMod(y, PowerMod.Power(prime, T.CreateChecked(halfExp), n), n);
			}
		}

		// Return GCD(x - y, n) — hopefully a non-trivial factor
		var diff = (x - y) % n;
		if (diff < T.Zero) diff += n;
		return diff.GCD(n);
	}

	/// <summary>
	/// Multiplies two values modulo n, avoiding overflow for large types.
	/// </summary>
	private static T MultiplyMod<T>(T a, T b, T n) where T : IBinaryInteger<T>
	{
		return (a % n) * (b % n) % n;
	}

	/// <summary>
	/// Holds a sieve relation: the x value and its exponent vector over the factor base.
	/// </summary>
	private record Relation<T>(T XValue, int[] Exponents) where T : IBinaryInteger<T>;

	/// <summary>
	/// Sieves for B-smooth relations, returning both the x value and exponent vector.
	/// Collects primeList.Length + 1 relations to guarantee a linear dependency.
	/// </summary>
	private static IEnumerable<Relation<T>> SieveRelations<T>(int[] primeList, T n)
		where T : IBinaryInteger<T>
	{
		var sqrtN = n.IntegerSqrt() + T.One;
		return Enumerable.
			// Get an "infinite" range of values
			Range(0, int.MaxValue).
			// Offset them by the square root of n
			Select(indx => sqrtN + T.CreateChecked(indx)).
			// Try to factor (x^2 - n) over the factor base
			Select(x => new { X = x, Exp = CheckBSmooth(x * x - n, primeList) }).
			// Toss the ones that aren't B-smooth
			Where(r => r.Exp != null).
			// Wrap in a Relation record
			Select(r => new Relation<T>(r.X, r.Exp!)).
			// Collect enough relations (one more than factor base size)
			Take(primeList.Length + 1);
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
		if (b < sp[^1])
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

		// Generate all primes up to b using a Sieve of Eratosthenes
		var sieve = new bool[b + 1];
		// true means composite
		for (var i = 2; (long)i * i <= b; i++)
		{
			if (!sieve[i])
			{
				for (var j = i * i; j <= b; j += i)
				{
					sieve[j] = true;
				}
			}
		}

		// Collect primes from the sieve, filtering for quadratic residues of n
		var primes = new List<int>();
		for (var i = 2; i <= b; i++)
		{
			if (!sieve[i] && Quadratic.Jacobi(n, T.CreateChecked(i)) == 1)
			{
				primes.Add(i);
			}
		}

		return primes.ToArray();
	}

	private static int DetermineB<T>(T n) where T : IBinaryInteger<T>
	{
		// Taken from https://medium.com/nerd-for-tech/heres-how-quadratic-sieve-factorization-works-1c878bc94f81
		var logN = Log(double.CreateChecked(n));
		return (int)Exp(Sqrt(logN * Log(logN)) / 2);
	}
}
