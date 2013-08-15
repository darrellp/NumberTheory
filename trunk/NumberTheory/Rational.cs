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
		public nt Num { get; private set; }
		public nt Den { get; private set; }
		public bool AutoReduce { get; set; }

		public Rational(nt num, nt den, bool autoReduce = true)
		{
			Num = num;
			Den = den;
			AutoReduce = autoReduce;
			if (AutoReduce)
			{
				Reduce();
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

		public nt Frac()
		{
			return (Num % Den) / Den;
		}

		public Rational Recip()
		{
			return new Rational(Den, Num);
		}

		public static implicit operator Rational(nt i)
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

		private void Reduce()
		{
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
	}
}
