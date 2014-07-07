using System.Linq;
#if BIGINTEGER
using System;
using System.Numerics;
using nt=System.Numerics.BigInteger;
#elif LONG
using System;
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
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>   Static class to hold routines related to the Euclidean algorithm. </summary>
	///
	/// <remarks>   Darrellp, 2/12/2011. </remarks>
	////////////////////////////////////////////////////////////////////////////////////////////////////

	public static class Euclidean
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>   The standard Euclidean algorithm. </summary>
		///
		/// <remarks>   Darrellp, 2/12/2011. </remarks>
		///
		/// <param name="val1"> The first value. </param>
		/// <param name="val2"> The second value. </param>
		///
		/// <returns>   The GCD of val1 and val2. </returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		public static nt GCD(this nt val1, nt val2)
		{
#if BIGINTEGER
			// If we're working with BigIntegers, there's a library routine for this
			return BigInteger.GreatestCommonDivisor(BigInteger.Abs(val1), BigInteger.Abs(val2));
#else
			// Use absolute values
			val1 = Math.Abs(val1);
			val2 = Math.Abs(val2);

			// While our remainder is non-zero
			while (val2 != 0)
			{
				// Do another division
				var r = val1 % val2;
				val1 = val2;
				val2 = r;
			}
			// Return the answer
			return val1;
#endif
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>   The standard Euclidean algorithm for multiple arguments. </summary>
		///
		/// <remarks>   Darrellp, 2/12/2011. </remarks>
		///
		/// <param name="vals"> A variable-length parameters list containing vals. </param>
		///
		/// <returns>   The GCD of all the values passed in. </returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		public static nt GCD(params nt[] vals)
		{
			return vals.Aggregate(GCD);
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>   Least Common Multiple for multiple values. </summary>
		///
		/// <remarks>   Darrellp, 2/12/2011. </remarks>
		///
		/// <param name="vals"> A variable-length parameters list containing vals. </param>
		///
		/// <returns>   The LCM of the passed in values. </returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		public static nt LCM(params nt[] vals)
		{
			return vals.Aggregate(LCM);
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>   Least Common Multiple. </summary>
		///
		/// <remarks>   Darrellp, 2/12/2011. </remarks>
		///
		/// <param name="val1"> The first value. </param>
		/// <param name="val2"> The second value. </param>
		///
		/// <returns>   The LCM of the val1 and val2. </returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		public static nt LCM(this nt val1, nt val2)
		{
			return val1*val2/GCD(val1, val2);
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>   Implements the extended Euclidean algorithm. </summary>
		///
		/// <remarks>   See the EuclideanExt class for the return value from this function.
		/// Darrellp, 2/12/2011. </remarks>
		///
		/// <param name="val1"> The first value. </param>
		/// <param name="val2"> The second value. </param>
		///
		/// <returns>   GCD of val1 and val2 along with the coefficients to linearly combine them 
		/// to yield that GCD. </returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		public static EuclideanExt EuclideanExt(this nt val1, nt val2)
		{
			return new EuclideanExt(val1, val2);
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>   Solve the diophantine equation ax + by = c. </summary>
		///
		/// <remarks>   This equation is only solvable if c divides the GCD of a and b.  In that case,
		/// there are an infinite number of solutions.  This method returns a function which gives a different
		/// solution for any value of it's passed in integral parameter.
		/// Darrellp, 2/12/2011. </remarks>
		///
		/// <param name="a">    The value of a in ax + by = c </param>
		/// <param name="b">    The value of b in ax + by = c. </param>
		/// <param name="c">    The value of c in ax + by = c. </param>
		///
		/// <returns>   Function which returns a different solution for each different passed in integer
		/// or null if no solution exists.</returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		public static Func<nt,nt[]> DiophantineSolve(nt a, nt b, nt c)
		{
			nt gcd;

			return DiophantineSolve(a, b, c, out gcd);
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>   Solve the diophantine equation ax + by = c and return the GCD of a and b</summary>
		///
		/// <remarks>   Identical to DiophantineSolve(nt a, nt b, nt c) except it also returns the GCD which
		/// is required to be produced during the course of the algorithm.  This is a time savings where both the
		/// solution and the GCD are required.
		/// Darrellp, 2/12/2011. </remarks>
		///
		/// <param name="a">    The value of a in ax + by = c </param>
		/// <param name="b">    The value of b in ax + by = c. </param>
		/// <param name="c">    The value of c in ax + by = c. </param>
		/// <param name="gcd">  The returned GCD of a and b </param>
		///
		///<returns>   Function which returns a different solution for each different passed in integer or
		/// null if no solution exists.</returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		public static Func<nt, nt[]> DiophantineSolve(nt a, nt b, nt c, out nt gcd)
		{
			// Do the extended Euclidean algorithm on a and b
			var ext = new EuclideanExt(a, b);
			gcd = ext.GCD;

			// If c doesn't divide the GCD of a and b, there's no hope
			if (c % gcd != 0)
			{
				return null;
			}

			// Get a solution
			var cnst1 = c * ext.Coeff1 / gcd;
			var cnst2 = c * ext.Coeff2 / gcd;
			var cf1 = b / gcd;
			var cf2 = -a / gcd;
			nt q;

			// See which coefficient is largest and try to lower it maximally

			// If it's the first coefficient
			if (TypeAdaptation.Abs(cnst1) > TypeAdaptation.Abs(cnst2))
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
			return i => new[] { cf1 * i + cnst1, cf2 * i + cnst2 };
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>   Inversion mod m. </summary>
		///
		/// <remarks>   Inverts n mod a particular value.
		/// Darrellp, 2/12/2011. </remarks>
		///
		/// <param name="n">    The number to be inverted. </param>
		/// <param name="mod">  The modulus. </param>
		///
		/// <returns>  1/n Mod mod or null if there's no solution </returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		public static nt InverseMod(this nt n, nt mod)
		{
			var solns = LinearCongruenceSolve(n, 1, mod);
			if (solns == null)
			{
				return -1;
			}
			return solns[0];
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>  Solves Mod(a*x, mod) == b for x </summary>
		///
		/// <remarks>   Darrellp, 2/12/2011. </remarks>
		///
		/// <param name="a">    The value of a in Mod(a*x, mod) == b. </param>
		/// <param name="b">    The value of b in Mod(a*x, mod) == b. </param>
		/// <param name="mod">  The modulus. </param>
		///
		/// <returns>   x between 0 and mod-1 such that Mod(a*x, mod) == b or null if there's no solutions. </returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		public static nt[] LinearCongruenceSolve(nt a, nt b, nt mod)
		{
			nt gcd;
			var fnSolns = DiophantineSolve(a, mod, b, out gcd);
			if (fnSolns == null || gcd > int.MaxValue)
			{
				return null;
			}

			var ret = new nt[(int)gcd];
			for (var i = 0; i < gcd; i++)
			{
				var sln = fnSolns(i);
				ret[i] = sln[0];
				if (ret[i] > mod)
				{
					ret[i] -= (ret[i]/mod)*mod;
				}
				else if (ret[i] < 0)
				{
					ret[i] += ((mod - 1 - ret[i])/mod)*mod;
				}
			}

			return ret;
		}
	}
}
