#if BIGINTEGER
using NumberTheoryBig;
using nt = System.Numerics.BigInteger;
#else
using NumberTheoryLong;
using nt = System.Int64;
#endif


#if BIGINTEGER
namespace NumberTheoryBig
#elif LONG
namespace NumberTheoryLong
#endif
{
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>   Static class to handle Quadratic residues and related stuff. </summary>
	///
	/// <remarks>   Darrellp, 2/12/2011. </remarks>
	////////////////////////////////////////////////////////////////////////////////////////////////////

	static public class Quadratic
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>   Returns the Jacobi symbol for the input values </summary>
		///
		/// <remarks>   Uses the algorithm as given in Computational Number Theory by Wagon and Bressoud.
		/// This is the iterative, optimized solution.  I didn't use the table lookup because the arithmetic
		/// is done in integers so it's not a big hit, it's easier to understand without the lookup and the
		/// lookup incorporates taking -1 to the power so doesn't work with the pure arithmetic version
		/// I use here.  I'm not sure why they don't just glom all the exponents together in the book.
		/// Seems preferable and faster to me since they take two exponents when one is sufficient.
		/// Darrellp, 2/12/2011. </remarks>
		///
		/// <param name="a">    a of (a/n) </param>
		/// <param name="n">    n of (a/n) </param>
		///
		/// <returns>  The Jacobi symbol value of (a/n) </returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		static public int Jacobi(nt a, nt n)
		{
			// Is n even?
			if ((n & 1) != 1)
			{
				return 0;
			}

			// Get a value to correct for negative "numerator"
			var negCorrection = 1;

			// Is a Negative?
			if (a < 0)
			{
				// Reverse sign and set our correction factor properly
				a = -a;
				negCorrection = ((n - 1)/2).NegOnePower();
			}

			// Use the absolute value of the "denominator"
			n = TypeAdaptation.Abs(n);

			// Setup for loop
			var gcd = a;
			var nCur = n;
			var c = 1;

			while (true)
			{
				var rCap = gcd % nCur;
				gcd = nCur;

				if (rCap == 0)
				{
					break;
				}

				nt r;
				var s = rCap.TwosExponent(out r);
				var nCurMod8 = (int) (nCur & 7);
				c *= ((2 * (nCurMod8 - 1) * (r - 1) + s * (nCurMod8 * nCurMod8 - 1)) / 8).NegOnePower();
				nCur = r;
			}
			return negCorrection * (gcd != 1 ? 0 : c);
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Takes the square root of a mod p if one exists. </summary>
		///
		/// <remarks>	
		/// <para>The out parameter, fSuccess, indicates our ability to locate a square root.</para>
		/// <para>This algorithm is essentially the one in the Pomerance book "Prime Numbers...".  I
		/// think that it's a teeny bit more efficient than "Tonelli's algorithm" in Wagon's CNT but
		/// they're both the same idea.  In CNT, they only check primes when looking for non-residues.
		/// I'm not sure why.  Pomerance makes no mention of any necessity for doing this nor do I
		/// see one.</para>
		/// Darrellp, 2/16/2011. 
		/// </remarks>
		///
		/// <param name="a">		The number whose square root is desired </param>
		/// <param name="p">		The prime we're doing this modulo. </param>
		/// <param name="fSuccess">	[out] False if there is no square root. </param>
		///
		/// <returns>	The square root if fSuccess is true, else indeterminate. </returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		static public nt SqrtMod(nt a, nt p, out bool fSuccess)
		{
			// If there's no square root
			if (Jacobi(a, p) != 1)
			{
				// Indicate that there's no square root and return
				fSuccess = false;
				return 0;
			}

			// Indicate that we can find a square root
			fSuccess = true;

			// If p is not congruent to 1 mod 8
			if (p % 8 != 1)
			{
				// Return the simple case
				return SqrtSimpleCase(a, p);
			}

			// Find a random non-residue
			var d = NonResidue(p);

			// Set up A, D and m for the loop
			nt t;
			var s = (p - 1).TwosExponent(out t);
			var aCap = PowerMod.Power(a, t, p);
			var dCap = PowerMod.Power(d, t, p);
			nt m = 0;

			// For each step in the halving
			for (var iHalve = 0; iHalve < s; iHalve++ )
			{
				// Modify m appropriately
				var prod = (aCap*PowerMod.Power(dCap, m, p)) << (s - 1 - iHalve);

				// If our product is congruent to -1 mod p
				if ((prod % p) == p - 1)
				{
					// Adjust m accordingly
					m += ((nt) 1 << iHalve);
				}
			}

			// Return our square root
			return (PowerMod.Power(a, (t + 1)/2, p)*PowerMod.Power(dCap, m/2, p))%p;
		}

		private static nt NonResidue(nt p)
		{
			// For each possible non-residue
			for (nt i = 2; i < p; i++)
			{
				// Is it, in fact, a non-residue?
				if (Jacobi(i, p) != 1)
				{
					// Return it.
					return i;
				}
			}
			// We'll never get here
			return -1;
		}

		// This routine is only called if a is not congruent to 1 mod 8.
		private static nt SqrtSimpleCase(nt a, nt p)
		{
			nt x;

			// if a is congruent to 5 mod 8
			if ((a % 8) == 5)
			{
				x = PowerMod.Power(a, (p + 3)/8, p);
				
				// If the current value of x is not a square root
				if (x * x % p != a % p)
				{
					// Modify it
					x = (x * PowerMod.Power(2, (p - 1)/4, p)) % p;
				}
			}
			else
			{
				// Simple case when a is congruent to 3 or 7 mod 8
				x = PowerMod.Power(a, (p + 1)/4, p);
			}
			return x;
		}

	}
}
