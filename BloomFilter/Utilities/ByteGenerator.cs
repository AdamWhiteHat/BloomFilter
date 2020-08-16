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
			private static byte zeroByte = 0;
			private int increment = 1;
			private int counter = 0;
			private int size = 4;

			public SequenceGenerator(int wordSize, int startValue = 0, int incrementValue = 1)
			{
				size = wordSize;
				counter = startValue;
				increment = incrementValue;
			}

			public string GetNext()
			{
				counter += increment;
				return counter.ToString().PadLeft(size, (char)zeroByte);
			}
		}

		public class RandomGenerator
		{
			private CryptoRandom rand;
			private int counter = 0;
			public int Size { get; private set; }

			public RandomGenerator()
			{
				Size = 1;
				rand = new CryptoRandom();
			}

			public string GetNext()
			{
				counter++;
				if (counter > 3000)
				{
					Size++;
					counter = 0;
				}
				return Encoding.UTF8.GetString(rand.NextBytes(Size));
			}
		}

		private string GetRepeat(int wordSize, int value = 0)
		{
			byte[] result = Enumerable.Repeat((byte)value, wordSize).ToArray();
			return ByteBits.GetString(result);
		}
	}
}
