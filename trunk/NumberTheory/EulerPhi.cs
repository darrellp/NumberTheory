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
{
	static public class EulerPhi
	{
		static public nt Phi(nt n, long seed = -1, int cIters = 10000)
		{
			if (n == 1)
			{
				return 1;
			}

			var factorization = Factoring.Factor(n, seed, cIters);
			nt ret = 1;

			foreach (var primeFactor in factorization)
			{
				// We're save doing it mod n here since phi is always less than n
				ret *= PowerMod.Power(primeFactor.Prime, primeFactor.Exp - 1, n) * (primeFactor.Prime - 1);
			}
			return ret;
		}
	}
}
