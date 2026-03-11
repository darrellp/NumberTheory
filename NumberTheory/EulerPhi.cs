using System.Numerics;

namespace NumberTheory;

/// <summary>
/// Class to determine Euler's Phi function
/// </summary>
static public class EulerPhi
{
	/// <summary>
	/// Calculate Euler's Phi function
	/// </summary>
	/// <param name="n">Value to find Phi for</param>
	/// <param name="seed">Seed to use for Pollard Rho factoring algorithm</param>
	/// <param name="cIters">Iterations to use in Pollard Rho</param>
	/// <returns>Phi(n)</returns>
	static public T Phi<T>(T n, long seed = -1, int cIters = 10000) where T : IBinaryInteger<T>
	{
		if (n == T.One)
		{
			return T.One;
		}

		var factorization = Factoring.Factor(n, seed, cIters);
		T ret = n;

		foreach (var primeFactor in factorization)
		{
			ret /= primeFactor.Prime;
			ret *= primeFactor.Prime - T.One;
		}
		return ret;
	}
}
