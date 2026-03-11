using System.Numerics;

namespace NumberTheory;

/// <summary>   Static class to handle Quadratic residues and related stuff. </summary>
public static class Quadratic
{
	/// <summary>   Returns the Jacobi symbol for the input values </summary>
	/// <remarks>
	/// Uses the algorithm as given in Computational Number Theory by Wagon and Bressoud.
	/// This is the iterative, optimized solution.
	/// </remarks>
	/// <param name="a">    a of (a/n) </param>
	/// <param name="n">    n of (a/n) </param>
	/// <returns>  The Jacobi symbol value of (a/n) </returns>
	public static int Jacobi<T>(T a, T n) where T : IBinaryInteger<T>
	{
		// Is n even?
		if ((n & T.One) != T.One)
		{
			return 0;
		}

		// Get a value to correct for negative "numerator"
		var negCorrection = 1;

		// Is a Negative?
		if (a < T.Zero)
		{
			// Reverse sign and set our correction factor properly
			a = -a;
			negCorrection = ((n - T.One) / (T.One + T.One)).NegOnePower();
		}

		// Use the absolute value of the "denominator"
		n = T.Abs(n);

		// Setup for loop
		var gcd = a;
		var nCur = n;
		var c = 1;

		while (true)
		{
			var rCap = gcd % nCur;
			gcd = nCur;

			if (T.IsZero(rCap))
			{
				break;
			}

			var s = rCap.TwosExponent(out var r);
			var nCurMod8 = int.CreateChecked(nCur & T.CreateChecked(7));
			c *= ((2 * (nCurMod8 - 1) * (int.CreateChecked(r) - 1) + s * (nCurMod8 * nCurMod8 - 1)) / 8).NegOnePower();
			nCur = r;
		}
		return negCorrection * (gcd != T.One ? 0 : c);
	}

	/// <summary>	Takes the square root of a mod p if one exists. </summary>
	/// <remarks>
	/// The out parameter, fSuccess, indicates our ability to locate a square root.
	/// This algorithm is essentially the one in the Pomerance book "Prime Numbers...".
	/// </remarks>
	/// <param name="a">		The number whose square root is desired </param>
	/// <param name="p">		The prime we're doing this modulo. </param>
	/// <param name="fSuccess">	[out] False if there is no square root. </param>
	/// <returns>	The square root if fSuccess is true, else indeterminate. </returns>
	public static T SqrtMod<T>(T a, T p, out bool fSuccess) where T : IBinaryInteger<T>
	{
		var two = T.One + T.One;
		var eight = T.CreateChecked(8);

		// If there's no square root
		if (Jacobi(a, p) != 1)
		{
			// Indicate that there's no square root and return
			fSuccess = false;
			return T.Zero;
		}

		// Indicate that we can find a square root
		fSuccess = true;

		// If p is not congruent to 1 mod 8
		if (p % eight != T.One)
		{
			// Return the simple case
			return SqrtSimpleCase(a, p);
		}

		// Find a random non-residue
		var d = NonResidue(p);

		// Set up A, D and m for the loop
		var s = (p - T.One).TwosExponent(out var t);
		var aCap = PowerMod.Power(a, t, p);
		var dCap = PowerMod.Power(d, t, p);
		T m = T.Zero;

		// For each step in the halving
		for (var iHalve = 0; iHalve < s; iHalve++)
		{
			// Modify m appropriately
			var prod = (aCap * PowerMod.Power(dCap, m, p)) << (s - 1 - iHalve);

			// If our product is congruent to -1 mod p
			if ((prod % p) == p - T.One)
			{
				// Adjust m accordingly
				m += (T.One << iHalve);
			}
		}

		// Return our square root
		return (PowerMod.Power(a, (t + T.One) / two, p) * PowerMod.Power(dCap, m / two, p)) % p;
	}

	private static T NonResidue<T>(T p) where T : IBinaryInteger<T>
	{
		// For each possible non-residue
		for (T i = T.One + T.One; i < p; i++)
		{
			// Is it, in fact, a non-residue?
			if (Jacobi(i, p) != 1)
			{
				// Return it.
				return i;
			}
		}
		// We'll never get here
		return -T.One;
	}

	// This routine is only called if a is not congruent to 1 mod 8.
	private static T SqrtSimpleCase<T>(T a, T p) where T : IBinaryInteger<T>
	{
		T x;
		var two = T.One + T.One;
		var four = two + two;
		var eight = four + four;

		// if a is congruent to 5 mod 8
		if ((a % eight) == T.CreateChecked(5))
		{
			x = PowerMod.Power(a, (p + T.CreateChecked(3)) / eight, p);

			// If the current value of x is not a square root
			if (x * x % p != a % p)
			{
				// Modify it
				x = (x * PowerMod.Power(two, (p - T.One) / four, p)) % p;
			}
		}
		else
		{
			// Simple case when a is congruent to 3 or 7 mod 8
			x = PowerMod.Power(a, (p + T.One) / four, p);
		}
		return x;
	}
}
