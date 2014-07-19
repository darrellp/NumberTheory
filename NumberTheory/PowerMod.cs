#if BIGINTEGER
using System.Numerics;
using nt = System.Numerics.BigInteger;
#elif LONG
using System;
using nt = System.Int64;
#endif

// ReSharper disable CheckNamespace
#if BIGINTEGER
namespace NumberTheoryBig
#elif LONG
namespace NumberTheoryLong
#endif
// ReSharper restore CheckNamespace
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

        static public nt Power(nt x, nt n, nt mod = -1)
        {
#if BIGINTEGER
            var nsol = BigInteger.ModPow(x, BigInteger.Abs(n), mod);
            if (n < 0)
            {
                nsol = nsol.InverseMod(mod);
            }
            return nsol;
#else
#if LONG
			if (n > int.MaxValue)
			{
				throw new ArgumentException("n can't exceed int.MaxValue in long version of Power - try BigInteger version");
			}
#endif
			if (n == 0)
            {
                return 1;
            }

	        bool fNeg = n < 0;
	        n = Math.Abs(n);
            var mask = n.TopBitMask();
            nt res = 1;

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
	            if (mod > 0)
	            {
					res = res%mod;
	            }
                mask >>= 1;
            }
	        if (fNeg)
	        {
				res = res.InverseMod(mod);
	        }
            return res;
#endif
        }

	    static private nt[] MatrixMultiply(nt[] m1, nt[] m2)
	    {
		    var mRet = new nt[4];
			mRet[0] = m1[0] * m2[0] + m1[1] * m2[2];
		    mRet[1] = m1[0] * m2[1] + m1[1] * m2[3];
		    mRet[2] = m1[2] * m2[0] + m1[3] * m2[2];
		    mRet[3] = m1[2] * m2[1] + m1[3] * m2[3];
		    return mRet;
	    }

	    /// <summary>
	    /// Takes a power of a matrix using the same splitting technique
	    /// that PowerMod uses.
	    /// </summary>
	    /// <param name="n">The power to be taken</param>
	    /// <param name="matrix">The matrix as an array.  First two elements
	    /// is the first row and the last two are the second row.</param>
	    /// <returns>matrix^n in the same format as the input matrix</returns>
	    static public nt[] MatrixPower(nt n, nt[] matrix)
	    {
			if (n == 0)
			{
				return new nt[] {1, 0, 0, 1};
			}

			var mask = n.TopBitMask();
			var res = new nt[] { 1, 0, 0, 1 };

			while ((mask & n) == 0)
			{
				mask >>= 1;
			}
			while (mask != 0)
			{
				res = MatrixMultiply(res, res);
				if ((mask & n) != 0)
				{
					res = MatrixMultiply(res, matrix);
				}
				mask >>= 1;
			}
			return res;
		}
    }
}
