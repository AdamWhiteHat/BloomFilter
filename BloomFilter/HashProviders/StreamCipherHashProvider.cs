using System;
using System.Linq;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using BloomFilterCore.Utilities;

namespace BloomFilterCore.HashProviders
{
	[DataContract]
	public class StreamCipherHashProvider : IHashProvider
	{
		[DataMember]
		private int _filterSize;
		[DataMember]
		private int _hashesPerElement;
		[IgnoreDataMember]
		private bool _isDisposed;
		//[IgnoreDataMember]
		//private RC4TableHash _streamCipher;

		public StreamCipherHashProvider()
		{
			_isDisposed = false;
		}

		#region Public Methods

		public void SetParameters(int hashesPerElement, int filterSize)
		{
			_filterSize = filterSize;
			_hashesPerElement = hashesPerElement;
		}

		public void Initialize()
		{
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				_isDisposed = true;

				// Add additional clean-up here.
			}
		}

		public int[] HashElement(string element)
		{
			List<int> results = new List<int>();
			using (RC4TableHash _streamCipher = new RC4TableHash())
			{
				//_streamCipher.Reset(); // Reset the internal state of the stream cipher

				byte[] elementBytes = ByteBits.GetBytes(element);
				_streamCipher.Shuffle(elementBytes.Length);
				_streamCipher.KeySchedule(elementBytes);
				_streamCipher.Shuffle(elementBytes.Length);

				int counter = 0;
				BigInteger grandTotal = 0;
				foreach (byte bite in elementBytes)
				{
					grandTotal += BigInteger.Pow(bite * byte.MaxValue, counter);
					counter++;
				}



				int shuffle;
				counter = 0;
				foreach (byte bite in elementBytes) // Shuffle by some amount that should be unique for the element
				{
					// We are using mod 255 here because it would be too slow if we didn't restrict the rounds of shuffling
					shuffle = (int)BigInteger.ModPow(bite * byte.MaxValue, counter, byte.MaxValue - 1);
					_streamCipher.Shuffle(shuffle);
					counter++;
				}

				// Now, grab some bytes from the stream cipher
				int maxValue = _filterSize - 1;
				int bytesToTake = (int)Math.Ceiling(Math.Log(_filterSize, 2) / 8);

				counter = 0;
				List<byte> bytes;
				while (counter < _hashesPerElement)
				{
					bytes = _streamCipher.GetBytes().Take(bytesToTake).ToList();

					// Pad bytes to a multiple of 4
					while (bytes.Count % 4 != 0)
					{
						bytes.Add(0);
					}

					int hashValue = Math.Abs(BitConverter.ToInt32(bytes.ToArray(), 0));
					if (hashValue > maxValue)
					{
						hashValue = hashValue % maxValue;
					}

					results.Add(hashValue);
					counter++;
				}
			}

			return results.ToArray();
		}

		#endregion

		#region Private Class

		private class RC4TableHash : IDisposable
		{
			public static readonly int TableSize = 256;// Because we are using bytes
			public bool IsDisposed { get; private set; } = true;

			private byte i = 0;
			private byte j = 0;
			private byte l = 0;
			private byte[] _table;
			private int shuffleRounds = 1;
			private byte k { get { unchecked { l = (byte)(_table[i] + _table[j]); return _table[l]; } } }

			private static readonly byte[] NewTable;

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
				IsDisposed = false;
				Reset();
			}

			public byte this[int index]
			{
				get
				{
					CheckDisposed();
					return _table[index];
				}
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

			public void KeySchedule(byte[] key)
			{
				if (key.Length == 0)
				{
					throw new ArgumentException($"Parameter {nameof(key)} must have a length of at least one.");
				}
				j = 0;
				int i = 0;
				int keylength = key.Length;
				unchecked
				{
					while (i < 255)
					{
						j = (byte)((j + _table[i] + key[i % keylength]) % 256);
						swapIandJ();
						i++;
					}
				}
				Shuffle(TableSize + 1);
			}

			public void Shuffle(int rounds)
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

			public void Reset()
			{
				Clear();
				_table = NewTable.ToArray();
				Shuffle(TableSize + 1);
			}

			public void Clear()
			{
				i = 0;
				j = 0;
				l = 0;
				_table = new byte[0];
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

			private void CheckDisposed()
			{
				if (IsDisposed)
				{
					throw new ObjectDisposedException(this.GetType().Name);
				}
			}

			private void swapIandJ()
			{
				l = _table[i];
				_table[i] = _table[j];
				_table[j] = l;
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

		#endregion

	}
}
