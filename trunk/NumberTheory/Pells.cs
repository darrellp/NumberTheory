using System;
using System.Collections.Generic;
using System.Linq;
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
	/// <summary>
	/// Solve Pell's equation through continued fractions
	/// </summary>
	public static class Pells
	{
		/// <summary>
		/// Solve Pell's equation x^2-d*y^2=r
		/// </summary>
		/// 
		/// <param name="d">d in the above equation</param>
		/// <param name="rhs">r in the above equation - currently only +1 is supported</param>
		/// <param name="x">returns smallest x for which equation is true</param>
		/// <param name="y">returns smallest y for which equation is true</param>
		/// <returns>false if there is no solution, else true</returns>
		static public bool SolvePells(nt d, int rhs, out nt x, out nt y)
		{
			x = y = 0;
			switch (rhs)
			{
				case 1:
					nt p = 0;
					nt q = 1;
					var ret = new List<nt> { d.IntegerSqrt() };

					var firstTime = true;
					while (firstTime || q != 1)
					{
						firstTime = false;
						p = ret[ret.Count - 1] * q - p;
						q = (d - p * p) / q;
						ret.Add((p + ret[0]) / q);
					}
					Rational cnvVal = new ContinuedFraction(ret.Take(ret.Count - 1)).Val;
					break;

				default:
					throw new NotImplementedException();
			}
			x = y = -1;
			return false;
		}
	}
}