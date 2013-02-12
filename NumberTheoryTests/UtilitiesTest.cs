﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

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
			nt m;
			var n = Utilities.TwosExponent(4, out m);
			Assert.AreEqual(1, m);
			Assert.AreEqual(2,n);
			n = Utilities.TwosExponent(12, out m);
			Assert.AreEqual(3, m);
			Assert.AreEqual(2,n);
		}
		[TestMethod]
		public void BitCountTest()
		{
#if BIGINTEGER
			var n = BigInteger.Pow(2, 73);
			Assert.AreEqual(74,n.BitCount());
#else
			Assert.AreEqual(3, 4l.BitCount());
			var n = (long)Math.Pow(2, 37);
			Assert.AreEqual(38,n.BitCount());
#endif
		}

		[TestMethod]
		public void IRootTest()
		{
			for (int i = 1; i < 32; i++)
			{
				Assert.AreEqual(1, Utilities.IntegralRootOf(i, 5));
			}
			for (int i = 32; i < 242; i++)
			{
				Assert.AreEqual(2, Utilities.IntegralRootOf(i, 5));
			}
			Assert.AreEqual(3, Utilities.IntegralRootOf(243, 5));
		}

		[TestMethod]
		public void ISqrtTest()
		{
			Assert.AreEqual(5, ((nt)31).ISqrt());
			Assert.AreEqual(7, ((nt)63).ISqrt());
			Assert.AreEqual(0, ((nt)0).ISqrt());
			Assert.AreEqual(4, ((nt)16).ISqrt());
			Assert.AreEqual(6, ((nt)36).ISqrt());
			Assert.AreEqual(6, ((nt)37).ISqrt());
			Assert.AreEqual(5, ((nt)35).ISqrt());
		}

		[TestMethod]
		public void TopBitMaskTest()
		{
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
