using System;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Collections;
using System.Linq.Expressions;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BloomFilterCore;
using System.Diagnostics;

namespace UnitTestBloomFilter
{
	[TestClass]
	public class TestMultiplicativeGroup
	{
		public TestContext TestContext { get { return m_testContext; } set { m_testContext = value; } }
		private TestContext m_testContext;


		[TestMethod]
		public void TestModulus()
		{
			BloomFilterCore.PrimeHelper.Initialize();

			// BigInteger FindPrimitiveRoot(BigInteger p)
			ParameterExpression paramP = Expression.Parameter(typeof(BigInteger), "p");
			MethodInfo findPrimitiveRootMethodInfo = typeof(BloomFilter).GetMethod("FindPrimitiveRoot", BindingFlags.Static | BindingFlags.NonPublic);
			Expression findPrimitiveRootExpression = Expression.Call(findPrimitiveRootMethodInfo, paramP);
			var findPrimitiveRoot = Expression.Lambda<Func<BigInteger, BigInteger>>(findPrimitiveRootExpression, paramP).Compile();

			// BigInteger PowerMod(BigInteger value, BigInteger exponent, BigInteger modulus)
			ParameterExpression paramValue = Expression.Parameter(typeof(BigInteger), "value");
			ParameterExpression paramExponent = Expression.Parameter(typeof(BigInteger), "exponent");
			ParameterExpression paramModulus = Expression.Parameter(typeof(BigInteger), "modulus");
			MethodInfo powerModMethodInfo = typeof(BloomFilter).GetMethod("PowerMod", BindingFlags.Static | BindingFlags.NonPublic);
			Expression powerModExpression = Expression.Call(powerModMethodInfo, paramValue, paramExponent, paramModulus);
			var powerMod = Expression.Lambda<Func<BigInteger, BigInteger, BigInteger, BigInteger>>(powerModExpression, paramValue, paramExponent, paramModulus).Compile();

			BigInteger p = 1811;
			BigInteger n = 5689;

			BigInteger g = findPrimitiveRoot(p);

			Stopwatch stopWatch = Stopwatch.StartNew();
			stopWatch.Restart();
			BigInteger result1 = powerMod(g, n, p);
			TimeSpan time1 = stopWatch.Elapsed;
			stopWatch.Restart();
			BigInteger result2 = powerMod(g, n.Mod(p - 1), p);
			TimeSpan time2 = stopWatch.Elapsed;

			TestContext.WriteLine($"g^n ≡ result (mod p)");
			TestContext.WriteLine($"{g}^{n} ≡ {result1} (mod {p})");
			TestContext.WriteLine($"({time1.TotalMilliseconds} milliseconds)");
			TestContext.WriteLine("");
			TestContext.WriteLine($"g^(n%(p-1)) ≡ result (mod p)");
			TestContext.WriteLine($"{g}^{n.Mod(p - 1)} ≡ {result2} (mod {p})");
			TestContext.WriteLine($"({time2.TotalMilliseconds} milliseconds)");
			TestContext.WriteLine("");
			TestContext.WriteLine($"Success? {result1 == result2}");

			Assert.AreEqual(result1, result2);
		}
	}
}
