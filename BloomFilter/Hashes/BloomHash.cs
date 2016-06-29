using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomFilterCore.Hashes
{
	public class BloomHash : RC4TableHash
	{
		private static int maxIndex;
		private static int quantityIndexBytes;
		
		public BloomHash(string input, int indexByteSize, int maximumIndex)
			: base()
		{
			maxIndex = maximumIndex;
			quantityIndexBytes = indexByteSize;
						
			int counter = 0;
			int shuffle = 0;

			byte[] inputBytes = ByteBits.GetBytes(input);
			byte[] mixedBytes = BitShuffle.Interleave(inputBytes);
			mixedBytes = BitShuffle.Interleave(inputBytes);
			mixedBytes = BitShuffle.Interleave(inputBytes);
			mixedBytes = BitShuffle.Interleave(inputBytes);

			Shuffle(mixedBytes.Length);
			foreach (byte bite in mixedBytes)
			{
				shuffle = ((bite * counter) - counter ) + 1;
				Shuffle(shuffle);
				counter++;
			}
		}

		public IEnumerable<int> GetIndices()
		{
			CheckDisposed();
			List<byte> bytes;
			while (!IsDisposed)
			{
				bytes = this.GetBytes().Take(quantityIndexBytes).ToList();
				
				// Pad bytes to a multiple of 4
				while (bytes.Count % 4 != 0) 
				{
					bytes.Add(0);
				}

				int result = Math.Abs(BitConverter.ToInt32(bytes.ToArray(), 0));
				if (result > maxIndex)
				{
					result = result % maxIndex;
				}
				yield return result;
			}
			yield break;
		}
	}
}
