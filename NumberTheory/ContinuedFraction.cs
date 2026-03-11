using System.Collections.Generic;
using System.Linq;
using System.Numerics;
// ReSharper disable MemberCanBePrivate.Global

namespace NumberTheory;

/// <summary>
/// Represents a finite continued fraction
/// </summary>
public class ContinuedFraction<T> where T : IBinaryInteger<T>
{
	/// <summary>
	/// The value represented by this continued fraction
	/// </summary>
	public Rational<T> Val { get; private set; }

	/// <summary>
	/// The values in the continued fraction notation, sometimes called
	/// partial quotients.
	/// </summary>
	public List<T> Vals { get; private set; }

	/// <summary>
	/// Returns the continued fraction form for a/b.
	/// </summary>
	/// <param name="a">numerator</param>
	/// <param name="b">denominator</param>
	public ContinuedFraction(T a, T b)
	{
		Vals = new List<T>();
		Val = new Rational<T>(a, b);
		while (true)
		{
			Vals.Add(a / b);
			var oldB = b;
			b = a % b;
			if (T.IsZero(b))
			{
				return;
			}
			a = oldB;
		}
	}

	/// <summary>
	/// Constructor which takes a rational
	/// </summary>
	/// <param name="r">Rational this CF is to represent</param>
	public ContinuedFraction(Rational<T> r) : this(r.Num, r.Den) { }

	/// <summary>
	/// Constructor from the partial quotients
	/// </summary>
	/// <param name="vals">The partial quotients</param>
	public ContinuedFraction(IEnumerable<T> vals)
	{
		Vals = vals.ToList();
		var cnv = Convergents();
		Val = cnv[^1];
	}

	/// <summary>
	/// The convergents of the CF
	/// </summary>
	/// <returns>A list of the convergents</returns>
	public List<Rational<T>> Convergents()
	{
		if (Vals.Count == 1)
		{
			return [new Rational<T>(Vals[0], T.One)];
		}
		var ret = new List<Rational<T>>();
		var pm2 = Vals[0];
		var qm2 = T.One;
		var pm1 = Vals[0] * Vals[1] + T.One;
		var qm1 = Vals[1];

		ret.Add(new Rational<T>(pm2, qm2));
		ret.Add(new Rational<T>(pm1, qm1));

		for (var iVal = 2; iVal < Vals.Count; iVal++)
		{
			var pCur = Vals[iVal] * pm1 + pm2;
			var qCur = Vals[iVal] * qm1 + qm2;
			ret.Add(new Rational<T>(pCur, qCur));

			qm2 = qm1;
			pm2 = pm1;
			qm1 = qCur;
			pm1 = pCur;
		}
		return ret;
	}
}
