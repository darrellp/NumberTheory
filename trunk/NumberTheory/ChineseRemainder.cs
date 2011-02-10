using System.Linq;
#if BIGINTEGER
using System;
using System.Numerics;
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
    public static class ChineseRemainder
    {
        public static nt CRT(nt[] aVals, nt[] mods)
        { 
            var mult = mods.Aggregate((acc, next) => acc * next);

            if (mult != Euclidean.LCM(mods))
            {
                return -1;
            }
            return mods.
                Select(m => (mult/m).InverseMod(m) * mult / m).
                Zip(aVals, (m, a) => a * m).
                Aggregate((acc,val) => acc + val) % mult;
        }
    }
}
