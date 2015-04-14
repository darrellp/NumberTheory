using System;
using System.Collections;
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
	class Eratosthenes
	{

#if NOTYET
		/// ////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		///  Basic sieve that finds all primes from 2 to n.
		/// </summary>
		/// <remarks>
		/// The limit can't be too high for this function since it enumerates all the way up to n.  We allow
		/// for n to be an nt and return a list of nt's mainly as a convenience but understand that BigIntegers
		/// would be inappropriate here.
		/// 
		/// Note also that if this produces more primes than SmallPrimes currently holds, SmallPrimes is reset to
		/// hold the returned list if memoize is true.  
		/// Darrellp - 8/28/14
		/// </remarks>
		/// <param name="n">Limit to find primes to.</param>
		/// <param name="memoize">True if we should replace SmallPrimes with the returned list.</param>
		/// <returns>System.Collections.Generic.List&lt;System.Int32&gt;.</returns>
		/// ////////////////////////////////////////////////////////////////////////////////////////////////////
		static IEnumerable<nt> Basic(nt n, bool memoize = true)
		{
			var nT = (int) n;
			var isPrime = new BitArray(nT);
			var smallPrimesCount = Primes.SmallPrimes.Length;

			if (nT <= smallPrimesCount)
			{
				return Primes.SmallPrimes.Take(nT).ToArray();
			}

			// We avoid the multiple enumeration by returning SmallPrimes if we memoized, else returning
			// the enumeration smallPrimes...

			// ReSharper disable PossibleMultipleEnumeration
			var smallPrimes = Primes.SmallPrimes.Concat(Bounded(Primes.SmallPrimes[smallPrimesCount - 1] + 1, nT));
			if (memoize)
			{
				Primes.SmallPrimes = smallPrimes.ToArray();
			}

			return memoize ? Primes.SmallPrimes : smallPrimes;
			// ReSharper restore PossibleMultipleEnumeration
		}

		//private static IEnumerable<nt> Bounded(nt left, nt right, int blockSize = -1)
		//{
		//	if (left <= 2)
		//	{
		//		left = 2;
		//		yield return 2;
		//	}

		//	// Left and right assumed even...
		//	left = left - (left & 1);
		//	right = right + (right & 1);

		//	if (blockSize == -1)
		//	{
		//		blockSize = (int)(right - left);
		//	}

		//	var block = new BitArray(blockSize);
		//	var smallPrimes = Basic((right + 1).IntegerSqrt());
		//	var starts = smallPrimes.Select(p => (-(left + 1 + p) / 2).Normalize(p)).ToArray();
		//	var next = left;
		//	while (next < right)
		//	{
		//		foreach (var q in starts)
		//		{
		//			for (nt j = q; j < blockSize; j++)
		//			{
		//				block[j] = true;
		//			}
		//		}
		//	}
		//}
#endif
	}
}
