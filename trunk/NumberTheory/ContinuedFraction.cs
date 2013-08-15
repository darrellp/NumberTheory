using System.Collections.Generic;
#if BIGINTEGER
using nt = System.Numerics.BigInteger;
#elif LONG
using nt = System.Int64;
#endif


#if BIGINTEGER

// ReSharper disable once CheckNamespace
namespace NumberTheoryBig
#elif LONG
namespace NumberTheoryLong
#endif
{
	/// <summary>
	/// Represents a continued fraction
	/// </summary>
	public class ContinuedFraction
	{
		public Rational Val { get; private set; }
		public List<nt> Vals {get; private set;}

		/// <summary>
		/// Returns the continued fraction form for a/b.  The first value of 
		/// the list will be 0 if a is less than b.  
		/// </summary>
		/// <param name="a">numerator</param>
		/// <param name="b">denonimator</param>
		/// <returns>List of the continued fraction values for a/b</returns>
		public ContinuedFraction(nt a, nt b)
		{
			Vals = new List<nt>();
			Val = new Rational(a, b);
			while (true)
			{
				Vals.Add(a / b);
				var oldB = b;
				b = a % b;
				if (b == 0)
				{
					return;
				}
				a = oldB;
			}
		}

		public ContinuedFraction(Rational r) : this(r.Num, r.Den){}

		public List<Rational> Convergents()
		{
			if (Vals.Count == 1)
			{
				return new List<Rational> {(Rational)Vals[0]};
			}
			var ret = new List<Rational>();
			var pm2 = Vals[0];
			var qm2 = (nt)1;
			var pm1 = Vals[0] * Vals[1] + 1;
			var qm1 = Vals[1];

			ret.Add(new Rational(pm2, qm2));
			ret.Add(new Rational(pm1, qm1));

			for (var iVal = 2; iVal < Vals.Count; iVal++)
			{
				var pCur = Vals[iVal] * pm1 + pm2;
				var qCur = Vals[iVal] * qm1 + qm2;
				ret.Add(new Rational(pCur, qCur));

				qm2 = qm1;
				pm2 = pm1;
				qm1 = qCur;
				pm1 = pCur;
			}
			return ret;
		}
	}
}
