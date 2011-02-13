using System.Linq;
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
        // First 200 small primes
        private static readonly nt[] SmallPrimes =
            new nt[]
            {
                2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61,
                67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127, 131, 137,
                139, 149, 151, 157, 163, 167, 173, 179, 181, 191, 193, 197, 199, 211,
                223, 227, 229, 233, 239, 241, 251, 257, 263, 269, 271, 277, 281, 283,
                293, 307, 311, 313, 317, 331, 337, 347, 349, 353, 359, 367, 373, 379,
                383, 389, 397, 401, 409, 419, 421, 431, 433, 439, 443, 449, 457, 461,
                463, 467, 479, 487, 491, 499, 503, 509, 521, 523, 541
            };

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Prime by division. </summary>
        ///
        /// <remarks>   Determines if d might be composite by dividing it by several small primes
        /// Darrellp, 2/13/2011. </remarks>
        ///
        /// <param name="n">    Value to be tested. </param>
        ///
        /// <returns>   true implies n is composite, false if the test is indeterminate. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static bool CompositeByDivision(this nt n)
        {
            return SmallPrimes.Where(p => n != p && n % p == 0).Any();
        }

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

        public static bool PsuedoPrimeTest(nt p, nt b)
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

        public static bool StrongPsuedoPrimeTest(nt n, nt b)
        {
            if (n <= 0)
            {
                throw new ArgumentException("Negative value in StrongPsuedoPrimeTest");
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

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Query if 'n' is prime. </summary>
        ///
        /// <remarks>   This is my approximate guess from what I can glean about how the Mathematica PrimeQ
        /// function works.  I'm pretty sure on the the 2 and 3 psuedoPrime case.  See the remarks at
        /// <see cref="Lucas.LucasPsuedoprimeTest(nt)">LucasPsuedoPrimeTest</see> for confusion regarding that test.
        /// Darrellp, 2/13/2011. </remarks>
        ///
        /// <param name="n">    Value to be tested. </param>
        ///
        /// <returns>   true if prime, false if not. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static bool IsPrime(this nt n)
        {
            return !CompositeByDivision(n) &&
                   StrongPsuedoPrimeTest(n, 2) &&
                   StrongPsuedoPrimeTest(n, 3) &&
                   Lucas.LucasPsuedoprimeTest(n);
        }
    }
}
