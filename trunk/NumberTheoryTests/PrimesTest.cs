using Microsoft.VisualStudio.TestTools.UnitTesting;

#if BIGINTEGER
using NumberTheoryBig;
using nt = System.Numerics.BigInteger;
#else
using NumberTheoryLong;
using nt = System.Int64;
#endif

namespace NumberTheoryTests
{
    [TestClass]
    public class PrimesTest
    {
        [TestMethod]
        public void StrongPsuedoPrimeTest()
        {
            Assert.IsTrue(Primes.StrongPsuedoPrime(2047, 2));
            Assert.IsFalse(Primes.StrongPsuedoPrime(2049, 2));
            Assert.IsFalse(Primes.StrongPsuedoPrime(2051, 2));
            Assert.IsTrue(Primes.StrongPsuedoPrime(2053, 2));
#if BIGINTEGER
            for (var i = 2; i < 13; i++)
            {
                if (i == 11)
                {
                    Assert.IsFalse(Primes.StrongPsuedoPrime(3215031751, i));
                }
                else
                {
                    Assert.IsTrue(Primes.StrongPsuedoPrime(3215031751, i));
                }
            }
#endif
        }
    }
}
