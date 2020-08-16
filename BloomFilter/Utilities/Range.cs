using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace BloomFilterCore
{
	public static class Range
	{
		public static IEnumerable<BigInteger> GetRange(BigInteger min, BigInteger max)
		{
			BigInteger counter = max;
			BigInteger currentValue = min;
			while (counter-- > 0)
			{
				yield return currentValue;
				currentValue += 1;
			}
			yield break;
		}
	}
}
