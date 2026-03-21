using System;
using System.Diagnostics;
using System.Formats.Asn1;
using System.Linq;
using System.Numerics;

namespace NumberTheory;

/// <summary>   Static class to hold functions related to the Chinese remainder. </summary>
public static class ChineseRemainder
{
	/// <summary>
	/// Implements the Chinese Remainder Theorem. This version of the algorithm only works for
	/// pairwise coprime moduli. This algorithm solves for x a set of simultaneous congruencies of
	/// the form x % mods[k] == a[k] where mods and a are parameters to the function.
	/// </summary>
	/// <param name="aVals">	Array of values for the a[k] in the equation above. </param>
	/// <param name="mods">		The mods in the equation above. </param>
	/// <returns>	Value which satisfies all the above congruencies. </returns>
	// ReSharper disable once UnusedMember.Global
	public static T CRT<T>(T[] aVals, T[] mods) where T : IBinaryInteger<T>
	{
		// Ensure that the moduli are pairwise relatively prime
		var mult = mods.Aggregate((acc, next) => acc * next);

		// If they're not relatively prime
		if (mult != Euclidean.LCM(mods))
		{
			return -T.One;
		}

		// If they are, proceed with the CRT
		return mods.
			Select(m => (mult / m).InverseMod(m) * mult / m).
			Zip(aVals, (m, a) => a * m).
			Aggregate((acc, val) => acc + val) % mult;
	}
	
	/// <summary>
	/// Implements the Chinese Remainder Theorem. This version of the algorithm only works for
	/// pairwise coprime moduli. This algorithm solves for x a set of simultaneous congruencies of
	/// the form x % mods[k] == a[k] where mods and a are parameters to the function.
	/// </summary>
	/// <param name="aVals">	Array of values for the a[k] in the equation above. </param>
	/// <param name="mods">		The mods in the equation above. </param>
	/// <returns>	Value which satisfies all the above congruencies. </returns>
	// ReSharper disable once UnusedMember.Global
	public static (T value, T modulus) CRTStrong<T>(T[] aVals, T[] mods, out bool success) where T : IBinaryInteger<T>
	{
		if (aVals.Length != mods.Length)
		{
			throw new ArgumentException("aVals and mods must have the same length in CRTStrong.");
		}

		if (aVals.Length == 0)
		{
			throw new ArgumentException("aVals must not be empty.");
		}
		var ret = (aVals[0], mods[0]);
		success = true;
		for (int i = 1; i < aVals.Length; i++)
		{
			ret = CRTStrongTwo(ret, (aVals[i], mods[i]), out success);
			if (!success)
			{
				return ret;
			}
		}
		return ret;
	}

	private static (T value, T mod) CRTStrongTwo<T>((T,T) a1m1, (T,T)a2m2, out bool success) where T : IBinaryInteger<T>
	{
		var a = a1m1.Item1;
		var m = a1m1.Item2;
		var b = a2m2.Item1;
		var n = a2m2.Item2;

		var gcd = m.GCD(n);
		if (!(a - b).Divides(gcd))
		{
			success = false;
			return (T.Zero, T.Zero);
		}

		var lambda = (a - b) / gcd;
		var eucExt = Euclidean.ExtGCD(m, n);
		var (u, v) = (eucExt.Coeff1, eucExt.Coeff2);
		var modRet = m.LCM(n);
		var valRet = (a - m * u * lambda).PositiveMod(modRet);

		success = true;
		// Debug.WriteLine($"a: {a} b: {b} m: {m} n: {n} valRet: {valRet} modRet: {modRet}");
		return (valRet, modRet);
	}
}
