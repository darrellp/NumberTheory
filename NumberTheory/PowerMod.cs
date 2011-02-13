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
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   PowerMod code for the number theory library. </summary>
    ///
    /// <remarks>   Darrellp, 2/12/2011. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    static public class PowerMod
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Returns x^n Mod mod. </summary>
        ///
        /// <remarks>   The long version of this algorithm is modelled after the algorithm given in 
        /// Computational Number Theory by Wagon and Bressoud.  The BigInteger version uses the CLR supplied
        /// function to do this.  Unfortunately, it doesn't handle negative exponents so I have to manhandle
        /// the return value if the exponent is negative.  This all happens transparently in the algorithm.
        /// Darrellp, 2/12/2011. </remarks>
        ///
        /// <param name="x">    Value to be exponentiated. </param>
        /// <param name="n">    The exponent. </param>
        /// <param name="mod">  The modulus. </param>
        ///
        /// <returns>   x^n Mod mod </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        static public nt Power(nt x, nt n, nt mod)
        {
#if BIGINTEGER
            var nsol = BigInteger.ModPow(x, BigInteger.Abs(n), mod);
            if (n < 0)
            {
                nsol = nsol.InverseMod(mod);
            }
            return nsol;
#else
            if (n == 0)
            {
                return 1;
            }

            nt mask;
            nt res = 1;

            for (mask = (~nt.MaxValue >> 1) & nt.MaxValue; (mask & n)== 0; mask >>= 1)
            {
            }

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
