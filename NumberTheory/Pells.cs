using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace NumberTheory;

/// <summary>
/// Solve Pell's equation through continued fractions
/// </summary>
public static class Pells
{
	/// <summary>
	/// Solve Pell's equation x^2-d*y^2=r
	/// </summary>
	/// <param name="d">d in the above equation</param>
	/// <param name="rhs">r in the above equation - currently only +1 is supported</param>
	/// <param name="x">returns smallest x for which equation is true</param>
	/// <param name="y">returns smallest y for which equation is true</param>
	/// <returns>false if there is no solution, else true</returns>
	static public bool SolvePells<T>(T d, int rhs, out T x, out T y) where T : IBinaryInteger<T>
	{
		return SolvePells(d, rhs, T.One, out x, out y);
	}

	/// <summary>
	/// Overloads SolvePells above to allow passing in k, the k'th solution.
	/// </summary>
	/// <param name="d">d in the above equation</param>
	/// <param name="rhs">r in the above equation</param>
	/// <param name="k">Index of the solution desired</param>
	/// <param name="x">returns x for which equation is true</param>
	/// <param name="y">returns y for which equation is true</param>
	/// <returns>True if we can solve, else false</returns>
	static public bool SolvePells<T>(T d, int rhs, T k, out T x, out T y) where T : IBinaryInteger<T>
	{
		var two = T.One + T.One;
		x = y = T.Zero;
		List<T> cnfRepeat;
		var mtx = PellMatrix(d, out cnfRepeat);

		switch (rhs)
		{
			case 1:
				var kt = ((cnfRepeat.Count & 1) != 0) ? k : two * k;
				var mtxp = PowerMod.MatrixPower(kt, mtx);
				x = mtxp[0];
				y = mtxp[1];
				return true;

			case -1:
				if ((cnfRepeat.Count & 1) != 0)
				{
					return false;
				}
				var mtxn = PowerMod.MatrixPower(two * k - T.One, mtx);
				x = mtxn[0];
				y = mtxn[1];
				return true;

			default:
				return false;
		}
	}

	/// <summary>
	/// Returns the matrix for a Pell equation as defined in p. 230 of
	/// "Computational Number Theory"
	/// </summary>
	/// <param name="d">D for Pell's equation</param>
	/// <param name="cnfRepeat">Gives the CNF parameters for one full repeat</param>
	/// <returns>The 2 x 2 matrix</returns>
	static public T[] PellMatrix<T>(T d, out List<T> cnfRepeat) where T : IBinaryInteger<T>
	{
		T p = T.Zero;
		T q = T.One;
		cnfRepeat = new List<T> { d.IntegerSqrt() };

		var firstTime = true;
		while (firstTime || q != T.One)
		{
			firstTime = false;
			p = cnfRepeat[cnfRepeat.Count - 1] * q - p;
			q = (d - p * p) / q;
			cnfRepeat.Add((p + cnfRepeat[0]) / q);
		}
		var cnvVal = new ContinuedFraction<T>(cnfRepeat.Take(cnfRepeat.Count - 1)).Val;
		return new[] { cnvVal.Num, cnvVal.Den, d * cnvVal.Den, cnvVal.Num };
	}
}