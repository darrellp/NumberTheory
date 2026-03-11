using System;
using System.Linq;
using System.Numerics;

namespace NumberTheory;

/// <summary>
/// Static class to hold routines related to the Euclidean algorithm.
/// </summary>
public static class Euclidean
{
	/// <summary>   The standard Euclidean algorithm. </summary>
	/// <param name="val1"> The first value. </param>
	/// <param name="val2"> The second value. </param>
	/// <returns>   The GCD of val1 and val2. </returns>
	public static T GCD<T>(this T val1, T val2) where T : IBinaryInteger<T>
	{
		// Use absolute values
		val1 = T.Abs(val1);
		val2 = T.Abs(val2);

		// While our remainder is non-zero
		while (!T.IsZero(val2))
		{
			// Do another division
			var r = val1 % val2;
			val1 = val2;
			val2 = r;
		}
		// Return the answer
		return val1;
	}

	/// <summary>   The standard Euclidean algorithm for multiple arguments. </summary>
	/// <param name="vals"> A variable-length parameters list containing vals. </param>
	/// <returns>   The GCD of all the values passed in. </returns>
	public static T GCD<T>(params T[] vals) where T : IBinaryInteger<T>
	{
		return vals.Aggregate(GCD);
	}

	/// <summary>   Least Common Multiple for multiple values. </summary>
	/// <param name="vals"> A variable-length parameters list containing vals. </param>
	/// <returns>   The LCM of the passed in values. </returns>
	public static T LCM<T>(params T[] vals) where T : IBinaryInteger<T>
	{
		return vals.Aggregate(LCM);
	}

	/// <summary>   Least Common Multiple. </summary>
	/// <param name="val1"> The first value. </param>
	/// <param name="val2"> The second value. </param>
	/// <returns>   The LCM of the val1 and val2. </returns>
	public static T LCM<T>(this T val1, T val2) where T : IBinaryInteger<T>
	{
		return val1 * val2 / GCD(val1, val2);
	}

	/// <summary>   Implements the extended Euclidean algorithm. </summary>
	/// <param name="val1"> The first value. </param>
	/// <param name="val2"> The second value. </param>
	/// <returns>   GCD of val1 and val2 along with the coefficients to linearly combine them
	/// to yield that GCD. </returns>
	public static EuclideanExt<T> EuclideanExt<T>(this T val1, T val2) where T : IBinaryInteger<T>
	{
		return new EuclideanExt<T>(val1, val2);
	}

	/// <summary>   Solve the diophantine equation ax + by = c. </summary>
	/// <param name="a">    The value of a in ax + by = c </param>
	/// <param name="b">    The value of b in ax + by = c. </param>
	/// <param name="c">    The value of c in ax + by = c. </param>
	/// <returns>   Function which returns a different solution for each different passed in integer
	/// or null if no solution exists.</returns>
	public static Func<T, T[]>? DiophantineSolve<T>(T a, T b, T c) where T : IBinaryInteger<T>
	{
		return DiophantineSolve(a, b, c, out _);
	}

	/// <summary>   Solve the diophantine equation ax + by = c and return the GCD of a and b</summary>
	/// <param name="a">    The value of a in ax + by = c </param>
	/// <param name="b">    The value of b in ax + by = c. </param>
	/// <param name="c">    The value of c in ax + by = c. </param>
	/// <param name="gcd">  The returned GCD of a and b </param>
	/// <returns>   Function which returns a different solution for each different passed in integer or
	/// null if no solution exists.</returns>
	public static Func<T, T[]>? DiophantineSolve<T>(T a, T b, T c, out T gcd) where T : IBinaryInteger<T>
	{
		// Do the extended Euclidean algorithm on a and b
		var ext = new EuclideanExt<T>(a, b);
		gcd = ext.GCD;

		// If c doesn't divide the GCD of a and b, there's no hope
		if (c % gcd != T.Zero)
		{
			return null;
		}

		// Get a solution
		var cnst1 = c * ext.Coeff1 / gcd;
		var cnst2 = c * ext.Coeff2 / gcd;
		var cf1 = b / gcd;
		var cf2 = -a / gcd;
		T q;

		// See which coefficient is largest and try to lower it maximally

		// If it's the first coefficient
		if (T.Abs(cnst1) > T.Abs(cnst2))
		{
			// Get the multiplier for the first coefficient
			q = cnst1 / cf1;
		}
		else
		{
			// Get the multiplier for the second coefficient
			q = cnst2 / cf2;
		}
		// Lower both coefficients by the multiplier
		cnst1 -= q * cf1;
		cnst2 -= q * cf2;

		// Return a function which returns values which solve the equation
		return (Func<T, T[]>)(i => new[] { cf1 * i + cnst1, cf2 * i + cnst2 });
	}

	/// <summary>   Inversion mod m. </summary>
	/// <param name="n">    The number to be inverted. </param>
	/// <param name="mod">  The modulus. </param>
	/// <returns>  1/n Mod mod or -1 if there's no solution </returns>
	public static T InverseMod<T>(this T n, T mod) where T : IBinaryInteger<T>
	{
		var solns = LinearCongruenceSolve(n, T.One, mod);
		if (solns == null)
		{
			return -T.One;
		}
		return solns[0];
	}

	/// <summary>  Solves Mod(a*x, mod) == b for x </summary>
	/// <param name="a">    The value of a in Mod(a*x, mod) == b. </param>
	/// <param name="b">    The value of b in Mod(a*x, mod) == b. </param>
	/// <param name="mod">  The modulus. </param>
	/// <returns>   x between 0 and mod-1 such that Mod(a*x, mod) == b or null if there's no solutions. </returns>
	public static T[]? LinearCongruenceSolve<T>(T a, T b, T mod) where T : IBinaryInteger<T>
	{
		var fnSolns = DiophantineSolve(a, mod, b, out var gcd);
		if (fnSolns is null || gcd > T.CreateChecked(int.MaxValue))
		{
			return null;
		}

		var gcdInt = int.CreateChecked(gcd);
		var ret = new T[gcdInt];
		for (var i = 0; i < gcdInt; i++)
		{
			var sln = fnSolns(T.CreateChecked(i));
			ret[i] = sln[0];
			if (ret[i] > mod)
			{
				ret[i] -= (ret[i] / mod) * mod;
			}
			else if (ret[i] < T.Zero)
			{
				ret[i] += ((mod - T.One - ret[i]) / mod) * mod;
			}
		}

		return ret;
	}
}
