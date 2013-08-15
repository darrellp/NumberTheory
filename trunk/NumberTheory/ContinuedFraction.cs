using System;
using System.Collections.Generic;
using System.Linq;
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
	/// <summary>
	/// Solve Pell's equation through continued fractions
	/// </summary>
	public static class Cfr
	{
		/// <summary>
		/// Returns the continued fraction form for a/b.  The first value of 
		/// the list will be 0 if a is less than b.  
		/// </summary>
		/// <param name="a">numerator</param>
		/// <param name="b">denonimator</param>
		/// <returns>List of the continued fraction values for a/b</returns>
		static public List<nt> ContinuedFraction(nt a, nt b)
		{
			var ret = new List<nt>();

			while (true)
			{
				ret.Add(a / b);
				var oldB = b;
				b = a % b;
				if (b == 0)
				{
					return ret;
				}
				a = oldB;
			}
		}

		static public nt Convergents(List<nt> cf)
		{
			return -1;
		}
	}
}
