#if BIGINTEGER
using System;
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

        internal static nt TopBitMask(this nt n)
        {
            // ReSharper disable JoinDeclarationAndInitializer
            nt mask;
            // ReSharper restore JoinDeclarationAndInitializer
            #if BIGINTEGER
            if (n < 0)
            {
                throw new ArgumentException("Negative value in BigInteger version of TopBitMask");
            }
            if (n == 0)
            {
                return 0;
            }
            mask = 1;
            n >>= 1;
            while (n != 0)
            {
                n >>= 1;
                mask <<= 1;
            }
            #else
            if (n == 0)
            {
                return -1;
            }
            mask = (~nt.MaxValue >> 1) & nt.MaxValue;
            for (; (mask & n) == 0; mask >>= 1) { }
            #endif
            return mask;
        }
    }
}
