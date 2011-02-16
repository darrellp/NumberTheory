#if BIGINTEGER
using System.Linq;
using System.Numerics;
using nt = System.Numerics.BigInteger;
#elif LONG
using System;
using nt = System.Int64;
#endif


#if BIGINTEGER
namespace NumberTheoryBig
#elif LONG
namespace NumberTheoryLong
#endif
{
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>	Utilities for the Number Theory routines. </summary>
	///
	/// <remarks>	Darrellp, 2/15/2011. </remarks>
	////////////////////////////////////////////////////////////////////////////////////////////////////

	public static class Utilities
	{
		internal static int TwosExponent(this nt n, out nt m)
		{
			var exp = 0;
			m = n;
			while ((m & 1) == 0)
			{
				exp++;
				m >>= 1;
			}
			return exp;
		}

		internal static int NegOnePower(this nt n)
		{
			return ((n & 1) == 0) ? 1 : -1;
		}

		internal static int NegOnePower(this int n)
		{
			return ((n & 1) == 0) ? 1 : -1;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Normalizes a mod value to lie between 0 and mod-1. </summary>
		///
		/// <remarks>	Darrellp, 2/16/2011. </remarks>
		///
		/// <param name="n">	The value we're trying to normalize. </param>
		/// <param name="mod">	The mod we're normalizing to. </param>
		///
		/// <returns>	a value congruent to n Mod mod lying between 0 and mod-1. </returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		public static nt Normalize(this nt n, nt mod)
		{
			if (n < 0)
			{
				return n + ((mod -n - 1)/mod)*mod;
			}
			if (n > mod)
			{
				return n - (n/mod)*mod;
			}
			return n;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	
		/// Integer Square Root.  This is the version given in Crandall and Pomerance's "Prime Numbers: A
		/// Computational Perspective". 
		/// </summary>
		///
		/// <remarks>	Darrellp, 2/15/2011. </remarks>
		///
		/// <param name="n">	The value we're investigating. </param>
		///
		/// <returns>	. </returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		public static nt ISqrt(this nt n)
		{
			// Take care of 0 and 1 trivially
			if (n <= 1)
			{
				return n;
			}

			// Make a guess to initialize Newton's method
			var x = (nt)1 << ((n.BitCount() + 1)/2);
			nt y;

			// Loop through Newton's method
			while (x > (y = (x + n/x)/2))
			{
				// x[i] = x[i-1]
				x = y;
			}

			// Return the outcome
			return x;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Returns true if n is a perfect square. </summary>
		///
		/// <remarks>	Darrellp, 2/15/2011. </remarks>
		///
		/// <param name="n">	The value we're investigating. </param>
		///
		/// <returns>	true if n is a perfect square. </returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		public static bool IsPerfectSquare(this nt n)
		{
			var sqrt = ISqrt(n);

			// Check that our integer square root is our real square root
			return sqrt*sqrt == n;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	
		/// The count of bits used in the binary representation of n.  For these purposes the count of
		/// bits used to represent 0 is one. 
		/// </summary>
		///
		/// <remarks>	Darrellp, 2/15/2011. </remarks>
		///
		/// <param name="n">	The value we're investigating. </param>
		///
		/// <returns>	. </returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		public static int BitCount(this nt n)
		{
			// If n is zero
			if (n == 0)
			{
				// We special case it
				return 1;
			}
			#if BIGINTEGER
			// Retrieve the raw bytes of n
			var bytes = n.ToByteArray();

			// Initialize for the for loop
			var bHigh = bytes[bytes.Length - 1];
			var cbitsHigh = 0;
			
			// Shift to count bits in the high order byte
			for (; bHigh != 0; bHigh >>= 1, cbitsHigh++) { }

			// Return the bits in the high byte plus all the bits in lower bytes
			return cbitsHigh + 8 * (bytes.Length - 1);
			#else
			int cBits = 0;

			// Shift to count bits in the high order byte
			for (; n != 0; n >>= 1, cBits++){}

			// Return the count
			return cBits;
			#endif
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	
		/// Largest power of 2 smaller than n - i.e., a single bit at the same position as 
		/// the top bit of n. 
		/// </summary>
		///
		/// <remarks>	Darrellp, 2/15/2011. </remarks>
		///
		/// <param name="n">	The value we're investigating. </param>
		///
		/// <returns>	The largest power of 2 smaller than n. </returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		public static nt TopBitMask(this nt n)
		{
			// ReSharper disable JoinDeclarationAndInitializer
			nt mask;
			// ReSharper restore JoinDeclarationAndInitializer
			#if BIGINTEGER
			// Retrieve the raw bytes of n
			var bytes = n.ToByteArray();

			// Get the high order byte shifted right one bit
			var bHigh = (byte)(bytes[bytes.Length - 1] >> 1);
			byte bMask = 1;
			var rgbMask = Enumerable.Repeat<byte>(0, bytes.Length).ToArray();
			
			// Shift mask to account for bits in the high order byte
			for (; bHigh != 0; bHigh >>= 1, bMask <<= 1) { }
			rgbMask[bytes.Length - 1] = bMask;

			// Return the mask
			mask = new BigInteger(rgbMask);
			#else
			if (n == 0)
			{
				return 0;
			}
			mask = (~nt.MaxValue >> 1) & nt.MaxValue;
			for (; (mask & n) == 0; mask >>= 1) { }
			#endif
			return mask;
		}
	}
}
