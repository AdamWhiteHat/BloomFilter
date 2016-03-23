using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BloomFilterCore
{
	public class Coprimes
	{
		public int Number { get; private set; }
		public int Min { get; private set; }
		public int Max { get; private set; }
		

		private int counter;

		public Coprimes(int number, int min, int max)
		{
			Number = number;
			Min = min;
			Max = max;

			counter = Min;
		}

		public IEnumerable<int> GetCoprimes()
		{
			if (Max < 2 || Min < 2 || Max <= Min || Number < 1) { yield break; }

			while (counter < Max)
			{
				if (IsCoprime(Number, counter))
				{
					yield return counter;
				}
				counter++;
			}
			yield break;
		}

		public static bool IsCoprime(int value1, int value2)
		{
			return FindGCD(value1, value2) == 1;
		}

		public static int FindGCD(int value1, int value2)
		{
			while (value1 != 0 && value2 != 0)
			{
				if (value1 > value2)
				{
					value1 %= value2;
				}
				else
				{
					value2 %= value1;
				}
			}
			return Math.Max(value1, value2);
		}

		public static int FindLCM(int num1, int num2)
		{
			return (num1 * num2) / FindGCD(num1, num2);
		}
	}
}
