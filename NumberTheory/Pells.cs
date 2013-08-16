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
			return SolvePells(d, rhs, 1, out x, out y);
		}

		static public bool SolvePells(nt d, int rhs, nt k, out nt x, out nt y)
		{
			x = y = 0;
			List<nt> cnfRepeat;
			var mtx = PellMatrix(d, out cnfRepeat);

			switch (rhs)
			{
				case 1:
					var kt = ((cnfRepeat.Count & 1) != 0) ? k : 2 * k;
					var mtxp = PowerMod.MatrixPower(kt, mtx);
					x = mtxp[0];
					y = mtxp[1];
					return true;

				case -1:
					if ((cnfRepeat.Count & 1) != 0)
					{
						return false;
					}
					var mtxn = PowerMod.MatrixPower(2 * k - 1, mtx);
					x = mtxn[0];
					y = mtxn[1];
					return true;

				default:
					return false;
			}
		}

		static public nt[] PellMatrix(nt d, out List<nt> cnfRepeat)
		{
			nt p = 0;
			nt q = 1;
			cnfRepeat = new List<nt> { d.IntegerSqrt() };

			var firstTime = true;
			while (firstTime || q != 1)
			{
				firstTime = false;
				p = cnfRepeat[cnfRepeat.Count - 1] * q - p;
				q = (d - p * p) / q;
				cnfRepeat.Add((p + cnfRepeat[0]) / q);
			}
			Rational cnvVal = new ContinuedFraction(cnfRepeat.Take(cnfRepeat.Count - 1)).Val;
			return new nt[] {cnvVal.Num, cnvVal.Den, d * cnvVal.Den, cnvVal.Num};
		}
	}
}