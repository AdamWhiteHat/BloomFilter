using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace BloomFilterCore
{
	public class Factorization
	{
		private static BigInteger _cacheSize = BigInteger.Zero;
		private static List<BigInteger> primeCache = new List<BigInteger> { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47 };

		private static void EnsurePrimeCacheSize(BigInteger size)
		{
			if (_cacheSize < size)
			{
				primeCache = PrimeFactory.GetPrimes(size);
				_cacheSize = size;
			}
		}

		public static IEnumerable<BigInteger> GetDistinctPrimeFactors(BigInteger value, BigInteger maxValue)
		{
			return GetPrimeFactorization(value, maxValue).Distinct();
		}

		public static IEnumerable<BigInteger> GetPrimeFactorization(BigInteger value, BigInteger maxValue)
		{
			value = BigInteger.Abs(value);

			if (value == 0) { return new BigInteger[] { 0 }; }
			if (value < 10) { if (value == 0 || value == 1 || value == 2 || value == 3 || value == 5 || value == 7) { return new List<BigInteger>() { value }; } }

			BigInteger primeListSize = maxValue + 5;

			EnsurePrimeCacheSize(primeListSize);

			if (primeCache.Contains(value)) { return new List<BigInteger>() { value }; }

			List<BigInteger> factors = new List<BigInteger>();
			foreach (BigInteger prime in primeCache)
			{
				while (value % prime == 0)
				{
					value /= prime;
					factors.Add(prime);
				}

				if (value == 1) break;
			}

			if (value != 1) { factors.Add(value); }

			return factors;
		}

		private static BigInteger[] primeCheckBases = new BigInteger[] { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47 };
		private static int Threshold = 20000;
		public static bool IsProbablePrime(BigInteger input)
		{
			if (input == 2 || input == 3)
			{
				return true;
			}
			if (input < 2 || input % 2 == 0)
			{
				return false;
			}

			EnsurePrimeCacheSize(input + 1);

			if (input < _cacheSize)
			{
				return primeCache.Contains(input);
			}

			BigInteger d = input - 1;
			int s = 0;

			while (d % 2 == 0)
			{
				d >>= 1;
				s += 1;
			}

			double logS = BigInteger.Log(d, 2);
			int logs = (int)logS;

			foreach (BigInteger a in primeCheckBases)
			{
				BigInteger x = BigInteger.ModPow(a, d, input);
				if (x == 1 || x == input - 1)
				{
					continue;
				}

				for (int r = 1; r < s; r++)
				{
					x = BigInteger.ModPow(x, 2, input);
					if (x == 1)
					{
						return false;
					}
					if (x == input - 1)
					{
						break;
					}
				}

				if (x != input - 1)
				{
					return false;
				}
			}
			return true;
		}
	}
}
