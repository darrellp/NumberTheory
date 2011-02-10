﻿#if BIGINTEGER
using System;
using System.Numerics;
using nt = System.Numerics.BigInteger;
#elif LONG
using nt = System.Int64;
#endif

using Microsoft.VisualStudio.TestTools.UnitTesting;

using NumberTheory;
using euc = NumberTheory.Euclidean;

namespace NumberTheoryTests
{
    [TestClass]
    public class EuclideanAlgorithmTest
    {
        [TestMethod]
        public void TestGCD()
        {
            Assert.AreEqual(1, euc.GCD(97, 18));
            Assert.AreEqual(6, euc.GCD(102, 18));
            Assert.AreEqual(6, euc.GCD(18, 102));
        }

        [TestMethod]
        public void TestEuclideanExt()
        {
            var ext = euc.EuclideanExt(97, 18);
            Assert.AreEqual(1,ext.GCD);
            Assert.AreEqual(-5, ext.Coeff1);
            Assert.AreEqual(27, ext.Coeff2);
            ext = euc.EuclideanExt(541, 7919);
            Assert.AreEqual(1, ext.GCD);
            Assert.AreEqual(-1010, ext.Coeff1);
            Assert.AreEqual(69, ext.Coeff2);
        }

        [TestMethod]
        public void TestDiophantineSolve()
        {
            var fn = euc.DiophantineSolve(13, 51, 500);

            for (var i = 1; i < 6; i++)
            {
                var sln = fn(i);
                Assert.AreEqual(500, sln[0]*13 + sln[1]*51);
            }
        }
    }
}