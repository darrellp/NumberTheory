#if BIGINTEGER
using System;
using System.Numerics;
using nt=System.Numerics.BigInteger;
#elif LONG
using System;
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
#else
            while (val2 != 0)
            {
                var r = val1 % val2;
                val1 = val2;
                val2 = r;
            }
            return val1;
#endif
        }

        public static EuclideanExt EuclideanExt(this nt val1, nt val2)
        {
            return new EuclideanExt(val1, val2);
        }

        public static Func<nt,nt[]> DiophantineSolve(nt a, nt b, nt c)
        {
            var ext = new EuclideanExt(a,b);

            if (c % ext.GCD != 0)
            {
                return null;
            }

            var cnst1 = c * ext.Coeff1 / ext.GCD;
            var cnst2 = c * ext.Coeff2 / ext.GCD;
            var cf1 = b / ext.GCD;
            var cf2 = -a / ext.GCD;
            nt q;

            if (TypeAdaptation.Abs(cnst1) > TypeAdaptation.Abs(cnst2))
            {
                q = cnst1/cf1;
            }
            else
            {
                q = cnst2/cf2;
            }
            cnst1 -= q*cf1;
            cnst2 -= q*cf2;
            return i => new[] {cf1 * i + cnst1, cf2 * i + cnst2};
        }
    }
}
