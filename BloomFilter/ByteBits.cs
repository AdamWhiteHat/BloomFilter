using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BloomFilterCore
{
	public static class ByteBits
	{
		public static bool[] GetBools(byte[] bytes)
		{			
			Array.Reverse(bytes); // Modify copy instead of original array
			BitArray bitArray = new BitArray(bytes);
			List<bool> resultList = new List<bool>();

			int max = bitArray.Count;
			int counter = 0;
			while (counter < max)
			{
				resultList.Add(bitArray[counter]);
				counter += 1;
			}

			bool[] result = resultList.ToArray();
			Array.Reverse(result);

			return result;
		}

		public static byte[] GetBytes(bool[] bits)
		{
			bool[] copy = new bool[bits.Length];
			bits.CopyTo(copy, 0);
			Array.Reverse(copy); // Modify copy instead of original array
			BitArray bitArray = new BitArray(copy);
			byte[] result = new byte[bitArray.Length / 8];
			bitArray.CopyTo(result, 0);
			Array.Reverse(result);
			return result;
		}
	}
}
