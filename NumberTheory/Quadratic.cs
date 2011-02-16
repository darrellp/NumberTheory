#if BIGINTEGER
using nt = System.Numerics.BigInteger;
#elif LONG
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
	}
}
