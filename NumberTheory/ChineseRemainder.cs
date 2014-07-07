using System.Linq;
#if BIGINTEGER
using nt=System.Numerics.BigInteger;
#elif LONG
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
	/// <summary>   Static class to hold functions related to the Chinese remainder. </summary>
	///
	/// <remarks>   Darrellp, 2/12/2011. </remarks>
	////////////////////////////////////////////////////////////////////////////////////////////////////

	public static class ChineseRemainder
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>	
		/// Implements the Chinese Remainder Theorem.  This version of the algorithm only works for
		/// pairwise coprime moduli. This algorithm solves for x a set of simultaneous congruencies of
		/// the form x % mods[k] == a[k] where mods and a are parameters to the function. 
		/// </summary>
		///
		/// <remarks>	Darrellp, 2/12/2011. </remarks>
		///
		/// <param name="aVals">	Array of values for the a[k] in the equation above. </param>
		/// <param name="mods">		The mods in the equation above. </param>
		///
		/// <returns>	Value which satisfies all the above congruencies. </returns>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		// ReSharper disable once ParameterTypeCanBeEnumerable.Global
		public static nt CRT(nt[] aVals, nt[] mods)
		{ 
			// Ensure that the moduli are pairwise relatively prime
			var mult = mods.Aggregate((acc, next) => acc * next);

			// If they're not relatively prime
			if (mult != Euclidean.LCM(mods))
			{
				return -1;
			}

			// If they are, proceed with the CRT
			return mods.
				Select(m => (mult/m).InverseMod(m) * mult / m).
				Zip(aVals, (m, a) => a * m).
				Aggregate((acc,val) => acc + val) % mult;
		}
	}
}
