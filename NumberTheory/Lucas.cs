using System;
#if BIGINTEGER
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
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Static class to hold methods pertaining to Lucas sequences. </summary>
    ///
    /// <remarks>   Darrellp, 2/13/2011. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public static class Lucas
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Determine the n'th and n+1'st pair of U values in the Lucas sequence. </summary>
        ///
        /// <remarks>   Taken from Computational Number Theory by Wagon and Bressoud
        /// Darrellp, 2/13/2011. </remarks>
        ///
        /// <param name="p">    P value for Lucas Sequence </param>
        /// <param name="q">    Q value for Lucas Sequence. </param>
        /// <param name="mod">  The modulus. </param>
        /// <param name="n">    The index of the term we're searching for (zero based). </param>
        ///
        /// <returns>   A tuple with the n'th U value as the first element and the n+1's element as the second </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static Tuple<nt,nt> LucasUPair(int p, int q, nt mod, nt n)
        {
            // Find mask to match top bit of n
            var mask = n.TopBitMask();

            // Set up initial values of the U pair
            nt uk = 0;
            nt ukp1 = 1;

            // while the mask is non-zero
            while (mask != 0)
            {
                nt ukNew;
                nt ukp1New;
                // if the masked bit is 1
                if ((n & mask) != 0)
                {
                    // Calculate the 2k+1 and 2k+2 values of U
                    ukNew = ((ukp1 * ukp1) % mod) - ((q * uk * uk) % mod);
                    ukp1New = ((p * ukp1 * ukp1) % mod) - ((2 * q * uk * ukp1) % mod);
                }
                // else if it is 0
                else
                {
                    // Calculate the 2k and 2k+1 values of U
                    ukNew = ((2 * uk * ukp1) % mod) - ((p * uk * uk) % mod);
                    ukp1New = ((ukp1 * ukp1) % mod) - ((q * uk * uk) % mod);
                    if (ukNew < 0)
                    {
                        ukNew += mod;
                    }
                    if (ukp1New < 0)
                    {
                        ukp1New += mod;
                    }
                }
                // Update u pair
                uk = ukNew;
                ukp1 = ukp1New;

                // shift the mask right one bit
                mask >>= 1;
            }

            // return the pair of final U values
            return new Tuple<nt, nt>(uk, ukp1);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Determine the n'th value in the U Lucas sequence </summary>
        ///
        /// <remarks>   Darrellp, 2/13/2011. </remarks>
        ///
        /// <param name="p">    P value for Lucas Sequence. </param>
        /// <param name="q">    Q value for Lucas Sequence. </param>
        /// <param name="mod">  The modulus. </param>
        /// <param name="n">    The index of the term we're searching for (zero based). </param>
        ///
        /// <returns>   The n'th value of the U sequence </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static nt LucasU(int p, int q, nt mod, nt n)
        {
            return LucasUPair(p, q, mod, n).Item1;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Determine the n'th value in the V Lucas sequence </summary>
        ///
        /// <remarks>   Darrellp, 2/13/2011. </remarks>
        ///
        /// <param name="p">    P value for Lucas Sequence. </param>
        /// <param name="q">    Q value for Lucas Sequence. </param>
        /// <param name="mod">  The modulus. </param>
        /// <param name="n">    The index of the term we're searching for (zero based). </param>
        ///
        /// <returns>   The n'th value of the V sequence </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static nt LucasV(int p, int q, nt mod, nt n)
        {
            var pair = LucasUPair(p, q, mod, n);
            var v = ((2 * pair.Item2) % mod) - ((p * pair.Item1) % mod);
            if (v < 0)
            {
                v += mod;
            }
            return v;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Returns the n'th value of both the U and V Lucas sequences. </summary>
        ///
        /// <remarks>   Darrellp, 2/13/2011. </remarks>
        ///
        /// <param name="p">    P value for Lucas Sequence. </param>
        /// <param name="q">    Q value for Lucas Sequence. </param>
        /// <param name="mod">  The modulus. </param>
        /// <param name="n">    The index of the term we're searching for (zero based). </param>
        ///
        /// <returns>   A tuple with the U value as the first member and the V value as the second. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static Tuple<nt, nt> LucasBoth(int p, int q, nt mod, nt n)
        {
            var pair = LucasUPair(p, q, mod, n);
            return new Tuple<nt, nt>(pair.Item1, ((2 * pair.Item2) % mod) - ((p * pair.Item1) % mod));
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Implements the Lucas Psuedoprime test. </summary>
        ///
        /// <remarks>   Taken from Computational Number theory by Wagon and Bressoud.  The implementation
        /// in the book is a bit confusing since it suddenly introduces a test on GCD(n, 2qd) which isn't
        /// mentioned anywhere else in the text as far as I can see.  It isn't in Corollary 8.2.  It is
        /// explicitly mentioned in "Prime Numbers: A Computational Perspective" by Crandall and
        /// Pomerance, but no proof of explanation seems to be given there for it's inclusion in the
        /// theorem.  I try to follow CNT's lead in spite of the fact that I'm not sure I understand it.
        /// Also, in CNT, they just resort to PrimeQ when n divides into 2qd.  I don't have the luxury
        /// of resorting to PrimeQ in that case, so I just make it a condition that 2qd doesn't divide n
        /// and throw an argument exception if it does.  Typically, q and d will be small and n will be
        /// large so this is not a factor, but I'm trying to be complete here.
        /// Darrellp, 2/13/2011. </remarks>
        ///
        /// <exception cref="ArgumentException">    Thrown when n divides into 2q(p^2 - 4q)</exception>
        ///
        /// <param name="p">    P value for Lucas Sequence. </param>
        /// <param name="q">    Q value for Lucas Sequence. </param>
        /// <param name="n">    The index of the term we're searching for (zero based). </param>
        ///
        /// <returns>   true if p appears to be a prime, false if it's definitely composite. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static bool LucasPsuedoprimeTest(int p, int q, nt n)
        {
            // Initialize
            var d = p*p - 4*q;
            var g = (int)n.GCD(2*q*d);

            // If the test conditions aren't met
            if (n == g)
            {
                throw new ArgumentException("Invalid p, q, n in LucasPsuedoPrimeTest");
            }

            // If the test is trivially false
            if (g > 1 && g < n)
            {
                return false;
            }

            // Check the Lucas U value for n - (d/n)
            return LucasU(p, q, n, n - Quadratic.Jacobi(d, n)) == 0;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Implements the Lucas Psuedoprime test. </summary>
        ///
        /// <remarks>   I would like to implement the Mathematica PrimeQ test which is documented as "using
        /// the Lucas psuedoprime test" in several places.  The trouble is, "Lucas psuedoprime test" is 
        /// ambiguous since it relies on two values, p and q, which never seem to be specified in said
        /// documentation.  It would appear from a chapter in "Mathematica in Action" by Wagon that in the
        /// Mathematica version, q is 1 and p is chosen to the be smallest value such that p^2-4q is not a
        /// square mod n and GCD(p,n) == 1. But in CNT, it explains that using q = +/-1 is a bad idea.  
        /// It's all very confusing.  I'm going to use q = 2 and hope that this is as good or better than
        /// the Mathematica PrimeQ, though it's clear as mud.  We want Jacobi(d,n) == -1 so that when we
        /// use this in conjunction with the Euclidean psuedoprime tests we don't overlap effort.
        /// 
        /// This is mostly taken from "Mathematica in Action" since I don't think it's described in CNT.
        /// Darrellp, 2/13/2011. </remarks>
        ///
        /// <param name="n">    The index of the term we're searching for (zero based). </param>
        ///
        /// <returns>   true if the test passes, false if the test fails. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static bool LucasPsuedoprimeTest(nt n)
        {
            var p = 3;

            // Find an appropriate parameter
            for (; Quadratic.Jacobi(p * p - 8, n) == 1 || n.GCD(p) != 1; p++) { }

            // Use it in the Lucas primality test
            return LucasU(p, 2, n, n + 1) == 0;
        }

        //!+ TODO: Implement the MethodA and Quadratic Lucas tests as given in CNT
    }
}
