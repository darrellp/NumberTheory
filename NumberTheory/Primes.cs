#if BIGINTEGER
using System;
using nt=System.Numerics.BigInteger;
#elif LONG
using System;
using nt = System.Int64;
#endif


#if BIGINTEGER
namespace NumberTheoryBig
#elif LONG
namespace NumberTheoryLong
#endif
{
    public static class Primes
    {
        public static bool PsuedoPrime(nt p, nt b)
        {
            return PowerMod.Power(b, p - 1, p) == 1;
        }

        public static bool StrongPsuedoPrime(nt n, nt b)
        {
            if (n < 0)
            {
                throw new ArgumentException("Negative value in StrongPsuedoPrime");
            }
            if ((n & 1) == 0)
            {
                return n == 2;
            }

            nt nReduced;
            var s = (n - 1).TwosExponent(out nReduced);
            var bp = PowerMod.Power(b, nReduced, n);
            if (bp == 1)
            {
                return true;
            }

            for (var i = 0; i < s; i++)
            {
                if (bp == 1)
                {
                    return false;
                }
                if (bp == n - 1)
                {
                    return true;
                }
                bp = PowerMod.Power(bp, 2, n);
            }
            return false;
        }
    }
}
