﻿using System;
using System.Collections.Generic;
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
// ReSharper restore CheckNamespace
{
	/// <summary>
	/// Represents a prime power in a factorization
	/// </summary>
	public class PrimeFactor
	{
		/// <summary>
		/// The prime in this factor
		/// </summary>
		public nt Prime { get; private set; }

		/// <summary>
		/// Exponent for the prime
		/// </summary>
		public nt Exp { get; private set; }

		/// <summary>
		/// Create a prime factor for a factorization
		/// </summary>
		/// <param name="prime">The prime factor</param>
		/// <param name="exp">It's exponent</param>
		public PrimeFactor(nt prime, nt exp)
		{
			Prime = prime;
			Exp = exp;
		}
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>   Static class to hold functions related to factoring. </summary>
	///
	/// <remarks>   Darrellp, 2/12/2011. </remarks>
	////////////////////////////////////////////////////////////////////////////////////////////////////

	public static class Factoring
	{
		static Random _rnd = new Random();
		private const int MaxGathers = 10;

		class PSeq
		{
			private nt X { get; set; }
			private nt Y { get; set; }

			private nt Diff
			{
				get { return TypeAdaptation.Abs(X - Y); }
			}
			private nt _n;

			public PSeq(nt n)
			{
				Init(n);
			}

			public PSeq(nt n, long seed)
			{
				_rnd = new Random((int)seed);
				Init(n);
			}

			private void Init(nt n)
			{
				_n = n;
				X = (nt) (_rnd.NextDouble() * Int64.MaxValue);
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

			public List<nt> DiffsBlock()
			{
				var ret = new List<nt>(MaxGathers);
				for (var i = 0; i < MaxGathers; i++)
				{
					ret.Add(Diff);
					Advance();
				}
				return ret;
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
				var diffsBlock = pseq.DiffsBlock();

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

				if (fact == n)
				{
					// If we still couldn't manage, try with a different seed
					return PollardRho(n, seed == -1 ? -1 : seed + 1, cIters);
				}

				// If we've got a factor
				if (fact != 1 & fact != n)
				{
					// return it
					return fact;
				}
			}

			// Sadly, we never found a factor so return -1
			return -1;
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	Pollard Rho safe factoring. </summary>
		///
		/// <remarks>	
		/// Same as Pollard Rho but checks if n is prime before applying Pollard Rho which isn't happy
		/// when it gets primes passed into it.
		/// 
		/// Darrellp, 3/11/2014. 
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

		public static nt PollardRhoSafe(nt n, long seed = -1, int cIters = 10000)
		{
			return n.IsPrime() ? n : PollardRho(n, seed, cIters);
		}

		private static void IncDict(nt val, Dictionary<nt, int> dict)
		{
			if (dict.ContainsKey(val))
			{
				dict[val]++;
			}
			else
			{
				dict[val] = 1;
			}
		}

		/// <summary>
		/// Do a full factorization for a number
		/// </summary>
		/// <param name="n">Number to be factored</param>
		/// <param name="seed">Seed to use for Pollard Rho factoring algorithm</param>
		/// <param name="cIters">Iterations to use in Pollard Rho</param>
		/// <returns>A list of prime factors</returns>
		/// <exception cref="ArgumentException">Thrown if we can't factor a value during the algorithm</exception>
		public static List<PrimeFactor> Factor(nt n, long seed = -1, int cIters = 10000)
		{
			var ret = new List<PrimeFactor>();

			if (n == 1 || n.IsPrime())
			{
				ret.Add(new PrimeFactor(n, 1));
				return ret;
			}

			foreach (var prime in Primes.SmallPrimes)
			{
				var exp = 0;
				while (n % prime == 0)
				{
					exp++;
					n /= prime;
				}
				if (exp == 0)
				{
					continue;
				}
				ret.Add(new PrimeFactor(prime, exp));
				if (n == 1)
				{
					return ret;
				}
			}

			var factors = new Dictionary<nt, int>();
			var trialFactors = new Stack<nt>();
			trialFactors.Push(n);
			while (true)
			{
				if (trialFactors.Count == 0)
				{
					break;
				}
				n = trialFactors.Pop();
				if (n.IsPrime())
				{
					IncDict(n, factors);
					continue;
				}

				var factor = PollardRho(n, seed, cIters);

				if (factor == n || factor == -1)
				{
					throw new ArgumentException(string.Format("Unable to factor {0}", n));
				}

				trialFactors.Push(factor);
				trialFactors.Push(n / factor);
			}

			ret.AddRange(factors.Select(primeExp => new PrimeFactor(primeExp.Key, primeExp.Value)));

			return ret;
		}
	}
}
