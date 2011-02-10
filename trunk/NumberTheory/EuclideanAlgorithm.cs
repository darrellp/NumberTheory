#if BIGINTEGER
using System.Numerics;
using nt=System.Numerics.BigInteger;
#elif LONG
using nt = System.Int64;
#endif


namespace NumberTheory
{
    public static class Euclidean
    {
        public static nt GCD(this nt val1, nt val2)
        {
#if BIGINTEGER
            return BigInteger.GreatestCommonDivisor(val1, val2);
#endif
            while (val2 != 0)
            {
                var r = val1 % val2;
                val1 = val2;
                val2 = r;
            }
            return val1;
        }

        public static EuclideanExt EuclideanExt(this nt val1, nt val2)
        {
            return new EuclideanExt(val1, val2);
        }
    }
}
