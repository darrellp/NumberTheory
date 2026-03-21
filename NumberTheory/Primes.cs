using System;
using System.Linq;
using System.Numerics;

namespace NumberTheory;

/// <summary>   Static class for functions dealing with prime numbers. </summary>
public static class Primes
{
	// Primes less than 1000
	internal static readonly long[] SmallPrimes =
	[
		2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59,
		61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127, 131,
		137, 139, 149, 151, 157, 163, 167, 173, 179, 181, 191, 193, 197,
		199, 211, 223, 227, 229, 233, 239, 241, 251, 257, 263, 269, 271,
		277, 281, 283, 293, 307, 311, 313, 317, 331, 337, 347, 349, 353,
		359, 367, 373, 379, 383, 389, 397, 401, 409, 419, 421, 431, 433,
		439, 443, 449, 457, 461, 463, 467, 479, 487, 491, 499, 503, 509,
		521, 523, 541, 547, 557, 563, 569, 571, 577, 587, 593, 599, 601,
		607, 613, 617, 619, 631, 641, 643, 647, 653, 659, 661, 673, 677,
		683, 691, 701, 709, 719, 727, 733, 739, 743, 751, 757, 761, 769,
		773, 787, 797, 809, 811, 821, 823, 827, 829, 839, 853, 857, 859,
		863, 877, 881, 883, 887, 907, 911, 919, 929, 937, 941, 947, 953,
		967, 971, 977, 983, 991, 997
	];

	/// <summary>   Prime by division. </summary>
	/// <remarks>   Determines if n might be composite by dividing it by several small primes. </remarks>
	/// <param name="n">    Value to be tested. </param>
	/// <returns>   true implies n is composite, false if the test is indeterminate. </returns>
	public static bool CompositeByDivision<T>(this T n) where T : IBinaryInteger<T>
	{
		return SmallPrimes.Any(p =>
		{
			var pVal = T.CreateChecked(p);
			return n != pVal && n % pVal == T.Zero;
		});
	}

	/// <summary>   Simple pseudoprime algorithm to determine if p passes the b-pseudoprime test. </summary>
	/// <param name="p">    The proposed prime we're testing. </param>
	/// <param name="b">    The exponent we're testing against. </param>
	/// <returns>   true if p is exhibiting prime like behavior, false if p is composite. </returns>
	public static bool PsuedoPrimeTest<T>(T p, T b) where T : IBinaryInteger<T>
	{
		return PowerMod.Power(b, p - T.One, p) == T.One;
	}

	/// <summary>   Determine if p passes the b-strong pseudoprime test. </summary>
	/// <param name="n">    The value to test. </param>
	/// <param name="b">    The exponent we're testing against. </param>
	/// <returns>   true if it succeeds, false if it fails. </returns>
	public static bool StrongPsuedoPrimeTest<T>(T n, T b) where T : IBinaryInteger<T>
	{
		if (n <= T.Zero)
		{
			throw new ArgumentException("Negative value in StrongPsuedoPrimeTest");
		}
		if (n % b == T.Zero)
		{
			return n == b;
		}

		var s = (n - T.One).TwosExponent(out var nReduced);
		var bp = PowerMod.Power(b, nReduced, n);
		if (bp == T.One)
		{
			return true;
		}

		var two = T.One + T.One;
		for (var i = 0; i < s; i++)
		{
			if (bp == T.One)
			{
				return false;
			}
			if (bp == n - T.One)
			{
				return true;
			}
			bp = PowerMod.Power(bp, two, n);
		}
		return false;
	}

	/// <summary>   Query if 'n' is prime. </summary>
	/// <param name="n">    Value to be tested. </param>
	/// <returns>   true if prime, false if not. </returns>
	public static bool IsPrime<T>(this T n) where T : IBinaryInteger<T>
	{
		return !CompositeByDivision(n) &&
			   StrongPsuedoPrimeTest(n, T.One + T.One) &&
			   StrongPsuedoPrimeTest(n, T.CreateChecked(3)) &&
			   Lucas.LucasPsuedoprimeTest(n);
	}
}
