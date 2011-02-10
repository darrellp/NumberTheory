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
            nt gcd;

            return DiophantineSolve(a, b, c, out gcd);
        }

        public static Func<nt, nt[]> DiophantineSolve(nt a, nt b, nt c, out nt gcd)
        {
            var ext = new EuclideanExt(a, b);
            gcd = ext.GCD;

            if (c % gcd != 0)
            {
                return null;
            }

            var cnst1 = c * ext.Coeff1 / gcd;
            var cnst2 = c * ext.Coeff2 / gcd;
            var cf1 = b / gcd;
            var cf2 = -a / gcd;
            nt q;

            if (TypeAdaptation.Abs(cnst1) > TypeAdaptation.Abs(cnst2))
            {
                q = cnst1 / cf1;
            }
            else
            {
                q = cnst2 / cf2;
            }
            cnst1 -= q * cf1;
            cnst2 -= q * cf2;
            return i => new[] { cf1 * i + cnst1, cf2 * i + cnst2 };
        }

        public static nt[] LinearCongruenceSolve(nt a, nt b, nt mod)
        {
            nt gcd;
            var fnSolns = DiophantineSolve(a, mod, b, out gcd);
            if (fnSolns == null || gcd > int.MaxValue)
            {
                return null;
            }

            var ret = new nt[(int)gcd];
            for (var i = 0; i < gcd; i++)
            {
                var sln = fnSolns(i);
                ret[i] = sln[0];
                if (ret[i] > mod)
                {
                    ret[i] -= (ret[i]/mod)*mod;
                }
                else if (ret[i] < 0)
                {
                    ret[i] += ((mod - 1 - ret[i])/mod)*mod;
                }
            }

            return ret;
        }
    }
}
