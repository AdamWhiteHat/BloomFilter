using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomFilterCore.Hashes
{
	public class RC4TableHash : IDisposable
	{
		public static int TableSize = 256;// Because we are using bytes
		public bool IsDisposed { get; private set; }

		private byte i;
		private byte j;
		private byte l;
		private byte[] _table;
		private int shuffleRounds = 1;
		private byte k { get { unchecked { l = (byte)(_table[i] + _table[j]); return _table[l]; } } }

		private static byte[] NewTable;

		static RC4TableHash()
		{
			int counter = TableSize;
			List<byte> result = new List<byte>();
			while (counter-- > 0)
			{
				result.Add((byte)(255 - counter));
			}
			NewTable = result.ToArray();
		}

		public RC4TableHash()
		{		
			Clear();
			_table = NewTable;
			IsDisposed = false;
			Shuffle(TableSize+1);
		}

		public byte this[int index]
		{
			get
			{
				CheckDisposed();
				return _table[index];
			}
		}
		
		public byte[] GetBytes(int quantity)
		{
			return GetBytes().Take(quantity).ToArray();
		}

		public IEnumerable<byte> GetBytes()
		{
			CheckDisposed();
			while (!IsDisposed)
			{
				Shuffle(shuffleRounds);
				yield return k;
			}
			yield break;
		}

		protected void Shuffle(int rounds)
		{
			CheckDisposed();

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
		
		protected void CheckDisposed()
		{
			if (IsDisposed) 
			{ 
				throw new ObjectDisposedException(this.GetType().Name);
			}
		}

		public void Dispose()
		{
			if (!IsDisposed)
			{
				Clear();
				IsDisposed = true;
			}
		}

		public override string ToString()
		{
			CheckDisposed();

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
	}
}
