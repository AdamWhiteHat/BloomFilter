using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomFilterCore
{
	public class BloomFilter
	{
		public int MaxElements;
		public int SizeBits { get { return MaxElements; } }
		public int SizeBytes { get { return SizeBits / 8; } }
		public int SizeKB { get { return SizeBits / 8192; } }
		public int SizeMB { get { return SizeBits / 8388608; } }
		public double IndexBitSize { get { return Math.Log(SizeBits, 2); } }
		public int IndexByteSize { get { return (int)Math.Ceiling(IndexBitSize / 8); } }
		public int ElementsHashed { get; private set; }
		public int HashesPerToken { get; private set; }

		private BitArray filterArray;
		private int samples = 100;

		public BloomFilter(int maxElements, int hashesPerToken)
		{
			if (maxElements < 1 || hashesPerToken < 1)
			{
				throw new ArgumentException();
			}

			this.MaxElements = maxElements;
			if (MaxElements < 16777216)
			{
				this.MaxElements = 16777216;
			}
			this.HashesPerToken = hashesPerToken;
			
			double power = Math.Ceiling(IndexBitSize);

			while ( !(IndexBitSize % 8 == 0 && power == IndexBitSize) )
			{
				this.MaxElements += 1;
				power = Math.Ceiling(IndexBitSize);
			}

			double elements = Math.Pow(2, power);
			this.MaxElements = (int)elements;

			//// Increases MaxElements until divisible by 8
			//while (SizeBits % 2 != 0 && SizeBits % 4 != 0 && SizeBits % 8 != 0)
			//{
			//	this.MaxElements += 1;
			//}

			filterArray = new BitArray(SizeBits, false);

			int bytes = SizeBytes;

			ElementsHashed = 0;
		}

		public void Add(string token)
		{
			if (string.IsNullOrEmpty(token)) { return; }

			int maxIndex = filterArray.Length - 1;
			using (BloomHash tokenHash = new BloomHash(token, IndexByteSize, maxIndex))
			{
				// tokenHash.GetIndices().Take(HashesPerToken).ToList().ForEach(i => filterArray[i] |= true);
				List<int> indices = tokenHash.GetIndices().Take(HashesPerToken).Where(i => !filterArray[i]).ToList();

				foreach (int index in indices)
				{
					filterArray[index] = true;
				}
				ElementsHashed += 1;
			}

		}

		public bool Query(string token)
		{
			if (string.IsNullOrEmpty(token))
			{
				throw new ArgumentNullException("token");
			}
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

		public void SerializeFilter(string filename)
		{
			if (!string.IsNullOrWhiteSpace(filename))
			{
				List<byte> output = new List<byte>();
				output.AddRange(BitConverter.GetBytes(MaxElements));
				output.AddRange(BitConverter.GetBytes(HashesPerToken));
				output.AddRange(ByteBits.GetBytes(filterArray.OfType<bool>().ToArray()));
				File.WriteAllBytes(filename, output.ToArray());
			}
		}

		public static BloomFilter DeserializeFilter(string filename)
		{
			if (string.IsNullOrWhiteSpace(filename) || !File.Exists(filename))
			{
				throw new ArgumentException();
			}
			byte[] input = File.ReadAllBytes(filename);
			byte[] header = input.Take(8).ToArray();
			byte[] body = input.Skip(8).ToArray();

			int maxElements = BitConverter.ToInt32(header, 0);
			int hashesPerToken = BitConverter.ToInt32(header, 4);

			BloomFilter result = new BloomFilter(maxElements, hashesPerToken);
			result.filterArray = new BitArray(body);
			return result;
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
			int percent = (trueBits * 100) / totalBits;
			return string.Format("{0}%\t({1}/{2})", percent, trueBits, totalBits);
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
