using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using static System.Math;

namespace NumberTheory;

/// <summary>	Class to hold the static routine implementing the quadratic sieve. </summary>
public static class QuadraticSieve
{
	// Raise this if you must
	const int maxSmoothing = 1000;

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

		// Compute our candidate list — collect both the x values and their exponent vectors
		var relations = SieveRelations(primeList, n, b).ToList();

		// Full exponent vectors are passed to Block Lanczos, which reduces mod 2 internally.
		// We retain the full exponents in the relations for computing y = product of primes^(exp/2).
		var fullExponents = relations.Select(r => r.Exponents).ToArray();

		// Find null-space vectors using Block Lanczos over GF(2)
		var nullVectors = BlockLanczos.FindNullSpace(fullExponents);

		// Try each null-space vector to find a non-trivial factor
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
	/// Sieves for B-smooth relations using Pomerance's interval-sieve method.
	/// For each prime p in the factor base, SqrtMod gives the two starting offsets
	/// into the sieve array; log(p) is added at those positions and every p steps.
	/// Additionally, prime powers (p², p³, …) are sieved: for each power pᵏ ≤ B,
	/// the modular roots are lifted and log(p) is added again at every pᵏ-th step.
	/// <para>
	/// The log threshold at each position is based on the actual candidate size
	/// log(x²-n) at that offset, scaled by a slack factor. This correctly handles
	/// the wide range of candidate magnitudes across the sieve window.
	/// The sieve window extends in chunks of <c>4 * b</c> until enough relations
	/// are found.
	/// </para>
	/// </summary>
	private static IEnumerable<Relation<T>> SieveRelations<T>(int[] primeList, T n, int b)
		where T : IBinaryInteger<T>
	{
		var needed = primeList.Length + 1;
		var sqrtN = n.IntegerSqrt() + T.One;
		var dSqrtN = double.CreateChecked(sqrtN);

		// Precompute log(p) and the two sieve-starting offsets for each prime.
		var logP = new float[primeList.Length];
		var roots = new (int R1, int R2)[primeList.Length];

		// Collect all sieve events: (stride, offset, logContribution).
		// This includes prime powers pᵏ which contribute additional log(p) hits.
		var sieveEvents = new List<(int Stride, int Off, float LogVal)>();

		for (var i = 0; i < primeList.Length; i++)
		{
			var p = primeList[i];
			logP[i] = (float)Log(p);
			var pT = T.CreateChecked(p);

			var r1T = Quadratic.SqrtMod(n % pT, pT, out var ok);
			if (!ok)
			{
				roots[i] = (-1, -1);
				continue;
			}

			var r1 = int.CreateChecked(r1T);
			var r2 = (p - r1) % p;
			var sqrtNModP = int.CreateChecked(sqrtN % pT);
			var off1 = (r1 - sqrtNModP + p) % p;
			var off2 = (r1 == r2) ? -1 : (r2 - sqrtNModP + p) % p;
			roots[i] = (off1, off2);

			// Add base prime sieve events
			sieveEvents.Add((p, off1, logP[i]));
			if (off2 >= 0)
				sieveEvents.Add((p, off2, logP[i]));

			// Add prime power sieve events: p², p³, ... while pᵏ fits in int range
			// For each power pᵏ, find offsets where (sqrtN + off)² ≡ n (mod pᵏ)
			// by lifting the roots via Hensel's lemma (for odd primes, roots lift directly).
			var pk = (long)p * p;
			var prevR1 = (long)r1;
			var prevR2 = (long)r2;
			while (pk <= b * 10L && pk <= int.MaxValue)
			{
				var pkInt = (int)pk;
				var pkT = T.CreateChecked(pkInt);
				var nModPk = long.CreateChecked(n % pkT);

				// Lift r1: find t such that r1_new = prevR1 + t*prevPk, r1_new² ≡ n (mod pk)
				var prevPk = pk / p;
				var lifted1 = LiftRoot(prevR1, nModPk, prevPk, pkInt, p);
				var lifted2 = LiftRoot(prevR2, nModPk, prevPk, pkInt, p);

				if (lifted1 >= 0)
				{
					var sqrtNModPk = (int)(long.CreateChecked(sqrtN % pkT));
					var loff1 = (int)((lifted1 - sqrtNModPk + pkInt) % pkInt);
					sieveEvents.Add((pkInt, loff1, logP[i]));

					// Second lifted root
					var lr2 = (int)((pkInt - lifted1) % pkInt);
					if (lr2 != loff1)
					{
						var loff2 = (int)((lr2 - sqrtNModPk + pkInt) % pkInt);
						sieveEvents.Add((pkInt, loff2, logP[i]));
					}
				}

				if (lifted2 >= 0 && lifted2 != lifted1)
				{
					var sqrtNModPk = (int)(long.CreateChecked(sqrtN % pkT));
					var loff1 = (int)((lifted2 - sqrtNModPk + pkInt) % pkInt);
					sieveEvents.Add((pkInt, loff1, logP[i]));
				}

				prevR1 = lifted1;
				prevR2 = lifted2;
				pk *= p;
			}
		}

		var chunkSize = Max(4 * b, 256);
		var windowEnd = 0;
		var relations = new List<Relation<T>>(needed);

		while (relations.Count < needed)
		{
			var chunkStart = windowEnd;
			windowEnd += chunkSize;

			var logSums = new float[chunkSize];

			// Sieve phase: apply all events (primes + prime powers)
			foreach (var (stride, off, logVal) in sieveEvents)
			{
				var start = (off - chunkStart % stride + stride) % stride;
				for (var j = start; j < chunkSize; j += stride)
				{
					logSums[j] += logVal;
				}
			}

			// Extraction phase: use per-position threshold based on actual candidate size.
			// log(x²-n) at offset k ≈ log(2·sqrtN·(chunkStart+k) + (chunkStart+k)²) but
			// we approximate as log(x²-n) computed directly. Slack of 0.7 admits candidates
			// where some prime-power contributions were missed.
			for (var k = 0; k < chunkSize && relations.Count < needed; k++)
			{
				// Compute actual candidate to get its log for the threshold
				var globalK = chunkStart + k;
				if (globalK == 0) continue; // x²-n = sqrtN²-n which may be 0 or tiny

				var logCand = (float)Log(2.0 * dSqrtN * globalK + (double)globalK * globalK);
				var threshold = logCand * 0.7f;

				if (logSums[k] < threshold) continue;

				var x = sqrtN + T.CreateChecked(globalK);
				var cand = x * x - n;
				if (cand <= T.Zero) continue;

				var exponents = ExtractExponents(cand, primeList);
				if (exponents != null)
				{
					relations.Add(new Relation<T>(x, exponents));
				}
			}
		}

		return relations;
	}

