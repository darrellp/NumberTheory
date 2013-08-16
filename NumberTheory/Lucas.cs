using System;
using System.Linq;
#if BIGINTEGER
using nt=System.Numerics.BigInteger;
#elif LONG
using nt = System.Int64;
#endif

// ReSharper disable CheckNamespace
#if BIGINTEGER
namespace NumberTheoryBig
#elif LONG
namespace NumberTheoryLong
#endif
{
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>   Static class to hold methods pertaining to Lucas sequences. </summary>
	///
	/// <remarks>   Darrellp, 2/13/2011. </remarks>
	////////////////////////////////////////////////////////////////////////////////////////////////////

	public static class Lucas
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>   Determine the n'th and n+1'st pair of U values in the Lucas sequence. </summary>
		///
		/// <remarks>   Taken from Computational Number Theory by Wagon and Bressoud
		/// Darrellp, 2/13/2011. </remarks>
		///
		/// <param name="p">    P value for Lucas Sequence </param>
		/// <param name="q">    Q value for Lucas Sequence. </param>
		/// <param name="mod">  The modulus. </param>
		/// <param name="n">    The index of the term we're searching for (zero based). </param>
		///
		/// <returns>   A tuple with the n'th U value as the first element and the n+1's element as the second </returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		public static Tuple<nt,nt> LucasUPair(int p, int q, nt mod, nt n)
		{
			// Find mask to match top bit of n
			var mask = n.TopBitMask();

			// Set up initial values of the U pair
			nt uk = 0;
			nt ukp1 = 1;

			// while the mask is non-zero
			while (mask != 0)
			{
				// Locals
				nt ukNew;
				nt ukp1New;
				var udblkp1 = (ukp1 * ukp1 - q * uk * uk).Normalize(mod);

				// Is the masked bit 1?
				if ((n & mask) != 0)
				{
					// Calculate the 2k+1 and 2k+2 values of U
					ukNew = udblkp1;
					ukp1New = (ukp1 * (p * ukp1 - 2 * q * uk)).Normalize(mod);
				}
				else
				{
					// Calculate the 2k and 2k+1 values of U
					ukNew = (uk * (2 * ukp1 - p * uk)).Normalize(mod);
					ukp1New = udblkp1;
				}

				// Update u pair
				uk = ukNew;
				ukp1 = ukp1New;

				// shift the mask right one bit
				mask >>= 1;
			}

			// return the pair of final U values
			return new Tuple<nt, nt>(uk, ukp1);
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>   Determine the n'th value in the U Lucas sequence </summary>
		///
		/// <remarks>   Darrellp, 2/13/2011. </remarks>
		///
		/// <param name="p">    P value for Lucas Sequence. </param>
		/// <param name="q">    Q value for Lucas Sequence. </param>
		/// <param name="mod">  The modulus. </param>
		/// <param name="n">    The index of the term we're searching for (zero based). </param>
		///
		/// <returns>   The n'th value of the U sequence </returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		public static nt LucasU(int p, int q, nt mod, nt n)
		{
			return LucasUPair(p, q, mod, n).Item1;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>   Determine the n'th value in the V Lucas sequence </summary>
		///
		/// <remarks>   Darrellp, 2/13/2011. </remarks>
		///
		/// <param name="p">    P value for Lucas Sequence. </param>
		/// <param name="q">    Q value for Lucas Sequence. </param>
		/// <param name="mod">  The modulus. </param>
		/// <param name="n">    The index of the term we're searching for (zero based). </param>
		///
		/// <returns>   The n'th value of the V sequence </returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		public static nt LucasV(int p, int q, nt mod, nt n)
		{
			var pair = LucasUPair(p, q, mod, n);
			var v = ((2 * pair.Item2) % mod) - ((p * pair.Item1) % mod);
			if (v < 0)
			{
				v += mod;
			}
			return v;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>   Returns the n'th value of both the U and V Lucas sequences. </summary>
		///
		/// <remarks>   Darrellp, 2/13/2011. </remarks>
		///
		/// <param name="p">    P value for Lucas Sequence. </param>
		/// <param name="q">    Q value for Lucas Sequence. </param>
		/// <param name="mod">  The modulus. </param>
		/// <param name="n">    The index of the term we're searching for (zero based). </param>
		///
		/// <returns>   A tuple with the U value as the first member and the V value as the second. </returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		public static Tuple<nt, nt> LucasBoth(int p, int q, nt mod, nt n)
		{
			var pair = LucasUPair(p, q, mod, n);
			return new Tuple<nt, nt>(pair.Item1, ((2 * pair.Item2) % mod) - ((p * pair.Item1) % mod));
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Implements the Lucas Psuedoprime test. </summary>
		///
		/// <remarks>	
		/// Taken from Computational Number theory by Wagon and Bressoud.  The implementation in the book
		/// is a bit confusing since it suddenly introduces a test on GCD(n, 2qd) which isn't mentioned
		/// anywhere else in the text as far as I can see.  It isn't in Corollary 8.2.  It is explicitly
		/// mentioned in "Prime Numbers: A Computational Perspective" by Crandall and Pomerance, but no
		/// proof of explanation seems to be given there for it's inclusion in the theorem.  I try to
		/// follow CNT's lead in spite of the fact that I'm not sure I understand it. Also, in CNT, they
		/// just resort to PrimeQ when n divides into 2qd.  I don't have the luxury of resorting to
		/// PrimeQ in that case, so I just make it a condition that 2qd doesn't divide n and throw an
		/// argument exception if it does.  Typically, q and d will be small and n will be large so this
		/// is not a factor, but I'm trying to be complete here. Darrellp, 2/13/2011. 
		/// </remarks>
		///
		/// <param name="p">	P value for Lucas Sequence. </param>
		/// <param name="q">	Q value for Lucas Sequence. </param>
		/// <param name="n">	The value to be tested for primality. </param>
		///
		/// <returns>	true if n appears to be a prime, false if it's definitely composite. </returns>
		///
		/// ### <exception cref="ArgumentException">	Thrown when n divides into 2q(p^2 - 4q) </exception>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		public static bool LucasPsuedoprimeTest(int p, int q, nt n)
		{
			int d;

			// If trivially false
			if (!LucasPrep(p, q, n, out d))
			{
				return false;
			}

			// Check the Lucas U value for n - (d/n)
			return LucasU(p, q, n, n - Quadratic.Jacobi(d, n)) == 0;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Identical to the p-q test above, but checks the V value as well as the U.
		/// This makes it a little more accurate for essentially no performance hit. </summary>
		///
		/// <remarks>	Darrellp, 2/15/2011. </remarks>
		///
		/// <param name="n">	The index of the term we're searching for (zero based). </param>
		///
		/// <returns>	true if n displays prime behavior. </returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		public static bool LucasPsuedoprimeTest(nt n)
		{
			if ((n & 1) == 0)
			{
				return n == 2;
			}

			int d, p, q;

			// If trivially false
			if (!GetLucasParameters(n, out p, out q, out d))
			{
				return false;
			}

			// Retrieve the Lucas U and V values
			var tupLucas = LucasBoth(p, q, n, n + 1);

			// Check them for n - (d/n)
			return tupLucas.Item1 == 0 && tupLucas.Item2 == (2 * (nt)q).Normalize(n);
		}

		private static bool LucasPrep(int p, int q, nt n, out int d)
		{
			// Initialize
			d = p*p - 4*q;
			var g = (int)n.GCD(2*q*d);

			// If the test conditions aren't met
			if (n == g)
			{
				// Throw an ArgumentException
				throw new ArgumentException("Invalid p, q, n in LucasPsuedoPrimeTest");
			}

			// If the test is trivially false
			return g <= 1 || g >= n;
		}

		static bool GetLucasParameters(nt n, out int p, out int q, out int d)
		{
// ReSharper disable RedundantCast
			d = Enumerable.Range(0, 1000)
				.Select(i => ((i & 1) == 0 ? 5 + 2 * i : -5 - 2 * i))
				.FirstOrDefault(i => Quadratic.Jacobi((nt)i, n) == -1);
// ReSharper restore RedundantCast
			p = 1;
			q = (1 - d) / 4;
			return d != 0;
		}
	}
}
