using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomFilterCore
{
	public partial class BloomFilter
	{
		public Int32 MaxElements;
		public int SizeBits { get { return MaxElements; } }
		public int SizeBytes { get { return SizeBits / 8; } }
		public int SizeKB { get { return SizeBits / 8192; } }
		public int SizeMB { get { return SizeBits / 8388608; } }
		public double IndexBitSize { get { return Math.Log(SizeBits, 2); } }
		public int IndexByteSize { get { return (int)Math.Ceiling(IndexBitSize / 8); } }
		public Int32 ElementsHashed { get; private set; }
		public Int32 HashesPerToken { get; private set; }

		internal BitArray filterArray;
		private int samples = 100;

		internal BloomFilter(Int32 maxElements, Int32 hashesPerToken, Int32 elementsHashed, BitArray array)
		{
			if (maxElements < 1 || hashesPerToken < 1 || array == null || array.Length < 1) { throw new ArgumentException(); }
						
			this.HashesPerToken = hashesPerToken;
			this.MaxElements = maxElements;
			this.ElementsHashed = elementsHashed;

			filterArray = new BitArray(array);
		}

		public BloomFilter(Int32 maxElements, Int32 hashesPerToken)
		{
			if (maxElements < 1 || hashesPerToken < 1)
			{
				throw new ArgumentException();
			}
			
			this.HashesPerToken = hashesPerToken;
			this.MaxElements = maxElements;

			// Increases MaxElements until divisible by 8
			while (SizeBits % 2 != 0 && SizeBits % 4 != 0 && SizeBits % 8 != 0)
			{
				this.MaxElements += 1;
			}

			filterArray = new BitArray(SizeBits, false);

			ElementsHashed = 0;
		}

		public void Add(string token)
		{
			if (string.IsNullOrEmpty(token)) { return; }

			int maxIndex = filterArray.Length - 1;
			using (BloomHash tokenHash = new BloomHash(token, IndexByteSize, maxIndex))
			{				
				IEnumerable<int> indices = tokenHash.GetIndices().Take(HashesPerToken).Where(i => !filterArray[i]);

				foreach (int index in indices)
				{
					filterArray[index] = true;
				}
			}
			ElementsHashed += 1;
		}

		public bool Query(string token)
		{
			if (string.IsNullOrEmpty(token)) { throw new ArgumentNullException("token"); }

			int maxIndex = filterArray.Length - 1;
			using (BloomHash tokenHash = new BloomHash(token, IndexByteSize, maxIndex))
			{
				if (tokenHash.GetIndices().Take(HashesPerToken).Any(i => !filterArray[i]))
				{
					return false;
				}
			}
			return true;
		}


		public string AsString()
		{
			int chunk = (filterArray.Length / samples) - 1;
			bool[] filterBits = filterArray.Cast<bool>().ToArray();

			int index = 0;
			int counter = samples;
			StringBuilder result = new StringBuilder();
			List<string> density = new List<string>();
			while (index < filterArray.Length - 1)
			{
				density.Add(string.Format("{0,2}", filterBits.Skip(index).Take(chunk).Count(b => b)));
				//result.Append(filterBits[index]?'1':'0');				
				index += chunk;
			}

			string string1 = string.Concat("[", string.Join("][", density.Take(samples / 4).ToArray()), "]");
			string string2 = string.Concat("[", string.Join("][", density.Skip(samples / 4).Take(samples / 4).ToArray()), "]");
			string string3 = string.Concat("[", string.Join("][", density.Skip(samples / 2).Take(samples / 4).ToArray()), "]");
			string string4 = string.Concat("[", string.Join("][", density.Skip(samples / 2).Skip(samples / 4).Take(samples / 4).ToArray()), "]");

			int halfLen = string1.Length / 2;

			string a1 = string1.Substring(0, halfLen);
			string a2 = string1.Substring(halfLen);
			string b1 = string2.Substring(0, halfLen);
			string b2 = string2.Substring(halfLen);
			string c1 = string3.Substring(0, halfLen);
			string c2 = string3.Substring(halfLen);
			string d1 = string4.Substring(0, halfLen);
			string d2 = string4.Substring(halfLen);

			result.AppendLine();
			result.AppendLine(a1);
			result.AppendLine(a2);
			result.AppendLine(b1);
			result.AppendLine(b2);
			result.AppendLine(c1);
			result.AppendLine(c2);
			result.AppendLine(d1);
			result.AppendLine(d2);
			result.AppendLine();
			return result.ToString();
		}

		public string GetUtilization()
		{
			bool[] filterBits = filterArray.Cast<bool>().ToArray();
			int trueBits = filterBits.Count(b => b);
			int totalBits = filterArray.Length;
			decimal percent = 0;
			if (totalBits != 0)
			{
				percent = (trueBits * 100) / totalBits;
			}
			return string.Format("{0:0.00}% \t ({1} / {2})", percent, trueBits, totalBits);
		}

		public int[] GetActiveBitIndicies()
		{
			int index = 0;
			List<int> result = new List<int>();
			List<bool> bitList = filterArray.Cast<bool>().ToList();
			while (true)
			{
				int i = bitList.IndexOf(true, index);
				if (i == -1) { return result.ToArray(); }
				result.Add(i);
				index = i + 1;
			}
		}
	}
}
