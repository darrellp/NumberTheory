using Microsoft.VisualStudio.TestTools.UnitTesting;

#if BIGINTEGER
using NumberTheoryBig;
using System.Numerics;
using nt = System.Numerics.BigInteger;
#else
using System;
using NumberTheoryLong;
using nt = System.Int64;
#endif

namespace NumberTheoryTests
{
	[TestClass]
	public class UtilitiesTest
	{
		[TestMethod]
		public void TwosExponentTest()
		{
#if NOTIN
			nt m;
			var n = Utilities.TwosExponent(4, out m);
			Assert.AreEqual(1, m);
			Assert.AreEqual(2,n);
			n = Utilities.TwosExponent(12, out m);
			Assert.AreEqual(3, m);
			Assert.AreEqual(2,n);
#endif
		}
		[TestMethod]
		public void BitCountTest()
		{
#if BIGINTEGER
			var n = BigInteger.Pow(2, 73);
			Assert.AreEqual(74,n.BitCount());
#else
			Assert.AreEqual(3, 4L.BitCount());
			var n = (long)Math.Pow(2, 37);
			Assert.AreEqual(38,n.BitCount());
#endif
		}

		[TestMethod]
		public void IRootTest()
		{
			int[] testCases = {3, 7, 233, 70, 256, 47};

			for (var k = 2; k < 5; k++)
			{
				foreach (var n in testCases)
				{
					RootTest(n, k);
				}
			}
			Assert.AreEqual(10, Utilities.IntegerRoot(11111111,7));
		}

		private void RootTest(int n, int k)
		{
			nt power = PowerMod.Power(n, k);
			Assert.AreEqual(n, Utilities.IntegerRoot(power,k));
			Assert.AreEqual(n, Utilities.IntegerRoot(power + 1, k));
			Assert.AreEqual(n - 1, Utilities.IntegerRoot(power - 1, k));
		}

		[TestMethod]
		public void ISqrtTest()
		{
			Assert.AreEqual(5, ((nt)31).IntegerSqrt());
			Assert.AreEqual(7, ((nt)63).IntegerSqrt());
			Assert.AreEqual(0, ((nt)0).IntegerSqrt());
			Assert.AreEqual(4, ((nt)16).IntegerSqrt());
			Assert.AreEqual(6, ((nt)36).IntegerSqrt());
			Assert.AreEqual(6, ((nt)37).IntegerSqrt());
			Assert.AreEqual(5, ((nt)35).IntegerSqrt());
		}

		[TestMethod]
		public void TopBitMaskTest()
		{
			Assert.AreEqual((nt)0, ((nt)0).TopBitMask());
#if BIGINTEGER
			var n = BigInteger.Pow(2, 75) + BigInteger.Pow(2, 74) + 100;
			Assert.AreEqual(BigInteger.Pow(2, 75), n.TopBitMask());
#else
			var n = (long)Math.Pow(2, 37) + (long)Math.Pow(2, 36) + 100;
			Assert.AreEqual((long)Math.Pow(2, 37), n.TopBitMask());
#endif
			
		}
	}
}
