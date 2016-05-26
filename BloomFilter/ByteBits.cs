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
		public static BitArray GetBitArray(byte[] bytes)
		{
			return new BitArray(GetBools(bytes));
		}

		public static bool[] GetBools(byte[] bytes)
		{
			List<byte> copy = new List<byte>(bytes);
			copy.Reverse(); //if (BitConverter.IsLittleEndian) { }
			BitArray bitArray = new BitArray(copy.ToArray());
			
			List<bool> result = bitArray.Cast<bool>().ToList();
			result.Reverse();
			return result.ToArray();
		}

		public static byte[] GetBytes(BitArray bits)
		{
			bool[] copy = new bool[bits.Length];
			bits.CopyTo(copy, 0);
			return GetBytes(copy);
		}

		public static byte[] GetBytes(bool[] bits)
		{
			List<bool> copy = new List<bool>(bits);
			copy.Reverse(); // Modify copy instead of original array

			// Ensure divisible by 4
			//if (copy.Count % 8 != 0)
			//{
				//throw new Exception("copy.Count % 8 != 0");//copy.Add(false);
			//}

			BitArray bitArray = new BitArray(copy.ToArray());
			//byte[] result = new byte[bitArray.Length / 8];

			bitArray.CopyTo(result, 0);			
			Array.Reverse(result);
			return result;
		}
	}
}




			//List<bool> resultList = new List<bool>();			
			//int max = bitArray.Count;
			//int counter = 0;
			//while (counter < max) {
			//	resultList.Add(bitArray[counter]);
			//	counter += 1;
			//} bool[] result = resultList.ToArray();