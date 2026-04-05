using BenchmarkDotNet.Attributes;
using NumberTheory;
using System.Collections.Generic;
using System.Numerics;

namespace NumberTheoryBenchmarks
{
	[MemoryDiagnoser]
	[SimpleJob(warmupCount: 3, iterationCount: 5)]
	public class FactoringBenchmarks
	{
		private static readonly long SmallComposite = 7907L * 7919L;
		private static readonly BigInteger MediumComposite = BigInteger.Parse("12345678921") * BigInteger.Parse("12345678933");

		[Benchmark]
		public long PollardRho_SmallComposite()
		{
			return Factoring.PollardRho(SmallComposite);
		}

		[Benchmark]
		public List<PrimeFactor<long>> FullFactor_SmallComposite()
		{
			return Factoring.Factor(SmallComposite);
		}

		[Benchmark]
		public long QuadraticSieve_SmallComposite()
		{
			return QuadraticSieve.Factor(SmallComposite);
		}

		[Benchmark]
		public BigInteger QuadraticSieve_MediumComposite()
		{
			return QuadraticSieve.Factor(MediumComposite);
		}
	}
}