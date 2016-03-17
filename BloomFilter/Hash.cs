using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomFilterCore
{
	public class Hash : IDisposable
	{
		public int TableSize = 256;	// Because we are using bytes
		public bool IsDisposed { get; private set; }
	
		private static int maxIndex;
		private static int quantityIndexBytes;

		private byte i;
		private byte j;
		private byte l;
		private byte[] _table;
		private byte k { get { unchecked { l = (byte)(_table[i] + _table[j]); return _table[l]; } } }

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
				
		public Hash(string input, int indexByteSize, int maximumIndex)
		{
			token = input;
			maxIndex = maximumIndex;
			quantityIndexBytes = indexByteSize;
			Initialize();

			int prime = 1;
			byte[] inputBytes = Encoding.ASCII.GetBytes(input);
			foreach (byte bite in inputBytes)
			{
				prime = (prime * 2) + 1;
				Shuffle(bite * prime);
			}
			//Shuffle(TableSize); //Shuffle(input.Length);
		}

		private void Initialize()
		{
			Clear();
			int counter = TableSize;
			List<byte> result = new List<byte>();
			while (counter-- > 0)
			{
				result.Add((byte)(255 - counter));
			}
			_table = result.ToArray();
			IsDisposed = false;
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

		public byte[] GetBytes(int quantity)
		{
			return GetBytes().Take(quantity).ToArray();
		}

		public IEnumerable<byte> GetBytes()
		{
			while (!IsDisposed)
			{
				Shuffle(TableSize);
				yield return k;
			}
			yield break;
		}

		private void Shuffle(int rounds)
		{
			if (IsDisposed) { throw new ObjectDisposedException(this.GetType().Name); }

			int counter = rounds;
			unchecked // Just roll over on overflow. This is essentially mod 256, since everything is a byte
			{
				while (counter-- > 0)
				{
					i++;
					j = (byte)(j + _table[i]);
					swapIandJ();
				}
			}
		}

		public override string ToString()
		{
			if (IsDisposed) { throw new ObjectDisposedException(this.GetType().Name); }

			int m = 0; int n = 0;
			StringBuilder result = new StringBuilder();
			while (m < 256)
			{
				n = 0;
				while (n < 16)
				{
					if (n != 0)
					{
						result.Append(' ');
					}
					result.Append(string.Format("{0,3}", _table[m + n]));
					n++;
				}
				result.AppendLine();
				result.AppendLine();
				m += 16;
			}
			return result.ToString();
		}

		private void swapIandJ()
		{
			l = _table[i];
			_table[i] = _table[j];
			_table[j] = l;
		}

		private void Clear()
		{
			i = 0;
			j = 0;
			l = 0;
			_table = null;
		}

		public void Dispose()
		{
			if (!IsDisposed)
			{
				Clear();
				IsDisposed = true;
			}
		}
	}
}
