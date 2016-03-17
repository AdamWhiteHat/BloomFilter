using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BloomFilterCore
{
	[StructLayout(LayoutKind.Explicit)]
	public struct ByteBits
	{
		[FieldOffset(0)]
		public byte Value;

		[FieldOffset(0)]
		public bool Bit01;

		[FieldOffset(1)]
		public bool Bit02;

		[FieldOffset(2)]
		public bool Bit03;

		[FieldOffset(3)]
		public bool Bit04;

		[FieldOffset(4)]
		public bool Bit05;

		[FieldOffset(5)]
		public bool Bit06;

		[FieldOffset(6)]
		public bool Bit07;

		[FieldOffset(7)]
		public bool Bit08;

		public ByteBits(byte value)
		{
			this.Value = value;

			BitArray bits = new BitArray(this.Value);
			Bit01 = bits.Get(0);
			Bit02 = bits.Get(1);
			Bit03 = bits.Get(2);
			Bit04 = bits.Get(3);
			Bit05 = bits.Get(4);
			Bit06 = bits.Get(5);
			Bit07 = bits.Get(6);
			Bit08 = bits.Get(7);
		}

		public bool[] GetBits()
		{
			return new bool[] { Bit01, Bit02, Bit03, Bit04, Bit05, Bit06, Bit07, Bit08 };
		}

		//public static implicit operator ByteBits(byte value) { return new ByteBits(value); }
		public static implicit operator byte(ByteBits value)
		{
			return value.Value;
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
