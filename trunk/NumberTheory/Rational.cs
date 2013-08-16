using System;
#if BIGINTEGER
using nt = System.Numerics.BigInteger;
#elif LONG
using nt = System.Int64;
#endif

#if BIGINTEGER
// ReSharper disable once CheckNamespace
namespace NumberTheoryBig
#elif LONG
// ReSharper disable once CheckNamespace
namespace NumberTheoryLong
#endif
{
	/// <summary>
	/// Represents a rational number
	/// </summary>
	public class Rational
	{
		/// <summary>
		/// Numerator for the rational
		/// </summary>
		public nt Num
		{
			get { Reduce(); return _num; }
			private set { _num = value; }
		}

		/// <summary>
		/// Denominator for the rational
		/// </summary>
		public nt Den
		{
			get { Reduce(); return _den; }
			private set { _den = value; }
		}

		private bool _fReduced;
		private nt _num;
		private nt _den;

		/// <summary>
		/// Constructor for a rational
		/// </summary>
		/// <param name="num">numerator</param>
		/// <param name="den">denominator</param>
		public Rational(nt num, nt den)
		{
			_num = num;
			_den = num == 0 ? 1 : den;
			_fReduced = false;
			Reduce();
		}

		/// <summary>
		/// Defines equality for rationals
		/// </summary>
		/// <param name="other">The rational we're comparing ourselves to</param>
		/// <returns>True if the two rationals are equal, false otherwise</returns>
		protected bool Equals(Rational other)
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
		/// Determines whether the specified <see cref="T:System.Object"/> is equal to this.
		/// </summary>
		/// <returns>
		/// true if the specified <see cref="T:System.Object"/> is equal to this; otherwise, false.
		/// </returns>
		/// <param name="obj">The <see cref="T:System.Object"/> to compare with this</param>
		public override bool Equals(object obj)
		{
			return !ReferenceEquals(null, obj) &&
				(ReferenceEquals(this, obj) ||
				 Equals(obj as Rational));
		}

		/// <summary>
		/// Serves as a hash function for a particular type. 
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="T:System.Object"/>.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override int GetHashCode()
		{
			unchecked
			{
				Reduce();
				// ReSharper disable NonReadonlyFieldInGetHashCode
				var hashCode = 397 ^ _num.GetHashCode();
				hashCode = (hashCode * 397) ^ _den.GetHashCode();
				// ReSharper restore NonReadonlyFieldInGetHashCode
				return hashCode;
			}
		}

		/// <summary>
		/// Returns a rational representing the floor of this rational
		/// </summary>
		/// <returns>The floor of the rational</returns>
		public nt Floor()
		{
			return Num / Den;
		}

		/// <summary>
		/// Returns a rational representing the Ceiling of this rational
		/// </summary>
		/// <returns>The ceiling of the rational</returns>
		public nt Ceiling()
		{
			return (Num + Den - 1) / Den;
		}

		/// <summary>
		/// Returns a rational representing the fractional part of this rational
		/// </summary>
		/// <returns>The fractional part of the rational</returns>
		public Rational Frac()
		{
			return new Rational( Num % Den, Den);
		}

		/// <summary>
		/// Returns a rational representing the reciprocal of this rational
		/// </summary>
		/// <returns>The reciprocal of the rational</returns>
		public Rational Recip()
		{
			return new Rational(Den, Num);
		}

#if BIGINTEGER
		public static implicit operator Rational(nt i)
		{
			return new Rational(i, 1);
		}
#endif

		/// <summary>
		/// Turns longs into rationals over 1
		/// </summary>
		/// <param name="i">The long to be converted</param>
		/// <returns>A rational representing the long</returns>
		public static implicit operator Rational(Int64 i)
		{
			return new Rational(i, 1);
		}

		/// <summary>
		/// Addition of rationals
		/// </summary>
		/// <param name="r1">First rational</param>
		/// <param name="r2">Second Rational</param>
		/// <returns>Sum of the two rationals</returns>
		static public Rational operator +(Rational r1, Rational r2)
		{
			return new Rational(r1.Num * r2.Den + r2.Num * r1.Den, r1.Den * r2.Den);
		}

		/// <summary>
		/// Subtraction of rationals
		/// </summary>
		/// <param name="r1">First rational</param>
		/// <param name="r2">Second Rational</param>
		/// <returns>difference of the two rationals</returns>
		static public Rational operator -(Rational r1, Rational r2)
		{
			return new Rational(r1.Num * r2.Den - r2.Num * r1.Den, r1.Den * r2.Den);
		}

		/// <summary>
		/// Multiplication of rationals
		/// </summary>
		/// <param name="r1">First rational</param>
		/// <param name="r2">Second Rational</param>
		/// <returns>Product of the two rationals</returns>
		static public Rational operator *(Rational r1, Rational r2)
		{
			return new Rational(r1.Num * r2.Num, r1.Den * r2.Den);
		}

		/// <summary>
		/// Division of rationals
		/// </summary>
		/// <param name="r1">First rational</param>
		/// <param name="r2">Second Rational</param>
		/// <returns>Division result of the two rationals</returns>
		static public Rational operator /(Rational r1, Rational r2)
		{
			return new Rational(r1.Num * r2.Den, r1.Den * r2.Num);
		}

		/// <summary>
		/// Reduce the rational to it's lowest terms.
		/// </summary>
		public void Reduce()
		{
			if (_fReduced)
			{
				return;
			}
			_fReduced = true;
			var gcd = Den.GCD(Num);
			if (gcd != 1)
			{
				Num /= gcd;
				Den /= gcd;
			}
		}

		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the rational.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the rational.
		/// </returns>
		public override string ToString()
		{
			return string.Format("{0}/{1}", Num, Den);
		}

		/// <summary>
		/// Defining equality for rationals
		/// </summary>
		/// <param name="a">First rational</param>
		/// <param name="b">Second rational</param>
		/// <returns>True if they're equal, false otherwise</returns>
		public static bool operator ==(Rational a, Rational b)
		{
			if ((object) a == null || (object) b == null)
			{
				return false;
			}
			return a.Den == b.Den && a.Num == b.Num;
		}

		/// <summary>
		/// Defining inequality for rationals
		/// </summary>
		/// <param name="a">First rational</param>
		/// <param name="b">Second rational</param>
		/// <returns>True if they're unequal, false otherwise</returns>
		public static bool operator !=(Rational a, Rational b)
		{
			return !(a == b);
		}
	}
}
