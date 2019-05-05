using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using BloomFilterCore.Hashes;

namespace BloomFilterCore
{
    public partial class BloomFilter
	{
		public Int32 MaxElements { get; private set; }
		public Int32 ElementsHashed { get; private set; }
		public Int32 HashesPerElement { get; private set; }
		public double ErrorProbability { get; private set; }

		// Read-only properties
		public int SizeBits { get { return _filterArray.Count; } }
		public double IndexBitSize { get { return Math.Log(SizeBits, 2); } }
		public int IndexByteSize { get { return (int)Math.Ceiling(IndexBitSize / 8); } }

		public BitArray FilterArray { get { return _filterArray; } }
		private BitArray _filterArray;

		#region Constructors

		internal BloomFilter(Int32 maxElements, double collisionProbability, Int32 hashesPerToken, Int32 elementsHashed, BitArray array)
		{
			if (maxElements < 1 || hashesPerToken < 1 || array == null || array.Length < 1) { throw new ArgumentException(); }
						
			this.MaxElements = maxElements;
			this.ErrorProbability = collisionProbability;			
			this.HashesPerElement = hashesPerToken;
			this.ElementsHashed = elementsHashed;

			_filterArray = new BitArray(array);
		}

		public BloomFilter(Int32 maxElementsToHash, double collisionProbability)
		{
			if (maxElementsToHash < 1 || collisionProbability < 0) { throw new ArgumentException(); }

			this.MaxElements = maxElementsToHash;

			int sizeOfArray = CalculateFilterSize(MaxElements, collisionProbability);
			sizeOfArray = FindSquareMultipleOf8(sizeOfArray);
			this.HashesPerElement = CalculateHashesPerElement(sizeOfArray, MaxElements);
			this.ErrorProbability = collisionProbability;
			

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
	}
}
