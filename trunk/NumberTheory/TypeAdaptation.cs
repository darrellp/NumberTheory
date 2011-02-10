using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if BIGINTEGER
using System.Numerics;
using nt=System.Numerics.BigInteger;
#elif LONG
using nt = System.Int64;
#endif

#if BIGINTEGER
namespace NumberTheoryBig
#elif LONG
namespace NumberTheoryLong
#endif
{
    public static class TypeAdaptation
    {
#if BIGINTEGER
        public static BigInteger DivRem(BigInteger n1, BigInteger n2, out BigInteger rem)
        {
            return BigInteger.DivRem(n1, n2, out rem);
        }

        public static BigInteger Abs(BigInteger n)
        {
            return BigInteger.Abs(n);
        }
#else
        public static nt DivRem(nt n1, nt n2, out nt rem)
        {
            return Math.DivRem(n1, n2, out rem);
        }

        public static nt Abs(nt n)
        {
            return Math.Abs(n);
        }
#endif
    }
}
