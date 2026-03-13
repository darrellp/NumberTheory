using System;
using System.Numerics;

namespace NumberTheory;

/// <summary>   PowerMod code for the number theory library. </summary>
public static class PowerMod
{
	/// <summary>   Returns x^n with no modulus. </summary>
	/// <param name="x">    Value to be exponentiated. </param>
	/// <param name="n">    The exponent. </param>
	/// <returns>   x^n </returns>
	public static T Power<T>(T x, T n) where T : IBinaryInteger<T>
	{
		return Power(x, n, -T.One);
	}

	/// <summary>   Returns x^n Mod mod. </summary>
	/// <remarks>
	/// Uses the binary exponentiation algorithm. If mod is less than 2, no modular
	/// reduction is performed (allowing plain exponentiation). Negative exponents
	/// are handled via modular inverse.
	/// </remarks>
	/// <param name="x">    Value to be exponentiated. </param>
	/// <param name="n">    The exponent. </param>
	/// <param name="mod">  The modulus. Use a value less than 2 for no modulus. </param>
	/// <returns>   x^n Mod mod </returns>
	public static T Power<T>(T x, T n, T mod) where T : IBinaryInteger<T>
	{
		var two = T.One + T.One;
		bool fMod = mod >= two;
		if (!fMod && n < T.Zero)
		{
			throw new ArgumentException("Trying to take negative exponent without a modulus");
		}

		if (T.IsZero(n))
		{
			return T.One;
		}

		var fNeg = false;
		if (n < T.Zero)
		{
			fNeg = true;
			n = -n;
		}
		var mask = n.TopBitMask();
		T res = T.One;

		while ((mask & n) == T.Zero)
		{
			mask >>= 1;
		}
		while (!T.IsZero(mask))
		{
			res *= res;
			if ((mask & n) != T.Zero)
			{
				res *= x;
			}
			if (mod > T.Zero)
			{
				res %= mod;
			}
			mask >>= 1;
		}
		if (fNeg)
		{
			res = res.InverseMod(mod);
		}
		return res;
	}

	private static T[] MatrixMultiply<T>(T[] m1, T[] m2) where T : IBinaryInteger<T>
	{
		var mRet = new T[4];
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
	/// <param name="matrix">The matrix as an array. First two elements
	/// is the first row and the last two are the second row.</param>
	/// <returns>matrix^n in the same format as the input matrix</returns>
	public static T[] MatrixPower<T>(T n, T[] matrix) where T : IBinaryInteger<T>
	{
		if (T.IsZero(n))
		{
			return [T.One, T.Zero, T.Zero, T.One];
		}

		var mask = n.TopBitMask();
		T[] res = [T.One, T.Zero, T.Zero, T.One];

		while ((mask & n) == T.Zero)
		{
			mask >>= 1;
		}
		while (!T.IsZero(mask))
		{
			res = MatrixMultiply(res, res);
			if ((mask & n) != T.Zero)
			{
				res = MatrixMultiply(res, matrix);
			}
			mask >>= 1;
		}
		return res;
	}
}
