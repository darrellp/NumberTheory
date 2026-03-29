using System;
using System.ComponentModel;
using System.Numerics;

namespace NumberTheory;

/// <summary>
/// Utilities for the Number Theory routines.
/// </summary>
public static class Utilities
{
	internal static int TwosExponent<T>(this T n, out T m) where T : IBinaryInteger<T>
	{
		var exp = 0;
		m = n;
		while ((m & T.One) == T.Zero)
		{
			exp++;
			m >>= 1;
		}
		return exp;
	}

	internal static int NegOnePower<T>(this T n) where T : IBinaryInteger<T>
	{
		return (n & T.One) == T.Zero ? 1 : -1;
	}

	internal static int NegOnePower(this int n)
	{
		return (n & 1) == 0 ? 1 : -1;
	}

	/// <summary>	Normalizes a mod value to lie between 0 and mod-1. </summary>
	/// <param name="n">	The value we're trying to normalize. </param>
	/// <param name="mod">	The mod we're normalizing to. </param>
	/// <returns>	a value congruent to n Mod mod lying between 0 and mod-1. </returns>
	public static T Normalize<T>(this T n, T mod) where T : IBinaryInteger<T>
	{
		if (n < T.Zero)
		{
			return n + (mod - n - T.One) / mod * mod;
		}
		if (n >= mod)
		{
			return n - n / mod * mod;
		}
		return n;
	}

	/// <summary>
	/// Integer Square Root. This is the version given in Crandall and Pomerance's "Prime Numbers: A
	/// Computational Perspective".
	/// </summary>
	/// <param name="n">	The value we're investigating. </param>
	/// <returns>	Floor(Sqrt(n)) </returns>
	public static T IntegerSqrt<T>(this T n) where T : IBinaryInteger<T>
	{
		// Take care of 0 and 1 trivially
		if (n <= T.One)
		{
			return n;
		}

		// Make a guess to initialize
		var x = T.One << ((n.BitCount() + 1) / 2);
		T y;

		var two = T.One + T.One;
		// Loop through Newton's method
		while (x > (y = (x + n / x) / two))
		{
			// x[i] = x[i-1]
			x = y;
		}

		// Return the outcome
		return x;
	}

	private static T NewtonRootStep<T>(T k, T n, T x) where T : IBinaryInteger<T>
	{
		var xpow = PowerMod.Power(x, k - T.One);
		if (T.IsZero(xpow))
		{
			return x;
		}
		return ((k - T.One) * x * xpow + n) / (k * xpow);
	}

	/// <summary>
	/// Integer Root using Newton's method.
	/// </summary>
	/// <param name="n">	The value we're investigating. </param>
	/// <param name="k">	Order of the root. </param>
	/// <returns>	Floor(KthRoot(n, k)) </returns>
	public static T IntegerRoot<T>(T n, T k) where T : IBinaryInteger<T>
	{
		if (n <= T.One)
		{
			return n;
		}

		// Use bit count to get a reasonable starting estimate
		var bc = n.BitCount();
		var kInt = int.CreateChecked(k);
		var startBits = (bc + kInt - 1) / kInt;
		var x = T.One << startBits;

		if (x <= T.One)
		{
			x = T.One + T.One;
		}

		// Newton's method iteration
		var y = NewtonRootStep(k, n, x);
		while (y < x)
		{
			x = y;
			y = NewtonRootStep(k, n, x);
		}
		return x;
	}

	/// <summary>	Returns true if n is a perfect square. </summary>
	/// <param name="n">	The value we're investigating. </param>
	/// <returns>	true if n is a perfect square. </returns>
	public static bool IsPerfectSquare<T>(this T n) where T : IBinaryInteger<T>
	{
		var sqrt = IntegerSqrt(n);

		// Check that our integer square root is our real square root
		return sqrt * sqrt == n;
	}

	/// <summary>
	/// The count of bits used in the binary representation of n.
	/// </summary>
	/// <param name="n">	The value we're investigating. </param>
	/// <returns>   The number of bits needed to represent n. </returns>
	public static int BitCount<T>(this T n) where T : IBinaryInteger<T>
	{
		// If n is zero
		if (T.IsZero(n))
		{
			// We special case it
			return 1;
		}
		var cBits = 0;

		// Shift to count bits
		for (var temp = n; !T.IsZero(temp); temp >>= 1, cBits++) { }

		// Return the count
		return cBits;
	}

	/// <summary>
	/// Largest power of 2 smaller than n - i.e., a single bit at the same position as
	/// the top bit of n.
	/// </summary>
	/// <param name="n">	The value we're investigating. </param>
	/// <returns>	The largest power of 2 smaller than n. </returns>
	public static T TopBitMask<T>(this T n) where T : IBinaryInteger<T>
	{
		return T.IsZero(n) ? T.Zero : T.One << (BitCount(n) - 1);
	}
	
	/// <summary>
	/// Returns true if numerator divides denominator
	/// </summary>
	/// <param name="numerator">	The top number in the fraction. </param>
	/// <param name="denominator">	The bottom number in the fraction. </param>
	/// <returns>	true if the numerator can be divided by the numerator </returns>
	public static bool Divides<T>(this T numerator, T denominator) where T : IBinaryInteger<T>
	{
		return (numerator / denominator) * denominator == numerator;
	}

	public static T PositiveMod<T>(this T val, T mod) where T : IBinaryInteger<T>
	{
		var divT = T.Abs(mod);
		if (val < T.Zero)
		{
			return val + (T.One - val / divT) * mod;
		}
		return val % mod;
	}
}