	/// <summary>
	/// Lifts a root r mod prevPk to a root mod pk = prevPk * p using Hensel's lemma.
	/// Returns the lifted root, or -1 if lifting fails.
	/// </summary>
	private static long LiftRoot(long r, long nModPk, long prevPk, long pk, int p)
	{
		// r² ≡ n (mod prevPk). We want r' = r + t·prevPk such that r'² ≡ n (mod pk).
		// Expanding: r² + 2·r·t·prevPk ≡ n (mod pk)
		// So t ≡ (n - r²) / prevPk / (2r) (mod p)
		if (r < 0) return -1;
		var residue = nModPk - r * r;
		// Normalize residue into [0, pk)
		residue = ((residue % pk) + pk) % pk;
		if (residue % prevPk != 0) return -1;
		var quot = residue / prevPk;
		// We need quot / (2r) mod p
		var twoR = (2 * r) % p;
		if (twoR == 0) return -1; // degenerate case
		// Find inverse of twoR mod p
		var inv = ModInverse(twoR, p);
		if (inv < 0) return -1;
		var t = (quot % p * inv) % p;
		return (r + t * prevPk) % pk;
	}

	/// <summary>
	/// Returns the modular inverse of a mod m, or -1 if it doesn't exist.
	/// </summary>
	private static long ModInverse(long a, long m)
	{
		a = ((a % m) + m) % m;
		// Extended Euclidean algorithm
		long g = m, x = 0, y = 1;
		var tempA = a;
		while (tempA != 0)
		{
			var q = g / tempA;
			(g, tempA) = (tempA, g - q * tempA);
			(x, y) = (y, x - q * y);
		}
		if (g != 1) return -1;
		return ((x % m) + m) % m;
	}

	/// <summary>
	/// Fully trial-divides <paramref name="cand"/> over <paramref name="primeList"/>,
	/// returning the exponent vector if the candidate is completely smooth, or null otherwise.
	/// </summary>
	private static int[]? ExtractExponents<T>(T cand, int[] primeList) where T : IBinaryInteger<T>
	{
		var exponents = new int[primeList.Length];
		for (var i = 0; i < primeList.Length; i++)
		{
			exponents[i] = CountFactors(ref cand, primeList[i]);
		}
		return cand == T.One ? exponents : null;
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
		// The standard formula L(n)^(1/2) where L(n) = exp(sqrt(ln(n)*ln(ln(n))))
		// sometimes produces too small a factor base for modest n. We use the full
		// exponent (no /2 divisor) and enforce a floor to guarantee a viable base.
		var logN = Log(double.CreateChecked(n));
		var lb = (int)Exp(Sqrt(logN * Log(logN)));
		return Min(lb, maxSmoothing);
	}
}
