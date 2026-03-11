using System;
using System.Linq;
using System.Numerics;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace NumberTheory;

/// <summary>   Static class to hold methods pertaining to Lucas sequences. </summary>
public static class Lucas
{
	/// <summary>   Determine the n'th and n+1'st pair of U values in the Lucas sequence. </summary>
	/// <param name="p">    P value for Lucas Sequence </param>
	/// <param name="q">    Q value for Lucas Sequence. </param>
	/// <param name="mod">  The modulus. </param>
	/// <param name="n">    The index of the term we're searching for (zero based). </param>
	/// <returns>   A tuple with the n'th U value as the first element and the n+1's element as the second </returns>
	public static Tuple<T, T> LucasUPair<T>(int p, int q, T mod, T n) where T : IBinaryInteger<T>
	{
		var pT = T.CreateChecked(p);
		var qT = T.CreateChecked(q);
		var two = T.One + T.One;

		// Find mask to match top bit of n
		var mask = n.TopBitMask();

		// Set up initial values of the U pair
		T uk = T.Zero;
		T ukp1 = T.One;

		// while the mask is non-zero
		while (!T.IsZero(mask))
		{
			// Locals
			T ukNew;
			T ukp1New;
			var udblkp1 = (ukp1 * ukp1 - qT * uk * uk).Normalize(mod);

			// Is the masked bit 1?
			if ((n & mask) != T.Zero)
			{
				// Calculate the 2k+1 and 2k+2 values of U
				ukNew = udblkp1;
				ukp1New = (ukp1 * (pT * ukp1 - two * qT * uk)).Normalize(mod);
			}
			else
			{
				// Calculate the 2k and 2k+1 values of U
				ukNew = (uk * (two * ukp1 - pT * uk)).Normalize(mod);
				ukp1New = udblkp1;
			}

			// Update u pair
			uk = ukNew;
			ukp1 = ukp1New;

			// shift the mask right one bit
			mask >>= 1;
		}

		// return the pair of final U values
		return new Tuple<T, T>(uk, ukp1);
	}

	/// <summary>   Determine the n'th value in the U Lucas sequence </summary>
	/// <param name="p">    P value for Lucas Sequence. </param>
	/// <param name="q">    Q value for Lucas Sequence. </param>
	/// <param name="mod">  The modulus. </param>
	/// <param name="n">    The index of the term we're searching for (zero based). </param>
	/// <returns>   The n'th value of the U sequence </returns>
	public static T LucasU<T>(int p, int q, T mod, T n) where T : IBinaryInteger<T>
	{
		return LucasUPair(p, q, mod, n).Item1;
	}

	/// <summary>   Determine the n'th value in the V Lucas sequence </summary>
	/// <param name="p">    P value for Lucas Sequence. </param>
	/// <param name="q">    Q value for Lucas Sequence. </param>
	/// <param name="mod">  The modulus. </param>
	/// <param name="n">    The index of the term we're searching for (zero based). </param>
	/// <returns>   The n'th value of the V sequence </returns>
	public static T LucasV<T>(int p, int q, T mod, T n) where T : IBinaryInteger<T>
	{
		var pT = T.CreateChecked(p);
		var two = T.One + T.One;
		var pair = LucasUPair(p, q, mod, n);
		var v = ((two * pair.Item2) % mod) - ((pT * pair.Item1) % mod);
		if (v < T.Zero)
		{
			v += mod;
		}
		return v;
	}

	/// <summary>   Returns the n'th value of both the U and V Lucas sequences. </summary>
	/// <param name="p">    P value for Lucas Sequence. </param>
	/// <param name="q">    Q value for Lucas Sequence. </param>
	/// <param name="mod">  The modulus. </param>
	/// <param name="n">    The index of the term we're searching for (zero based). </param>
	/// <returns>   A tuple with the U value as the first member and the V value as the second. </returns>
	public static Tuple<T, T> LucasBoth<T>(int p, int q, T mod, T n) where T : IBinaryInteger<T>
	{
		var pT = T.CreateChecked(p);
		var two = T.One + T.One;
		var pair = LucasUPair(p, q, mod, n);
		return new Tuple<T, T>(pair.Item1, ((two * pair.Item2) % mod) - ((pT * pair.Item1) % mod));
	}

	/// <summary>	Implements the Lucas Pseudoprime test. </summary>
	/// <param name="p">	P value for Lucas Sequence. </param>
	/// <param name="q">	Q value for Lucas Sequence. </param>
	/// <param name="n">	The value to be tested for primality. </param>
	/// <returns>	true if n appears to be a prime, false if it's definitely composite. </returns>
	public static bool LucasPsuedoprimeTest<T>(int p, int q, T n) where T : IBinaryInteger<T>
	{
		// If trivially false
		if (!LucasPrep(p, q, n, out var d))
		{
			return false;
		}

		// Check the Lucas U value for n - (d/n)
		return T.IsZero(LucasU(p, q, n, n - T.CreateChecked(Quadratic.Jacobi(T.CreateChecked(d), n))));
	}

	/// <summary>	Identical to the p-q test above, but checks the V value as well as the U. </summary>
	/// <param name="n">	The value to be tested. </param>
	/// <returns>	true if n displays prime behavior. </returns>
	public static bool LucasPsuedoprimeTest<T>(T n) where T : IBinaryInteger<T>
	{
		var two = T.One + T.One;
		if ((n & T.One) == T.Zero)
		{
			return n == two;
		}

		// If trivially false
		if (!GetLucasParameters(n, out var p, out var q))
		{
			return false;
		}

		// Retrieve the Lucas U and V values
		var tupLucas = LucasBoth(p, q, n, n + T.One);

		// Check them for n - (d/n)
		return T.IsZero(tupLucas.Item1) && tupLucas.Item2 == (two * T.CreateChecked(q)).Normalize(n);
	}

	private static bool LucasPrep<T>(int p, int q, T n, out int d) where T : IBinaryInteger<T>
	{
		// Initialize
		d = p * p - 4 * q;
		var g = int.CreateChecked(n.GCD(T.CreateChecked(2 * q * d)));

		// If the test conditions aren't met
		if (T.CreateChecked(g) == n)
		{
			// Throw an ArgumentException
			throw new ArgumentException("Invalid p, q, n in LucasPsuedoPrimeTest");
		}

		// If the test is trivially false
		return g <= 1 || T.CreateChecked(g) >= n;
	}

	static bool GetLucasParameters<T>(T n, out int p, out int q) where T : IBinaryInteger<T>
	{
		var d = Enumerable.Range(0, 1000)
			.Select(i => ((i & 1) == 0 ? 5 + 2 * i : -5 - 2 * i))
			.FirstOrDefault(i => Quadratic.Jacobi(T.CreateChecked(i), n) == -1);
		p = 1;
		q = (1 - d) / 4;
		return d != 0;
	}
}
