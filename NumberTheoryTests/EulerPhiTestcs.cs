using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NumberTheory;
using static NumberTheory.EulerPhi;

namespace NumberTheoryTests
{
	[TestClass]
	public class EulerPhiTests
	{
		[TestMethod]
		public void TestEulerPhi()
		{
			var x = new List<long> { 1, 2, 49, 50, 51, 97, 98, 99, 100, 101, 125, 202, 250, 1234567 };
			var y = new List<long> { 1, 1, 42, 20, 32, 96, 42, 60, 40, 100, 100, 100, 100, 1224720 };

			for (var i = 0; i < x.Count; i++)
			{
				Assert.AreEqual(y[i], Phi(x[i]));
			}
		}
	}
}
