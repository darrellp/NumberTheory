using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace NumberTheory;

/// <summary>
/// Represents a prime power in a factorization
/// </summary>
public class PrimeFactor<T> where T : IBinaryInteger<T>
{
	/// <summary>
	/// The prime in this factor
	/// </summary>
	public T Prime { get; private set; }

	/// <summary>
	/// Exponent for the prime
	/// </summary>
	public T Exp { get; private set; }

	/// <summary>
	/// Create a prime factor for a factorization
	/// </summary>
	/// <param name="prime">The prime factor</param>
	/// <param name="exp">It's exponent</param>
	public PrimeFactor(T prime, T exp)
	{
		Prime = prime;
		Exp = exp;
	}
}

/// <summary>   Static class to hold functions related to factoring. </summary>
public static class Factoring
{
	static Random _rnd = new Random();
	private const int MaxGathers = 10;

	class PSeq<T> where T : IBinaryInteger<T>
	{
		private T X { get; set; }
		private T Y { get; set; }

		private T Diff
		{
			get { return T.Abs(X - Y); }
		}
		private T _n;

		public PSeq(T n)
		{
			Init(n);
		}

		public PSeq(T n, long seed)
		{
			_rnd = new Random((int)seed);
			Init(n);
		}

		private void Init(T n)
		{
			_n = n;
			X = T.CreateChecked((long)(_rnd.NextDouble() * Int64.MaxValue));
			Y = F(X);
		}

		private void Advance()
		{
			X = F(X) % _n;
			Y = F(F(Y)) % _n;
		}

		T F(T x)
		{
			return (x * x + T.One) % _n;
		}

		public List<T> DiffsBlock()
		{
			var ret = new List<T>(MaxGathers);
			for (var i = 0; i < MaxGathers; i++)
			{
				ret.Add(Diff);
				Advance();
			}
			return ret;
		}
	}

	/// <summary>	Pollard Rho factoring. </summary>
	/// <remarks>
	/// This follows the algorithm given in Computational Number Theory by Stan Wagon and David
	/// Bressoud.
	/// </remarks>
	/// <param name="n">		The value to be factored. </param>
	/// <param name="seed">		The seed to be used for random number generation. </param>
	/// <param name="cIters">	The count of iterations before giving up. </param>
	/// <returns>
	/// -1 if the algorithm failed to find a factor in cIters iterations. n if the factors couldn't
	/// be separated. Any other value is a bonafide non-trivial factor.
	/// </returns>
	public static T PollardRho<T>(T n, long seed = -1, int cIters = 10000) where T : IBinaryInteger<T>
	{
		// Get the list of differences
		var pseq = seed == -1 ? new PSeq<T>(n) : new PSeq<T>(n, seed);

		// For each block of differences
		for (var i = 0; i < cIters; i += MaxGathers)
		{
			// Get the next block
			var diffsBlock = pseq.DiffsBlock();

			// Get the GCD of n and the product of differences within the block
			var fact = diffsBlock.Aggregate((a, v) => (a * v) % n).GCD(n);

			// Is the GCD equal to n?
			if (fact == n)
			{
				// Try doing GCD's on individual differences within the block
				fact = diffsBlock.Select(d => n.GCD(d)).First(d => d != T.One);
			}

			if (fact == n)
			{
				// If we still couldn't manage, try with a different seed
				return PollardRho(n, seed == -1 ? -1 : seed + 1, cIters);
			}

			// If we've got a factor
			if (fact != T.One & fact != n)
			{
				// return it
				return fact;
			}
		}

		// Sadly, we never found a factor so return -1
		return -T.One;
	}

	/// <summary>	Pollard Rho safe factoring. </summary>
	/// <remarks>
	/// Same as Pollard Rho but checks if n is prime before applying Pollard Rho.
	/// </remarks>
	/// <param name="n">		The value to be factored. </param>
	/// <param name="seed">		The seed. </param>
	/// <param name="cIters">	The count of iterations before giving up. </param>
	/// <returns>
	/// -1 if the algorithm failed, n if prime or factors couldn't be separated,
	/// otherwise a non-trivial factor.
	/// </returns>
	public static T PollardRhoSafe<T>(T n, long seed = -1, int cIters = 10000) where T : IBinaryInteger<T>
	{
		return n.IsPrime() ? n : PollardRho(n, seed, cIters);
	}

	private static void IncDict<T>(T val, Dictionary<T, int> dict) where T : IBinaryInteger<T>
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
	public static List<PrimeFactor<T>> Factor<T>(T n, long seed = -1, int cIters = 10000) where T : IBinaryInteger<T>
	{
		var ret = new List<PrimeFactor<T>>();

		if (n == T.One || n.IsPrime())
		{
			ret.Add(new PrimeFactor<T>(n, T.One));
			return ret;
		}

		foreach (var prime in Primes.SmallPrimes)
		{
			var primeT = T.CreateChecked(prime);
			var exp = 0;
			while (n % primeT == T.Zero)
			{
				exp++;
				n /= primeT;
			}
			if (exp == 0)
			{
				continue;
			}
			ret.Add(new PrimeFactor<T>(primeT, T.CreateChecked(exp)));
			if (n == T.One)
			{
				return ret;
			}
		}

		var factors = new Dictionary<T, int>();
		var trialFactors = new Stack<T>();
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

			if (factor == n || factor == -T.One)
			{
				throw new ArgumentException(string.Format("Unable to factor {0}", n));
			}

			trialFactors.Push(factor);
			trialFactors.Push(n / factor);
		}

		ret.AddRange(factors.Select(primeExp => new PrimeFactor<T>(primeExp.Key, T.CreateChecked(primeExp.Value))));

		return ret;
	}
}
