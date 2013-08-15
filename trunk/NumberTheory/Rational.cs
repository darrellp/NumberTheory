using System;
using System.Runtime.CompilerServices;
#if BIGINTEGER
using nt = System.Numerics.BigInteger;
#elif LONG
using nt = System.Int64;
#endif

#if BIGINTEGER
namespace NumberTheoryBig
#elif LONG
namespace NumberTheoryLong
#endif
{
	public class Rational
	{
		public nt Num
		{
			get { Reduce(); return _num; }
			private set { _num = value; }
		}

		public nt Den
		{
			get { Reduce(); return _den; }
			private set { _den = value; }
		}

		private bool _fReduced;
		private nt _num;
		private nt _den;

		public Rational(nt num, nt den)
		{
			_num = num;
			_den = num == 0 ? 1 : den;
			_fReduced = false;
			Reduce();
		}

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

		public override bool Equals(object obj)
		{
			return !ReferenceEquals(null, obj) &&
				(ReferenceEquals(this, obj) ||
				 Equals(obj as Rational));
		}

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

		public nt Floor()
		{
			return Num / Den;
		}

		public nt Ceiling()
		{
			return (Num + Den - 1) / Den;
		}

		public Rational Frac()
		{
			return new Rational( Num % Den, Den);
		}

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

		public static implicit operator Rational(Int64 i)
		{
			return new Rational(i, 1);
		}

		static public Rational operator +(Rational r1, Rational r2)
		{
			return new Rational(r1.Num * r2.Den + r2.Num * r1.Den, r1.Den * r2.Den);
		}

		static public Rational operator -(Rational r1, Rational r2)
		{
			return new Rational(r1.Num * r2.Den - r2.Num * r1.Den, r1.Den * r2.Den);
		}

		static public Rational operator *(Rational r1, Rational r2)
		{
			return new Rational(r1.Num * r2.Num, r1.Den * r2.Den);
		}

		static public Rational operator /(Rational r1, Rational r2)
		{
			return new Rational(r1.Num * r2.Den, r1.Den * r2.Num);
		}

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

		public override string ToString()
		{
			return string.Format("{0}/{1}", Num, Den);
		}

		public static bool operator ==(Rational a, Rational b)
		{
			if ((object) a == null || (object) b == null)
			{
				return false;
			}
			return a.Den == b.Den && a.Num == b.Num;
		}

		public static bool operator !=(Rational a, Rational b)
		{
			return !(a == b);
		}
	}
}
