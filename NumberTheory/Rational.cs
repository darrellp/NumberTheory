using System.Numerics;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace NumberTheory;

/// <summary>
/// Represents a rational number
/// </summary>
public class Rational<T> where T : IBinaryInteger<T>
{
	/// <summary>
	/// Numerator for the rational
	/// </summary>
	public T Num
	{
		get { Reduce(); return _num; }
		private set => _num = value;
	}

	/// <summary>
	/// Denominator for the rational
	/// </summary>
	public T Den
	{
		get { Reduce(); return _den; }
		private set => _den = value;
	}

	private bool _fReduced;
	private T _num;
	private T _den;

	/// <summary>
	/// Constructor for a rational
	/// </summary>
	/// <param name="num">numerator</param>
	/// <param name="den">denominator</param>
	public Rational(T num, T den)
	{
		_num = num;
		_den = T.IsZero(num) ? T.One : den;
		_fReduced = false;
		Reduce();
	}

	/// <summary>
	/// Defines equality for rationals
	/// </summary>
	/// <param name="other">The rational we're comparing ourselves to</param>
	/// <returns>True if the two rationals are equal, false otherwise</returns>
	protected bool Equals(Rational<T> other)
	{
		if (other == null)
		{
			return false;
		}
		Reduce();
		other.Reduce();
		return _num.Equals(other._num) && _den.Equals(other._den);
	}

	/// <summary>
	/// Determines whether the specified object is equal to this.
	/// </summary>
	public override bool Equals(object obj)
	{
		return !ReferenceEquals(null, obj) &&
			(ReferenceEquals(this, obj) ||
			 Equals(obj as Rational<T>));
	}

	/// <summary>
	/// Serves as a hash function.
	/// </summary>
	public override int GetHashCode()
	{
		unchecked
		{
			Reduce();
			var hashCode = 397 ^ _num.GetHashCode();
			hashCode = (hashCode * 397) ^ _den.GetHashCode();
			return hashCode;
		}
	}

	/// <summary>
	/// Returns the floor of this rational
	/// </summary>
	public T Floor()
	{
		return Num / Den;
	}

	/// <summary>
	/// Returns the ceiling of this rational
	/// </summary>
	public T Ceiling()
	{
		return (Num + Den - T.One) / Den;
	}

	/// <summary>
	/// Returns the fractional part of this rational
	/// </summary>
	public Rational<T> Frac()
	{
		return new Rational<T>(Num % Den, Den);
	}

	/// <summary>
	/// Returns the reciprocal of this rational
	/// </summary>
	public Rational<T> Recip()
	{
		return new Rational<T>(Den, Num);
	}

	/// <summary>
	/// Turns a T value into a rational over 1
	/// </summary>
	public static implicit operator Rational<T>(T i)
	{
		return new Rational<T>(i, T.One);
	}

	/// <summary>
	/// Addition of rationals
	/// </summary>
	public static Rational<T> operator +(Rational<T> r1, Rational<T> r2)
	{
		return new Rational<T>(r1.Num * r2.Den + r2.Num * r1.Den, r1.Den * r2.Den);
	}

	/// <summary>
	/// Subtraction of rationals
	/// </summary>
	static public Rational<T> operator -(Rational<T> r1, Rational<T> r2)
	{
		return new Rational<T>(r1.Num * r2.Den - r2.Num * r1.Den, r1.Den * r2.Den);
	}

	/// <summary>
	/// Multiplication of rationals
	/// </summary>
	public static Rational<T> operator *(Rational<T> r1, Rational<T> r2)
	{
		return new Rational<T>(r1.Num * r2.Num, r1.Den * r2.Den);
	}

	/// <summary>
	/// Division of rationals
	/// </summary>
	public static Rational<T> operator /(Rational<T> r1, Rational<T> r2)
	{
		return new Rational<T>(r1.Num * r2.Den, r1.Den * r2.Num);
	}

	/// <summary>
	/// Reduce the rational to its lowest terms.
	/// </summary>
	public void Reduce()
	{
		if (_fReduced)
		{
			return;
		}
		_fReduced = true;
		var gcd = Den.GCD(Num);
		if (gcd != T.One)
		{
			Num /= gcd;
			Den /= gcd;
		}
	}

	/// <summary>
	/// Returns a string representation of the rational.
	/// </summary>
	public override string ToString()
	{
		return $"{Num}/{Den}";
	}

	/// <summary>
	/// Defining equality for rationals
	/// </summary>
	public static bool operator ==(Rational<T> a, Rational<T> b)
	{
		if (a is null && b is null)
		{
			return true;
        }
		else if (a is null || b is null)
		{
			return false;
        }
        return a.Den == b.Den && a.Num == b.Num;
	}

	/// <summary>
	/// Defining inequality for rationals
	/// </summary>
	public static bool operator !=(Rational<T> a, Rational<T> b)
	{
		return !(a == b);
	}
}
