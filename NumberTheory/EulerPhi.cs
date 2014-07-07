#if BIGINTEGER
using nt = System.Numerics.BigInteger;
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
		static public nt Phi(nt n, long seed = -1, int cIters = 10000)
		{
			if (n == 1)
			{
				return 1;
			}

			var factorization = Factoring.Factor(n, seed, cIters);
			nt ret = 1;

			// ReSharper disable once LoopCanBeConvertedToQuery
			foreach (var primeFactor in factorization)
			{
				// We're save doing it mod n here since phi is always less than n
				ret *= PowerMod.Power(primeFactor.Prime, primeFactor.Exp - 1, n) * (primeFactor.Prime - 1);
			}
			return ret;
		}
	}
}
