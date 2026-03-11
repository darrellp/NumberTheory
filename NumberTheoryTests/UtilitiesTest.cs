using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NumberTheory;

namespace NumberTheoryTests
{
	[TestClass]
	public class UtilitiesTest
	{
		[TestMethod]
		public void TwosExponentTest()
		{
			// Tests were disabled with #if NOTIN in the original
		}

		[TestMethod]
		public void BitCountTest()
		{
			Assert.AreEqual(3, 4L.BitCount());
			var n = (long)Math.Pow(2, 37);
			Assert.AreEqual(38, n.BitCount());
		}

		[TestMethod]
		public void IRootTest()
		{
			int[] testCases = { 3, 7, 233, 70, 256, 47 };

			for (var k = 2; k < 5; k++)
			{
				foreach (var n in testCases)
				{
					RootTest(n, k);
				}
			}
			Assert.AreEqual(10L, Utilities.IntegerRoot(11111111L, 7L));
		}

		private void RootTest(int n, int k)
		{
			long power = PowerMod.Power((long)n, (long)k);
			Assert.AreEqual((long)n, Utilities.IntegerRoot(power, (long)k));
			Assert.AreEqual((long)n, Utilities.IntegerRoot(power + 1, (long)k));
			Assert.AreEqual((long)(n - 1), Utilities.IntegerRoot(power - 1, (long)k));
		}

		[TestMethod]
		public void ISqrtTest()
		{
			Assert.AreEqual(5L, 31L.IntegerSqrt());
			Assert.AreEqual(7L, 63L.IntegerSqrt());
			Assert.AreEqual(0L, 0L.IntegerSqrt());
			Assert.AreEqual(4L, 16L.IntegerSqrt());
			Assert.AreEqual(6L, 36L.IntegerSqrt());
			Assert.AreEqual(6L, 37L.IntegerSqrt());
			Assert.AreEqual(5L, 35L.IntegerSqrt());
		}

		[TestMethod]
		public void TopBitMaskTest()
		{
			Assert.AreEqual(0L, 0L.TopBitMask());
			var n = (long)Math.Pow(2, 37) + (long)Math.Pow(2, 36) + 100;
			Assert.AreEqual((long)Math.Pow(2, 37), n.TopBitMask());
		}
	}
}
