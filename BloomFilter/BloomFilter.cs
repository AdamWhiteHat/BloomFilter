using System;
using System.Text;
using System.Linq;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BloomFilterCore
{
	using HashProviders;
	using Filters;

	[KnownType(typeof(MultiplicativeGroupHashProvider))]
	[KnownType(typeof(StreamCipherHashProvider))]
	[KnownType(typeof(RunLengthEncodedFilter))]
	[KnownType(typeof(BitArrayFilter))]
	[DataContract]
	public class BloomFilter
	{
		[DataMember(Order = 0)]
		public Int32 MaxElementsToHash { get; private set; }

		[DataMember(Order = 1)]
		public double ErrorProbability { get; private set; }

		[DataMember(Order = 2)]
		public Int32 HashesPerElement { get; private set; }

		[DataMember(Order = 3)]
		public Int32 FilterSizeInBits { get; private set; }

		[DataMember(Order = 4)]
		public Int32 ElementsHashed { get; private set; }

		[DataMember(Name = "IFilter", Order = 6)]
		public IFilter FilterArray;

		[DataMember(Name = "IHashProvider", Order = 5)]
		private IHashProvider _hashProvider { get; set; }

		[IgnoreDataMember]
		public decimal FilterSizeInBytes { get { return Math.Round(((decimal)FilterSizeInBits) / 8m); } }

		#region Constructors


		public BloomFilter(int maxElementsToHash, double collisionProbability)
			: this()
		{
			if (maxElementsToHash < 1 || collisionProbability < 0) { throw new ArgumentException(); }

			this.ElementsHashed = 0;
			this.MaxElementsToHash = maxElementsToHash;
			this.ErrorProbability = collisionProbability;

			int sizeOfArray = CalculateFilterSize(MaxElementsToHash, collisionProbability);
			this.HashesPerElement = CalculateHashesPerElement(sizeOfArray, MaxElementsToHash);

			int recalculatedSizeOfArray = RecalculateFilterSize(HashesPerElement, MaxElementsToHash);
			recalculatedSizeOfArray = NextSquareDivisibleByEight(recalculatedSizeOfArray);
			this.FilterSizeInBits = recalculatedSizeOfArray;

			_hashProvider.SetParameters(HashesPerElement, FilterSizeInBits);

			InitializeHashProvider();

			ClearElements();
		}

		public BloomFilter()
		{
			_hashProvider = new StreamCipherHashProvider(); // new MultiplicativeGroupHashProvider(); 
		}

		public void ClearElements()
		{
			FilterArray = new BitArrayFilter(FilterSizeInBits); // BitArrayFilter(FilterSizeInBits);
		}

		public void InitializeHashProvider()
		{
			if (_hashProvider == null)
			{
				_hashProvider = new StreamCipherHashProvider(); // new MultiplicativeGroupHashProvider(); 
			}
			_hashProvider.Initialize();
		}

		#endregion

		#region Public Methods

		public void AddElement(string element)
		{
			int[] indices = _hashProvider.HashElement(element);

			foreach (int index in indices)
			{
				FilterArray[index] = true;
			}

			ElementsHashed++;
		}

		public bool ContainsElement(string element)
		{
			int[] indices = _hashProvider.HashElement(element);
			return indices.All(i => FilterArray[i] == true);
		}

		// Union => bitwise OR
		// Intersection => bitwise AND

		public decimal GetUtilizationPercentage()
		{
			if (FilterArray == null || FilterArray.Length < 1) { throw new ArgumentNullException("filterArray"); }

			decimal percent = 0;
			int setBits = FilterArray.SetBitCount;
			if (setBits > 0)
			{
				percent = setBits * 100 / FilterSizeInBits;
			}
			return percent; // return string.Format("{0:0.00}% \t ({1} / {2})", percent, setBits, SizeBits); 
		}

		#endregion

		#region Private Methods

		private static int CalculateFilterSize(int maxElementsToHash, double probabilityFloor)
		{
			decimal n = (decimal)maxElementsToHash;
			decimal top = n * (decimal)Math.Abs(Math.Log(probabilityFloor));
			decimal bottom = (decimal)Math.Pow(Math.Log(2.0d), 2.0d);

			decimal result = top / bottom;
			return (int)Math.Ceiling(result);
		}

		private static int NextSquareDivisibleByEight(int number)
		{
			double result = number;
			double root = Math.Sqrt(result);

			if (root % 1 != 0)
			{
				root = Math.Ceiling(root);
			}

			if (root % 2 != 0)
			{
				root += 1;
			}

			while ((result = Math.Pow(root, 2)) % 8 != 0)
			{
				root += 1;
			}

			return (int)result;
		}

		private static int CalculateHashesPerElement(int sizeOfArray, int maxElementsToHash)
		{
			double m = (double)sizeOfArray;
			double n = (double)maxElementsToHash;

			double rhs = m / n;
			double hashesPerElement = Math.Abs(rhs * Math.Log(2.0d));
			return (int)Math.Ceiling(hashesPerElement);
		}

		private static int RecalculateFilterSize(int hashesPerElement, int maxElementsToHash)
		{
			int kn = hashesPerElement * maxElementsToHash;
			double result = kn / Math.Log(2.0d);
			return (int)Math.Ceiling(result);
		}

		#endregion

		public override string ToString()
		{
			return $"Bloom filter is {FilterSizeInBytes:n0} bytes long and has used {GetUtilizationPercentage():0.00}% of its {MaxElementsToHash:n0} element capacity.";
		}
	}
}
