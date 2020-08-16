using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace BloomFilterCore
{
	public static class BigIntegerExtensionMethods
	{
		public static BigInteger Clone(this BigInteger number)
		{
			return new BigInteger(number.ToByteArray());
		}

		public static BigInteger Mod(this BigInteger n, BigInteger mod)
		{
			BigInteger r = (n >= mod) ? n % mod : n;
			return (r < 0) ? BigInteger.Add(r, mod) : r.Clone();
		}

		public static BigInteger SquareRoot(this BigInteger input)
		{
			if (input.IsZero) return new BigInteger(0);

			BigInteger n = new BigInteger(0);
			BigInteger p = new BigInteger(0);
			BigInteger low = new BigInteger(0);
			BigInteger high = BigInteger.Abs(input);

			while (high > low + 1)
			{
				n = (high + low) >> 1;
				p = n * n;

				if (input < p)
				{ high = n; }
				else if (input > p)
				{ low = n; }
				else
				{ break; }
			}
			return input == p ? n : low;
		}
	}
}
