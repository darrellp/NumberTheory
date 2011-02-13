#if BIGINTEGER
using System;
using nt=System.Numerics.BigInteger;
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
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Static class for functions dealing with prime numbers. </summary>
    ///
    /// <remarks>   Darrellp, 2/12/2011. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public static class Primes
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Simple psuedoprime algorithm to determine if p passes the b-psuedoprime test. </summary>
        ///
        /// <remarks>   Taken from Computational Number Theory by Wagon and Bressoud
        /// Darrellp, 2/12/2011. </remarks>
        ///
        /// <param name="p">    The proposed prime we're testing. </param>
        /// <param name="b">    The exponent we're testing against. </param>
        ///
        /// <returns>   true p is exhibiting prime like behavior, false if p is composite. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static bool PsuedoPrime(nt p, nt b)
        {
            return PowerMod.Power(b, p - 1, p) == 1;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Determine if p passes the b-strong psuedoprime test. </summary>
        ///
        /// <remarks>   Taken from Computational Number Theory by Wagon and Bressoud
        /// Darrellp, 2/12/2011. </remarks>
        ///
        /// <exception cref="ArgumentException">    Thrown when n is less than or equal to 0. </exception>
        ///
        /// <param name="n">    The. </param>
        /// <param name="b">    The exponent we're testing against. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static bool StrongPsuedoPrime(nt n, nt b)
        {
            if (n <= 0)
            {
                throw new ArgumentException("Negative value in StrongPsuedoPrime");
            }
            if ((n & 1) == 0)
            {
                return n == 2;
            }

            nt nReduced;
            var s = (n - 1).TwosExponent(out nReduced);
            var bp = PowerMod.Power(b, nReduced, n);
            if (bp == 1)
            {
                return true;
            }

            for (var i = 0; i < s; i++)
            {
                if (bp == 1)
                {
                    return false;
                }
                if (bp == n - 1)
                {
                    return true;
                }
                bp = PowerMod.Power(bp, 2, n);
            }
            return false;
        }
    }
}
