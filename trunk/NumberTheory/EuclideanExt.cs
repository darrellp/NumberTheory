using System.Linq;
#if BIGINTEGER
using nt = System.Numerics.BigInteger;
#elif LONG
using nt = System.Int64;
#endif

namespace NumberTheory
{
    public class EuclideanExt
    {
        public nt GCD { get; private set; }
        public nt Coeff1 { get; private set; }
        public nt Coeff2 { get; private set; }

        internal EuclideanExt(nt val1, nt val2)
        {
            var rst = new[] { new[] { val1, 1, 0 }, new[] { val2, 0, 1 } };

            while (rst[1][0] != 0)
            {
                var q = rst[0][0]/rst[1][0];
                var rst1Save = rst[1];
                rst[1] = rst[0].Zip(rst[1], (v1, v2) => v1 - q*v2).ToArray();
                rst[0] = rst1Save;
            }
            GCD = rst[0][0];
            Coeff1 = rst[0][1];
            Coeff2 = rst[0][2];
        }

    }
}
