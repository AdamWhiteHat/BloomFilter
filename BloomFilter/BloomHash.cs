using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomFilterCore
{
	public class BloomHash : Hash
	{

		private static int maxIndex;
		private static int quantityIndexBytes;

		private string token;		
		private int _tokCntr = 0;
		private int tokenCounter { get { return ++_tokCntr; } }

		private int _tokIndx = -1;
		private int tokenIndex
		{
			get
			{
				_tokIndx += 1;
				if (_tokIndx > token.Length - 1)
				{
					_tokIndx = 0;
				}
				return _tokIndx;
			}
		}

		public BloomHash(string input, int indexByteSize, int maximumIndex)
			: base()
		{
			token = input;
			maxIndex = maximumIndex;
			quantityIndexBytes = indexByteSize;

			int prime = 1;
			byte[] inputBytes = Encoding.ASCII.GetBytes(input);
			foreach (byte bite in inputBytes)
			{
				prime = (prime * 2) + 1;
				Shuffle(bite * prime);
			}
			//Shuffle(TableSize); //Shuffle(input.Length);
		}

		public IEnumerable<int> GetIndices()
		{
			int shiftAmmount = 0;
			List<byte> bytes;
			while (!IsDisposed)
			{
				shiftAmmount = token[tokenIndex] * tokenCounter;
				Shuffle(shiftAmmount); // Shuffle by an amount that is unique to the token thats being hashed
				bytes = GetBytes().Take(quantityIndexBytes).ToList();
				if (bytes.Count == 3) { bytes.Add(0); }
				yield return Math.Abs(BitConverter.ToInt32(bytes.ToArray(), 0)) % maxIndex;
			}
			yield break;
		}
	}
}
