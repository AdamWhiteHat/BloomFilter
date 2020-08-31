using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;

namespace BloomFilterCore
{
	public static class PrimeHelper
	{
		private static int _probablePrimeThreshold;
		private static BigInteger[] _probablePrimeCheckBases;

		private static BigInteger _cacheCeiling;
		private static BigInteger _cacheLargestPrimeCurrently;
		private static List<BigInteger> _primesCache;

		static PrimeHelper()
		{
			_probablePrimeThreshold = 20000;
			_cacheCeiling = BigInteger.Pow(10, 7);
			_primesCache = new List<BigInteger> { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47 };
			_primesCache = GetPrimes(_probablePrimeThreshold);
			_cacheLargestPrimeCurrently = _primesCache.Last();
			_probablePrimeCheckBases = _primesCache.Take(15).ToArray();
		}

		public static void Initialize()
		{ }

		private static void EnsurePrimeCacheSize(BigInteger maxPrime)
		{
			BigInteger boundedPrimeRequest = BigInteger.Min(maxPrime, _cacheCeiling);
			if (_cacheLargestPrimeCurrently < boundedPrimeRequest)
			{
				_primesCache = GetPrimes(boundedPrimeRequest);
				_cacheLargestPrimeCurrently = _primesCache.Last();
			}
		}

		public static IEnumerable<BigInteger> GetPrimeFactorization(BigInteger value)
		{
			value = BigInteger.Abs(value);
			BigInteger maxValue = value.SquareRoot() + 1;

			if (value < 10) { if (value == 0 || value == 1 || value == 2 || value == 3 || value == 5 || value == 7) { return new List<BigInteger>() { value }; } }

			EnsurePrimeCacheSize(maxValue);

			if (_primesCache.Contains(value)) { return new List<BigInteger>() { value }; }

			List<BigInteger> factors = new List<BigInteger>();
			foreach (BigInteger prime in _primesCache)
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

		public static List<BigInteger> GetPrimes(BigInteger ceiling)
		{
			BigInteger floor = 2;
			ceiling = BigInteger.Min(ceiling, Int32.MaxValue - 2);

			if (floor > ceiling)
			{
				throw new ArgumentOutOfRangeException("floor > ceiling");
			}
			if (ceiling < 10)
			{
				if (ceiling == 9 || ceiling == 8 || ceiling == 7)
				{
					return new List<BigInteger>() { 2, 3, 5, 7 };
				}
				else if (ceiling == 6 || ceiling == 5)
				{
					return new List<BigInteger>() { 2, 3, 5 };
				}
				else if (ceiling == 4 || ceiling == 3)
				{
					return new List<BigInteger>() { 2, 3 };
				}
				else
				{
					return new List<BigInteger>() { 2 };
				}
			}

			if (_primesCache.Count > 0 && _cacheLargestPrimeCurrently >= ceiling)
			{
				return _primesCache.SkipWhile(i => floor > i).TakeWhile(i => ceiling >= i).ToList();
			}

			BigInteger counter = 0;
			BigInteger counterStart = 3;
			BigInteger inc;
			BigInteger sqrt = 3;

			bool[] primeMembershipArray = new bool[(int)ceiling + 1];

			primeMembershipArray[2] = true;

			// Set all odds as true
			for (counter = counterStart; counter <= ceiling; counter += 2)
			{
				if ((counter & 1) == 1) // Check if odd
				{
					primeMembershipArray[(int)counter] = true;
				}
			}

			while (sqrt * sqrt <= ceiling)
			{
				counter = sqrt * sqrt;
				inc = sqrt + sqrt;

				while (counter <= ceiling)
				{
					primeMembershipArray[(int)counter] = false;
					counter += inc;
				}

				sqrt += 2;

				while (!primeMembershipArray[(int)sqrt])
				{
					sqrt++;
				}
			}

			//var maxRange = ceiling.SquareRoot();

			IEnumerable<BigInteger> primesCollection = Range.GetRange(2, ceiling - 1).Where(n => primeMembershipArray[(int)(n)] == true).ToList();

			if (primesCollection.Last() > _cacheLargestPrimeCurrently)
			{
				_primesCache = primesCollection.ToList();
			}

			List<BigInteger> result = primesCollection.Where(l => l >= floor && l <= ceiling).ToList();
			return result;
		}

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

			if (input < _cacheLargestPrimeCurrently)
			{
				return _primesCache.Contains(input);
			}

			BigInteger d = input - 1;
			int s = 0;

			while (d % 2 == 0)
			{
				d >>= 1;
				s += 1;
			}

			foreach (BigInteger a in _probablePrimeCheckBases)
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

		public static BigInteger GetPreviousPrime(BigInteger fromValue)
		{
			BigInteger result = fromValue.IsEven ? fromValue - 1 : fromValue - 2;

			while (result > 0)
			{
				if (PrimeHelper.IsProbablePrime(result))
				{
					return result;
				}
				result -= 2;
			}

			throw new Exception($"No primes exist between {fromValue} and zero.");
		}
	}
}
