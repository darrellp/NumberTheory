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
    public static class Factoring
    {
        static readonly Random Rnd = new Random();
        private const int MaxGathers = 10;

        // ReSharper disable FunctionNeverReturns
        static IEnumerable<nt> PollardSequence(nt n, long seedLong)
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
