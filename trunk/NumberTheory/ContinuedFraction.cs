using System.Collections.Generic;
using System.Linq;
#if BIGINTEGER
using nt = System.Numerics.BigInteger;
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
	/// <summary>
	/// Represents a finite continued fraction
	/// </summary>
	public class ContinuedFraction
	{
		/// <summary>
		/// The value represented by this continued fraction
		/// </summary>
		public Rational Val { get; private set; }
		/// <summary>
		/// The values in the continued fraction notation, sometimes called
		/// partial quotients.
		/// </summary>
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

		/// <summary>
		/// Contructor which takes a rational
		/// </summary>
		/// <param name="r">Rational this CF is to represent</param>
		public ContinuedFraction(Rational r) : this(r.Num, r.Den){}

		/// <summary>
		/// Constructor from the partial quotients
		/// </summary>
		/// <param name="vals">The partial quotients</param>
		public ContinuedFraction(IEnumerable<nt> vals)
		{
			Vals = vals.ToList();
			var cnv = Convergents();
			Val = cnv[cnv.Count - 1];
		}

		/// <summary>
		/// The convergents of the CF
		/// </summary>
		/// <returns>A list of the convergents</returns>
		public List<Rational> Convergents()
		{
			if (Vals.Count == 1)
			{
				return new List<Rational> {Vals[0]};
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
