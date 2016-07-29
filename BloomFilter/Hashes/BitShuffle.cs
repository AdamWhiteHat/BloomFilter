using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomFilterCore
{
	public class BitShuffle
	{		
		public static Tuple<bool[], bool[]> SplitInput(byte[] Input)
		{
			// Convert input into an array of bits
			List<bool> bits = ByteBits.GetBits(Input).ToList();

			// Ensure we have an even number of bits
			if (bits.Count % 2 != 0)
			{
				bits.Add(false);
			}

			int midpoint = (bits.Count / 2);

			bool[] one = bits.Take(midpoint).ToArray();
			bool[] two = bits.Skip(midpoint).ToArray();
			
			// If the first bit of two is 1, reverse it.
			//   This provides more deterministic shuffling
			if (two[0])
			{
				Array.Reverse(two);
			}

			return new Tuple<bool[], bool[]>(one, two);
		}
		
		// Interleave two arrays of bool
		public static byte[] Interleave(byte[] Input)
		{
			Tuple<bool[], bool[]> halfs = SplitInput(Input);

			bool[] one = halfs.Item1;
			bool[] two = halfs.Item2;			
			
			// If the 'middle' bit of one is 1, swap the arrays
			if (one[one.Length / 2])
			{
				one = two;
				two = halfs.Item1;
			}

			IEnumerable<bool> result = one.Zip(two, (o, t) => new bool[] { o, t }).SelectMany(b => b);
			return ByteBits.GetBytes(result.ToArray());
		}

		// Swap bit source after three bits
		public static byte[] FlipEveryNBit(byte[] Input, int n)
		{
			byte[] interleaved = Interleave(Input);
			bool[] bits = ByteBits.GetBits(interleaved);
						
			int counter = 0;
			List<bool> result = new List<bool>();
			foreach(bool bit in bits)
			{
				result.Add(counter++ % n == 0 ? !bit : bit);				
			}

			return ByteBits.GetBytes(result.ToArray());
		}
	}
}
