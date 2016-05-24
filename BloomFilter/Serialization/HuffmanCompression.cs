using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomFilterCore.Serialization
{
	public class HuffmanCompression
	{
		public static byte[] Encode(IEnumerable<bool> bits)
		{
			if (bits == null || bits.Count() < 1) { return new byte[] {}; }
			List<byte> result = new List<byte>();

			byte counter = 1;
			bool lastBit = bits.First();
			foreach (bool bit in bits)
			{
				if (counter < 254 && lastBit.Equals(bit))
				{
					counter++;
					continue;
				}

				result.Add(counter);
				result.Add(lastBit ? (byte)1 : (byte)0);

				lastBit = bit;
				counter = 1;
			}
			result.Add(counter);
			result.Add(lastBit ? (byte)1 : (byte)0);

			return result.ToArray();
		}

		public static bool[] Decode(IEnumerable<byte> bytes)
		{
			if (bytes == null || bytes.Count() < 1) { return new bool[] { }; }
			List<bool> result = new List<bool>();

			byte counter = 0;
			foreach (byte bite in bytes)
			{
				if (counter == 0)
				{
					counter = bite;
					continue;
				}

				result.AddRange(Enumerable.Repeat(bite==1 ? true : false, counter));
				counter = 0;
			}		

			return result.ToArray();
		}
	}
}
