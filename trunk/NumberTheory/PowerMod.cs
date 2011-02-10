#if BIGINTEGER
using System.Numerics;
using nt = System.Numerics.BigInteger;
#elif LONG
using nt = System.Int64;
#endif

#if BIGINTEGER
namespace NumberTheoryBig
#elif LONG
namespace NumberTheoryLong
#endif
{
    static public class PowerMod
    {
        static public nt Power(nt x, nt n, nt mod)
        {
#if BIGINTEGER
            // TODO: Doesn't handle negative powers correctly
            return BigInteger.ModPow(x, n, mod);
#else
            if (n == 0)
            {
                return 1;
            }

            nt mask;
            nt res = 1;

            for (mask = (~nt.MaxValue >> 1) & nt.MaxValue; (mask & n)== 0; mask >>= 1) ;

            while ((mask & n) == 0)
            {
                mask >>= 1;
            }
            while (mask != 0)
            {
                res *= res;
                if ((mask & n) != 0)
                {
                    res *= x;
                }
                res = res%mod;
                mask >>= 1;
            }
            return res;
#endif
        }
    }
}
