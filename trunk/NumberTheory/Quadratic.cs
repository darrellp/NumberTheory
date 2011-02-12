#if BIGINTEGER
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
    static public class Quadratic
    {
        static public int Jacobi(nt a, nt n)
        {
            if ((n & 1) != 1)
            {
                return 0;
            }

            var gcd = a;
            var nCur = n;
            var c = 1;

            while (true)
            {
                var rCap = gcd % nCur;
                gcd = nCur;

                if (rCap == 0)
                {
                    break;
                }

                nt r;
                var s = rCap.TwosExponent(out r);
                var nCurMod8 = (int) (nCur & 7);
                c *= ((2 * (nCurMod8 - 1) * (r - 1) + s * (nCurMod8 * nCurMod8 - 1)) / 8).NegOnePower();
                nCur = r;
            }
            return gcd != 1 ? 0 : c;
        }
    }
}
