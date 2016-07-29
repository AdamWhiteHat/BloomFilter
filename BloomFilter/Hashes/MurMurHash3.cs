/*
 * 
 * Thanks to Adam Horvath for the algorithm
 * -->  http://blog.teamleadnet.com/2012/08/murmurhash3-ultra-fast-hash-algorithm.html
 *
 */

namespace BloomFilterCore.Hashes
{
	using System;

	public static class MurMurHash3
	{
		// 128 bit output, 64 bit platform version
		private static readonly ulong READ_SIZE = 16;
		private static readonly ulong C1 = 0x87c37b91114253d5L;
		private static readonly ulong C2 = 0x4cf5ad432745937fL;

		public static byte[] ComputeHash(byte[] byteArray, uint seed)
		{
			ulong k1 = 0;
			ulong k2 = 0;
			ulong h1 = seed;
			ulong h2 = 0;
			ulong length = 0L;
			
			int pos = 0;
			ulong remaining = (ulong)byteArray.Length;

			// Read 128 bits, 16 bytes, 2 longs in each cycle
			while (remaining >= READ_SIZE)
			{
				k1 = GetUInt64(byteArray, pos);
				pos += 8;

				k2 = GetUInt64(byteArray, pos);
				pos += 8;

				length += READ_SIZE;
				remaining -= READ_SIZE;

				h1 ^= MixKey1(k1);

				h1 = RotateLeft(h1, 27);
				h1 += h2;
				h1 = h1 * 5 + 0x52dce729;

				h2 ^= MixKey2(k2);

				h2 = RotateLeft(h2, 31);
				h2 += h1;
				h2 = h2 * 5 + 0x38495ab5;
			}

			// If the input MOD 16 != 0
			if (remaining > 0)
			{
				k1 = 0;
				k2 = 0;
				length += remaining;

				// Little endian (x86) processing
				switch (remaining)
				{
					case 15:
						k2 ^= (ulong)byteArray[pos + 14] << 48; // Fall through
						goto case 14;
					case 14:
						k2 ^= (ulong)byteArray[pos + 13] << 40; // Fall through
						goto case 13;
					case 13:
						k2 ^= (ulong)byteArray[pos + 12] << 32; // Fall through
						goto case 12;
					case 12:
						k2 ^= (ulong)byteArray[pos + 11] << 24; // Fall through
						goto case 11;
					case 11:
						k2 ^= (ulong)byteArray[pos + 10] << 16; // Fall through
						goto case 10;
					case 10:
						k2 ^= (ulong)byteArray[pos + 9] << 8; // Fall through
						goto case 9;
					case 9:
						k2 ^= (ulong)byteArray[pos + 8]; // Fall through
						goto case 8;
					case 8:
						k1 ^= GetUInt64(byteArray, pos);
						break;
					case 7:
						k1 ^= (ulong)byteArray[pos + 6] << 48; // Fall through
						goto case 6;
					case 6:
						k1 ^= (ulong)byteArray[pos + 5] << 40; // Fall through
						goto case 5;
					case 5:
						k1 ^= (ulong)byteArray[pos + 4] << 32; // Fall through
						goto case 4;
					case 4:
						k1 ^= (ulong)byteArray[pos + 3] << 24; // Fall through
						goto case 3;
					case 3:
						k1 ^= (ulong)byteArray[pos + 2] << 16; // Fall through
						goto case 2;
					case 2:
						k1 ^= (ulong)byteArray[pos + 1] << 8; // Fall through
						goto case 1;
					case 1:
						k1 ^= (ulong)byteArray[pos]; // Fall through
						break;
					default:
						throw new Exception("Something went wrong with remaining bytes calculation.");
				}

				h1 ^= MixKey1(k1);
				h2 ^= MixKey2(k2);
			}

			h1 ^= length;
			h2 ^= length;

			h1 += h2;
			h2 += h1;

			h1 = MixFinal(h1);
			h2 = MixFinal(h2);

			h1 += h2;
			h2 += h1;

			byte[] result = new byte[READ_SIZE];

			Array.Copy(BitConverter.GetBytes(h1), 0, result, 0, 8);
			Array.Copy(BitConverter.GetBytes(h2), 0, result, 8, 8);

			return result;
		}

		private static ulong MixKey1(ulong k1)
		{
			k1 *= C1;
			k1 = RotateLeft(k1, 31);
			k1 *= C2;
			return k1;
		}

		private static ulong MixKey2(ulong k2)
		{
			k2 *= C2;
			k2 = RotateLeft(k2, 33);
			k2 *= C1;
			return k2;
		}

		private static ulong MixFinal(ulong k)
		{
			// Avalanche bits
			k ^= k >> 33;
			k *= 0xff51afd7ed558ccdL;
			k ^= k >> 33;
			k *= 0xc4ceb9fe1a85ec53L;
			k ^= k >> 33;
			return k;
		}

		private static ulong RotateLeft(ulong value, int bits)
		{
			return (value << bits) | (value >> (64 - bits));
		}

		private static ulong RotateRight(ulong value, int bits)
		{
			return (value >> bits) | (value << (64 - bits));
		}

		private static ulong GetUInt64(byte[] byteArray, int startIndex)
		{
			return BitConverter.ToUInt64(byteArray, startIndex);
		}

		/* Uncomment the below and use it instead of the above for better performance,
		 * at the cost of having to compile for unsafe code */
		//unsafe private static ulong GetUInt64(byte[] byteArray, int pos)
		//{
		//	// We only read aligned longs, so a simple casting is enough
		//	fixed (byte* pbyte = &byteArray[pos])
		//	{
		//		return *((ulong*)pbyte);
		//	}
		//}
	}
}
