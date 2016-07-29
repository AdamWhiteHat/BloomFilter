using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace BloomFilterCore.Hashes
{
	public static class MD5Hash
	{
		public static int[] GetIndices(byte[] array, int indicesToReturn, int maxIndex)
		{
			List<int> result = new List<int>();

			// Will be filled with an array of 16 bytes
			List<byte> hash = new List<byte>();
			using (MD5 md5Hash = MD5.Create())
			{
				hash.AddRange(md5Hash.ComputeHash(array));
			}

			if(indicesToReturn > hash.Count)
			{
				throw new Exception("hash byte array length less than indicesToReturn");
			}

			IEnumerable<byte> temp = hash.Take(indicesToReturn);

			UInt32 roundAt = (UInt32)maxIndex;
			unchecked
			{
				UInt32 counter = 0;
				foreach (byte b in temp)
				{
					counter++;
					UInt64 product = b * counter * PrimeNumbers.UInt32Prime;
					UInt32 outValue = (UInt32)product % roundAt;
					result.Add((int)outValue);
				}
			}
			

			return result.ToArray();
		}
	}

	public static class PrimeHash64
	{
		public static int[] GetIndices(UInt64 prime, byte[] array, int indicesToReturn, Int32 maxIndex)
		{
			if (prime > ((UInt64.MaxValue / (ulong)255) / (ulong)array.Length)) //72340172838076673/array.Length
			{
				throw new ArgumentOutOfRangeException("prime", "prime * array.Length * 255 must equal less than UInt64.MaxValue");
			}

			UInt64 counter = 0;
			UInt64 divisor = (UInt64)maxIndex;
			List<int> result = new List<int>();
			unchecked
			{
				UInt64 product = 0;
				UInt32 index = 0;
				foreach (byte b in array)
				{
					product = b + counter;
					product *= prime;
					index = (UInt32)(product % divisor);
					result.Add((Int32)index);
					counter++;
				}
			}
			return result.ToArray();
		}
	}





	public static class PrimeNumbers
	{
		public static UInt32 UInt32Prime = 3213216389;

		public static UInt64[] PrimeDoubleArray = new UInt64[] 
		{
			2, 5, 11,
			23, 47,	97,
			197, 397, 797,
			1597, 3203, 6421,
			12853, 25717, 51437,
			102877, 205759, 411527,
			823117, 1646237, 3292489,
			6584983, 13169977, 26339969,
			52679969, 105359939, 210719881,
			421439783, 842879579, 1685759167,
			3371518343, 6743036717, 13486073473,
			26972146961, 53944293929, 107888587883,
			215777175787, 431554351609, 863108703229,
			1726217406467, 3452434812973, 6904869625999,
			13809739252051, 27619478504183, 55238957008387,
			110477914016779, 220955828033581, 441911656067171,
			883823312134381, 1767646624268779, 3535293248537579,
			7070586497075177, 14141172994150357, 28282345988300791,
			56564691976601587, 113129383953203213, 226258767906406483,
			452517535812813007, 905035071625626043,	1810070143251252131,
			3620140286502504283, 7240280573005008577, 14480561146010017043
		};
	}
}
