#if BIGINTEGER
using System.Numerics;
#elif LONG
using System;
using nt = System.Int64;
#endif

// ReSharper disable CheckNamespace
#if BIGINTEGER
namespace NumberTheoryBig
#elif LONG
namespace NumberTheoryLong
#endif
{
    internal static class TypeAdaptation
    {
#if BIGINTEGER
        internal static BigInteger DivRem(BigInteger n1, BigInteger n2, out BigInteger rem)
        {
            return BigInteger.DivRem(n1, n2, out rem);
        }

        internal static BigInteger Abs(BigInteger n)
        {
            return BigInteger.Abs(n);
        }
#else
        internal static nt DivRem(nt n1, nt n2, out nt rem)
        {
            return Math.DivRem(n1, n2, out rem);
        }

        internal static nt Abs(nt n)
        {
            return Math.Abs(n);
        }
#endif
    }
}
