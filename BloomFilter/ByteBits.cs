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
			return new BitArray(GetBits(bytes));
		}

		public static bool[] GetBits(byte[] bytes)
		{
			List<byte> copy = new List<byte>(bytes);
			copy.Reverse(); //if (BitConverter.IsLittleEndian) { }
			BitArray bitArray = new BitArray(copy.ToArray());
			
			List<bool> result = bitArray.Cast<bool>().ToList();
			result.Reverse();
			return result.ToArray();
		}

		public static byte[] GetBytes(string Input)
		{
			return Encoding.ASCII.GetBytes(Input);
		}

		public static byte[] GetBytes(BitArray bits)
		{
			bool [] copy = new bool[bits.Count];
			bits.CopyTo(copy, 0);
			Array.Reverse(copy); // Reverse the array

			byte[] result = new byte[(int)Math.Ceiling((double)bits.Length / 8)];
			bits.CopyTo(result, 0);

			// Reverse yet again
			Array.Reverse(result);
			return result;			
		}

		public static byte[] GetBytes(bool[] bits)
		{
			bool[] copy = new List<bool>(bits).ToArray();
			Array.Reverse(copy); // Modifies a copy, not the original array
			BitArray a = new BitArray(copy);
			byte[] output = new byte[a.Length / 8];
			a.CopyTo(output, 0);
			Array.Reverse(output);
			return output;
		}
	}
}