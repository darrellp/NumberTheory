#if BIGINTEGER
using System;
using System.Collections.Generic;
using System.Linq;
using nt=System.Numerics.BigInteger;
#elif LONG
using System;
using System.Collections.Generic;
using System.Linq;
using nt = System.Int64;
#endif


#if BIGINTEGER
namespace NumberTheoryBig
#elif LONG
namespace NumberTheoryLong
#endif
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Static class to hold functions related to factoring. </summary>
    ///
    /// <remarks>   Darrellp, 2/12/2011. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public static class Factoring
    {
        static readonly Random Rnd = new Random();
        private const int MaxGathers = 10;

        // ReSharper disable FunctionNeverReturns
        private static IEnumerable<nt> PollardSequence(nt n, long seedLong)
        {
            nt seed;
            if (seedLong == -1)
            {
                seed = (nt)(Rnd.NextDouble() * Int64.MaxValue);
            }
            else
            {
                seed = seedLong;
            }
            while (true)
            {
                yield return seed;
                seed = (seed*seed + 1) % n;
            }
        }

        private static IEnumerable<nt> PollardDiffs(nt n, long seed)
        {
            var seq = PollardSequence(n, seed);
            var sLast = seq.First();
            seq = seq.Skip(1);
            var twoCount = 1;
            while (true)
            {
                var arSeq = seq.Skip(twoCount).Take(twoCount).ToArray();
                seq = seq.Skip(2 * twoCount);

                foreach (var sCur in arSeq)
                {
                    yield return TypeAdaptation.Abs(sLast - sCur);
                }
                sLast = arSeq[twoCount - 1];
                twoCount *= 2;
            }
        }
        // ReSharper restore FunctionNeverReturns

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Pollard Rho factoring. </summary>
        ///
        /// <remarks>   This follows the algorithm given in Computational Number Theory by Stan Wagon and
        /// David Bressoud.  It utilizes the optimization suggested there by gathering MaxGathers values at
        /// a time and doing the GCD on that product and n rather than performing a GCD at every step.  It
        /// may fail in one of two ways - it might not find a factor in the given number of iterations, in which
        /// case it returns -1 or it may happen that all the factors are present in the cycle of the rho, in
        /// in which case it returns n.  The former happens most commonly on numbers too large and the latter on
        /// numbers too small.  It's best to verify that n is not prime using a prime test before calling
        /// this routine since it will just cycle the max number of iters on primes.
        /// 
        /// Darrellp, 2/12/2011.</remarks>
        ///
        /// <param name="n">        The value to be factored. </param>
        /// <param name="seed">     The seed to be used for random number generation.  Defaults to using random seed. </param>
        /// <param name="cIters">   The count of iterations before giving up. </param>
        ///
        /// <returns>   -1 if the algorithm failed to find a factor in cIters iterations.  n if the factors
        /// couldn't be separated.  Any other value is a bonafide non-trivial factor. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static nt PollardRho(nt n, long seed = -1, int cIters = 10000)
        {
            var diffs = PollardDiffs(n, seed);

            for (var i = 0; i < cIters; i += MaxGathers)
            {
                var cDiffs = Math.Min(MaxGathers, cIters - i);
                var fact = diffs.Take(cDiffs).Aggregate((a, v) => (a * v) % n).GCD(n);
                if (fact != 1)
                {
                    return fact;
                }
                diffs = diffs.Skip(cDiffs);
            }
            return -1;
        }
    }
}
