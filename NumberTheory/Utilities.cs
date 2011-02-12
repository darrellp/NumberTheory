using System.Linq;
#if BIGINTEGER
using System;
using System.Numerics;
using nt = System.Numerics.BigInteger;
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
    internal static class Utilities
    {
        internal static int TwosExponent(this nt n, out nt m)
        {
            var exp = 0;
            m = n;
            while ((m & 1) == 0)
            {
                exp++;
                m >>= 1;
            }
            return exp;
        }

        internal static int NegOnePower(this nt n)
        {
            return ((n & 1) == 0) ? 1 : -1;
        }

        internal static int NegOnePower(this int n)
        {
            return ((n & 1) == 0) ? 1 : -1;
        }
    }
}
