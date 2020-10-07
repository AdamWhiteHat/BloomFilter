using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomFilterCore
{
	public class ByteGenerator
	{
		public class SequenceGenerator
		{
			private CryptoRandom _rand;
			private static byte zeroByte = 0;
			private int increment = 1;
			private int counter = 0;
			private int size = 4;
			private int digitSize = 10000;

			public SequenceGenerator(int wordSize, int startValue = 0, int incrementValue = 1)
			{
				_rand = new CryptoRandom();
				size = wordSize;
				counter = startValue;
				increment = incrementValue;
				digitSize = (int)Math.Pow(10, size);
			}

			public string GetNext()
			{				
				counter += increment;
				return Math.Abs(_rand.Next(digitSize)).ToString().PadLeft(size, '1');
			}
		}

		public class RandomGenerator
		{
			private CryptoRandom _rand;
			private int counter = 0;
			public int Size { get; private set; }

			public RandomGenerator()
				: this(4)
			{
			}

			public RandomGenerator(int size)
			{
				Size = size;
				_rand = new CryptoRandom();
			}

			public string GetNext()
			{
				counter++;
				if (counter > 3000)
				{
					Size++;
					counter = 0;
				}
				return Encoding.UTF8.GetString(_rand.NextBytes(Size));
			}
		}

		private string GetRepeat(int wordSize, int value = 0)
		{
			byte[] result = Enumerable.Repeat((byte)value, wordSize).ToArray();
			return ByteBits.GetString(result);
		}
	}
}
