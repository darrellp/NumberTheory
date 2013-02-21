#if BIGINTEGER
using System;
using System.Collections.Generic;
using System.Linq;
using nt=System.Numerics.BigInteger;
#elif LONG
using System;
using System.Collections.Generic;
using System.Linq;
using nt = System.Int64;
#endif


#if BIGINTEGER
namespace NumberTheoryBig
#elif LONG
namespace NumberTheoryLong
#endif
{
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>   Static class to hold functions related to factoring. </summary>
	///
	/// <remarks>   Darrellp, 2/12/2011. </remarks>
	////////////////////////////////////////////////////////////////////////////////////////////////////

	public static class Factoring
	{
		static readonly Random Rnd = new Random();
		private const int MaxGathers = 10;

		class PSeq
		{
			private nt X { get; set; }
			private nt Y { get; set; }

			private nt Diff
			{
				get { return TypeAdaptation.Abs(X - Y); }
			}
			private readonly nt _n;

			public PSeq(nt n)
			{
				X = (nt)(Rnd.NextDouble() * Int64.MaxValue);
				Y = F(X);
				_n = n;
			}

			public PSeq(nt n, long seed)
			{
				_n = n;
				X = seed;
				Y = F(X);
			}

			private void Advance()
			{
#if BIGINTEGER
				X = F(X) % _n;
				Y = F(F(Y)) % _n;
#elif LONG
				X = F(X);
				Y = F(F(Y));
#endif
			}

			nt F(nt x)
			{
#if BIGINTEGER
				return x * x + 1;
#elif LONG
				// Possible overflow here keeps us from allowing numbers that won't fit
				// in an Int32 for the Long version of Pollard-Rho.
				return (x * x + 1) % _n;
#endif
			}

			public List<nt> DiffsBlock
			{
				get
				{
					var ret = new List<nt>();
					for (var i = 0; i < MaxGathers; i++)
					{
						ret.Add(Diff);
						Advance();
					}
					return ret;
				}
			}
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Pollard Rho factoring. </summary>
		///
		/// <remarks>	
		/// This follows the algorithm given in Computational Number Theory by Stan Wagon and David
		/// Bressoud.  It utilizes the optimization suggested there by gathering MaxGathers values at a
		/// time and doing the GCD on that product and n rather than performing a GCD at every step.  It
		/// may fail in one of two ways - it might not find a factor in the given number of iterations,
		/// in which case it returns -1 or it may happen that all the factors are present in the cycle of
		/// the rho, in in which case it returns n.  The former happens most commonly on numbers too
		/// large and the latter on numbers too small.  It's best to verify that n is not prime using a
		/// prime test before calling this routine since it will just cycle the max number of iters on
		/// primes.
		/// 
		/// Darrellp, 2/12/2011. 
		/// </remarks>
		///
		/// <param name="n">		The value to be factored. </param>
		/// <param name="seed">		The seed to be used for random number generation.  Defaults to using
		/// 						random seed. </param>
		/// <param name="cIters">	The count of iterations before giving up. </param>
		///
		/// <returns>	
		/// -1 if the algorithm failed to find a factor in cIters iterations.  n if the factors couldn't
		/// be separated.  Any other value is a bonafide non-trivial factor. 
		/// </returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		public static nt PollardRho(nt n, long seed = -1, int cIters = 10000)
		{
#if LONG
			if (n > int.MaxValue)
			{
				throw new ArgumentException("n can't exceed int.MaxValue in long version of PollardRho - try BigInteger version");
			}
#endif
			// Get the list of differences
			var pseq = seed == -1 ? new PSeq(n) : new PSeq(n, seed);

			// For each block of differences
			for (var i = 0; i < cIters; i += MaxGathers)
			{
				// Get the next block
				var diffsBlock = pseq.DiffsBlock;

				// Get the GCD of n and the product of differences within the block
				var fact = diffsBlock.Aggregate((a, v) => (a * v) % n).GCD(n);

				// Is the GCD equal to n?
				//
				// If the GCD is n, we may still have hope by backing up and trying the GCD's one at a time
				if (fact == n)
				{
					// Try doing GCD's on individual differences within the block
					fact = diffsBlock.Select(d => n.GCD(d)).First(d => d != 1);
				}
				
				// If we've got a factor
				if (fact != 1)
				{
					// return it
					return fact;
				}
			}

			// Sadly, we never found a factor so return -1
			return -1;
		}
	}
}
