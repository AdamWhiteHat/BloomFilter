using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace BloomFilterCore
{

	public static class PrimeFactory
	{
		//static PrimeFactory() { _primesCache = GetPrimes(1000000).ToArray(); }
		internal static BigInteger[] _primesCache = new BigInteger[] { 2, 3, 5, 7, 11, 13 };

		public static bool IsPrime(BigInteger p)
		{
			return _primesCache.Contains(BigInteger.Abs(p));
		}

		private static void IncreaseMaxValue(int newMaxValue = 0)
		{
			// Increase bound
			int currentValue = _primesCache.Any() ? (int)_primesCache.Last() : 0;
			int MaxValue = Math.Max(newMaxValue + 1000, currentValue + 100000);
			_primesCache = GetPrimes(MaxValue).ToArray();
		}

		public static List<BigInteger> GetPrimes(BigInteger ceiling)
		{
			return GetPrimes(2, ceiling);
		}

		public static List<BigInteger> GetPrimes(BigInteger floor, BigInteger ceiling)
		{
			if (floor < 2)
			{
				floor = 2;
			}

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

			if (_primesCache.Length > 0 && _primesCache.Last() >= ceiling)
			{
				return _primesCache.SkipWhile(i => floor > i).TakeWhile(i => ceiling >= i).ToList();
			}

			BigInteger counter = 0;
			BigInteger counterStart = 3;
			BigInteger inc;
			BigInteger sqrt = 3;
			BigInteger ceil = ceiling > Int32.MaxValue ? Int32.MaxValue - 2 : ceiling;

			bool[] primeMembershipArray = new bool[(int)ceil + 1];

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

			var maxRange = ceiling.SquareRoot();

			IEnumerable<BigInteger> primesCollection = Range.GetRange(2, ceiling - 1).Where(n => primeMembershipArray[(int)(n)] == true).ToList();

			if (primesCollection.Count() > _primesCache.Count())
			{
				_primesCache = primesCollection.ToArray();
			}

			List<BigInteger> result = primesCollection.Where(l => l >= floor && l <= ceiling).ToList();
			return result;
		}

		public static BigInteger GetNextPrime(BigInteger fromValue)
		{
			BigInteger result = fromValue + 1;
			if (result.IsEven)
			{
				result += 1;
			}

			while (!Factorization.IsProbablePrime(result))
			{
				result += 2;
			}

			return result;
		}

		public static BigInteger GetPreviousPrime(BigInteger fromValue)
		{
			BigInteger result = fromValue.IsEven ? fromValue - 1 : fromValue;

			while (!Factorization.IsProbablePrime(result))
			{
				result -= 2;
			}

			return result;
		}
	}
}
