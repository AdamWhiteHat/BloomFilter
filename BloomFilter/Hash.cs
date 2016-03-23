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
		public int TableSize { get; private set; }
		public bool IsDisposed { get; private set; }

		private byte i;
		private byte j;
		private byte l;
		private byte[] _table;
		private byte k { get { unchecked { l = (byte)(_table[i] + _table[j]); return _table[l]; } } }

		public Hash()
		{
			Clear();
			TableSize = 256; 	// Because we are using bytes
			int counter = TableSize;
			List<byte> result = new List<byte>();
			while (counter-- > 0)
			{
				result.Add((byte)(255 - counter));
			}
			_table = result.ToArray();
			IsDisposed = false;
		}

		public Hash(int coPrime)
			: this(256, coPrime)
		{
		}

		public Hash(int tableSize, int coPrime)
		{
			if (!Coprimes.IsCoprime(tableSize, coPrime))
			{
				throw new ArgumentException(string.Format("coPrime must be co-prime to tableSize: {0}", tableSize));
			}

			Clear();
			TableSize = tableSize;

			int counter = 0;
			int val = coPrime % tableSize;
			List<byte> result = new List<byte>();
			while (counter < 255)
			{
				val = val + coPrime;
				if (val > TableSize)
				{
					val = val % TableSize;
				}
				result.Add((byte)(val));
				counter += 1;
			}
			_table = result.ToArray();
			IsDisposed = false;
		}

		public byte this[int index]
		{
			get
			{
				return _table[index];
			}
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

		protected void Shuffle(int rounds)
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

		protected void Clear()
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
