using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;

using BloomFilterCore.Hashes;

namespace BloomFilterCore
{
	public partial class BloomFilter
	{
		public Int32 MaxElements { get; private set; }
		public Int32 ElementsHashed { get; private set; }
		public Int32 HashesPerElement { get; private set; }

		// Read-only properties
		public int SizeBits { get { return _filterArray.Count; } }
		public double IndexBitSize { get { return Math.Log(SizeBits, 2); } }
		public int IndexByteSize { get { return (int)Math.Ceiling(IndexBitSize / 8); } }

		public BitArray FilterArray { get { return _filterArray; } }
		private BitArray _filterArray;
		private int samples = 100;

		#region Constructors

		internal BloomFilter(Int32 maxElements, Int32 hashesPerToken, Int32 elementsHashed, BitArray array)
		{
			if (maxElements < 1 || hashesPerToken < 1 || array == null || array.Length < 1) { throw new ArgumentException(); }

			this.HashesPerElement = hashesPerToken;
			this.MaxElements = maxElements;
			this.ElementsHashed = elementsHashed;

			_filterArray = new BitArray(array);
		}

		public BloomFilter(Int32 maxElementsToHash, double errorProbabilityFloor)
		{
			if (maxElementsToHash < 1 || errorProbabilityFloor < 0) { throw new ArgumentException(); }

			this.MaxElements = maxElementsToHash;

			int sizeOfArray = CalculateFilterSize(MaxElements, errorProbabilityFloor);
			sizeOfArray = FindSquareMultipleOf8(sizeOfArray);
			this.HashesPerElement = CalculateHashesPerElement(sizeOfArray, MaxElements);

			_filterArray = new BitArray(sizeOfArray, false);
			ElementsHashed = 0;
		}

		#endregion

		#region Calculate Sizes

		private static int FindSquareMultipleOf8(int number)
		{
			double result = number;

			double root = Math.Sqrt(result);

			if (root % 1 != 0)
			{
				root = Math.Round(root, MidpointRounding.ToEven);
			}

			if (root % 2 != 0)
			{
				root += 1;
			}

			while ((result = Math.Pow(root, 2)) % 8 != 0)
			{
				root += 2;
			}

			return (int)result;
		}

		private static int CalculateFilterSize(int elementsToHashCeiling, double probabilityFloor)
		{
			double top = elementsToHashCeiling * Math.Log(probabilityFloor);
			double bottom = Math.Pow(Math.Log(2), 2);

			top = Math.Abs(top);
			bottom = Math.Abs(bottom);

			double dividend = top / bottom;
			double result = Math.Round(dividend);

			return (int)result;
		}

		private static int CalculateHashesPerElement(int sizeOfArray, int maxElementsToHash)
		{
			double rhs = sizeOfArray / maxElementsToHash;
			double hashesPerElement = rhs * Math.Log(2);

			double result = Math.Round(Math.Abs(hashesPerElement));
			return (int)result;
		}

		#endregion

		public void Add(string token)
		{
			int[] indices = GetIndices(token);

			foreach (int index in indices)
			{
				_filterArray[index] = true;
			}

			ElementsHashed += 1;
		}

		private int[] GetIndices(string token)
		{
			if (string.IsNullOrEmpty(token)) { throw new ArgumentNullException("token"); }

			int maxIndex = _filterArray.Length - 1;
			int[] indices = new int[] { };

			using (BloomHash tokenHash = new BloomHash(token, IndexByteSize, maxIndex))
			{
				indices = tokenHash.GetIndices().Take(HashesPerElement).Where(i => !_filterArray[i]).ToArray();
			}

			//indices = MD5Hash.GetIndices(Encoding.ASCII.GetBytes(token), HashesPerElement, MaxElements);

			return indices;
		}

		public bool Query(string token)
		{
			int[] indices = GetIndices(token);

			if (indices.Any(i => !_filterArray[i]))
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		public string GetUtilization()
		{
			if (_filterArray == null || _filterArray.Length < 1) { throw new ArgumentNullException("filterArray"); }

			decimal percent = 0;
			IEnumerable<bool> flippedBits = _filterArray.Cast<bool>().Where(b => b==true);
			int setBits = flippedBits.Count();
			if (setBits > 0)
			{
				percent = setBits * 100 / SizeBits;
			}
			return string.Format("{0:0.00}% \t ({1} / {2})", percent, setBits, SizeBits);
		}
		
		public string AsString()
		{
			int chunk = (_filterArray.Length / samples) - 1;
			bool[] filterBits = _filterArray.Cast<bool>().ToArray();

			int index = 0;
			int counter = samples;
			StringBuilder result = new StringBuilder();
			List<string> density = new List<string>();
			while (index < _filterArray.Length - 1)
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
	}
}
