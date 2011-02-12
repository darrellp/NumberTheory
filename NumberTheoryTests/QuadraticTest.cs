using Microsoft.VisualStudio.TestTools.UnitTesting;

#if BIGINTEGER
using NumberTheoryBig;
#else
using NumberTheoryLong;
#endif

namespace NumberTheoryTests
{
    [TestClass]
    public class QuadraticTests
    {
        [TestMethod]
        public void TestJacobi()
        {
            var results = new[] {1, 0, 1, 0, 0, 0, -1, 0, 1, 0, -1, 0, 1, 0, 0, 0, -1, 0, -1, 0};
            for (var i = 1; i <= 20; i++)
            {
                Assert.AreEqual(results[i - 1], Quadratic.Jacobi(10, i));
            }
        }
    }
}
