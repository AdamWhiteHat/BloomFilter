using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace BloomFilterCore.Utilities
{
	public class CryptoRandom : IDisposable
	{
		private bool IsDisposed = false;
		private byte[] rngBytes8 = new byte[8];
		private byte[] rngBytes4 = new byte[4];
		private RNGCryptoServiceProvider _crng;

		public CryptoRandom()
		{
			disposeCheck();
			_crng = new RNGCryptoServiceProvider();
		}

		public void Dispose()
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				if (_crng != null)
				{
					_crng.Dispose();
					_crng = null;
				}

				if (rngBytes8 != null)
				{
					ClearBuffer(rngBytes8);
					rngBytes8 = null;
				}

				if (rngBytes4 != null)
				{
					ClearBuffer(rngBytes4);
					rngBytes4 = null;
				}
			}
		}

		private void disposeCheck()
		{
			if (IsDisposed)
			{
				throw new ObjectDisposedException("CryptoRandom");
			}
		}

		private static void ClearBuffer(byte[] buffer)
		{
			if (buffer != null)
			{
				int counter = buffer.Length - 1;
				while (counter >= 0)
				{
					buffer[counter] = byte.MinValue;
					counter--;
				}
				counter = 0;
			}
		}

		public byte[] NextBytes(int quantity)
		{
			disposeCheck();
			byte[] result = new byte[quantity];
			_crng.GetBytes(result);
			return result;
		}

		public int Next(int maxValue)
		{
			disposeCheck();
			return Next(0, maxValue);
		}

		public int Next(int lower, int upper)
		{
			disposeCheck();
			if (lower > upper) { throw new ArgumentOutOfRangeException($"{nameof(upper)} must be greater than {nameof(lower)}"); }

			int range;
			while (true)
			{
				range = lower + (int)((upper - lower) * NextDouble());
				if (range >= lower && range <= upper)
				{
					return range;
				}
			}
		}

		public double NextDouble()
		{
			disposeCheck();
			_crng.GetBytes(rngBytes8);
			ulong u64 = BitConverter.ToUInt64(rngBytes8, 0);
			double result = (double)((u64 + 1.0) * 5.421010862427522E-20);
			ClearBuffer(rngBytes8);
			return result;
		}
	}
}
