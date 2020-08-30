using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization;
using System.Text;
using BloomFilterCore.Hashes;

namespace BloomFilterCore
{
	[DataContract]
	public partial class BloomFilter
	{
		[DataMember]
		public double ErrorProbability { get; private set; }
		[DataMember]
		public Int32 MaxElementsToHash { get; private set; }
		[DataMember]
		public Int32 HashesPerElement { get; private set; }
		[DataMember]
		public Int32 ElementsHashed { get; private set; }
		[DataMember]

		public Int32 FilterSizeInBits { get; private set; }
		[IgnoreDataMember]
		public decimal FilterSizeInBytes { get { return Math.Round(((decimal)FilterSizeInBits) / 8m); } }

		[IgnoreDataMember]
		public List<bool> FilterArray { get { return _filterArray; } }
		[DataMember]
		internal List<bool> _filterArray = new List<bool>();

		[DataMember]
		public List<Tuple<BigInteger, BigInteger>> HashFunctionParameters { get; private set; } = new List<Tuple<BigInteger, BigInteger>>();
		[IgnoreDataMember]
		private List<Func<BigInteger, int>> HashFunctions;

		#region Constructors

		public BloomFilter()
		{
			this.HashFunctions = new List<Func<BigInteger, int>>();
			this.HashFunctionParameters = new List<Tuple<BigInteger, BigInteger>>();
		}

		public BloomFilter(int maxElementsToHash, double collisionProbability)
			: this()
		{
			if (maxElementsToHash < 1 || collisionProbability < 0) { throw new ArgumentException(); }

			this.ElementsHashed = 0;
			this.MaxElementsToHash = maxElementsToHash;
			this.ErrorProbability = collisionProbability;

			int sizeOfArray = CalculateFilterSize(MaxElementsToHash, collisionProbability);
			sizeOfArray = NextSquareDivisibleByEight(sizeOfArray);
			this.FilterSizeInBits = sizeOfArray;

			this.HashesPerElement = CalculateHashesPerElement(FilterSizeInBits, MaxElementsToHash);
			this.HashFunctionParameters = FindHashFunctionParameters(HashesPerElement, FilterSizeInBits);
			BuildHashFunctions();

			Clear();
		}

		#endregion

		#region Public Methods

		public void BuildHashFunctions()
		{
			this.HashFunctions = BuildHashFunctions(HashFunctionParameters);
		}

		public void Add(string element)
		{
			int[] indices = GetIndicesToSet(element);

			foreach (int index in indices)
			{
				_filterArray[index] = true;
			}

			ElementsHashed++;
		}

		public bool Contains(string element)
		{
			int[] indices = GetIndicesToSet(element);
			return indices.All(i => _filterArray[i] == true);
		}

		public void Clear()
		{
			_filterArray = new BitArray(FilterSizeInBits, false).Cast<bool>().ToList();
		}

		// Union => bitwise OR
		// Intersection => bitwise AND

		public decimal GetUtilizationPercentage()
		{
			if (_filterArray == null || _filterArray.Count < 1) { throw new ArgumentNullException("filterArray"); }

			decimal percent = 0;
			IEnumerable<bool> flippedBits = _filterArray.Where(b => b == true);
			int setBits = flippedBits.Count();
			if (setBits > 0)
			{
				percent = setBits * 100 / FilterSizeInBits;
			}
			return percent; // return string.Format("{0:0.00}% \t ({1} / {2})", percent, setBits, SizeBits); 
		}

		#endregion

		#region Private Methods

		#region Calculate Sizes

		private static int CalculateFilterSize(int maxElementsToHash, double probabilityFloor)
		{
			decimal top = (decimal)maxElementsToHash * (decimal)Math.Abs(Math.Log(probabilityFloor));
			decimal bottom = (decimal)Math.Pow(Math.Log(2), 2.0d);

			decimal result = top / bottom;
			return (int)Math.Ceiling(result);
		}

		private static int NextSquareDivisibleByEight(int number)
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
				root += 1;
			}

			return (int)result;
		}

		private static int CalculateHashesPerElement(int sizeOfArray, int maxElementsToHash)
		{
			double rhs = sizeOfArray / maxElementsToHash;
			double hashesPerElement = Math.Abs(rhs * Math.Log(2));

			double result = Math.Round(hashesPerElement);
			return (int)result;
		}

		private static List<Tuple<BigInteger, BigInteger>> FindHashFunctionParameters(int quantity, int sizeOfFilter)
		{
			List<Tuple<BigInteger, BigInteger>> results = new List<Tuple<BigInteger, BigInteger>>();

			BigInteger p = sizeOfFilter + 1;

			int counter = 0;
			while (counter < quantity)
			{
				p = PrimeFactory.GetPreviousPrime(p - 1);
				BigInteger g = FindPrimitiveRoot(p);
				results.Add(new Tuple<BigInteger, BigInteger>(p, g));

				counter++;
			}
			return results;
		}

		private static List<Func<BigInteger, int>> BuildHashFunctions(List<Tuple<BigInteger, BigInteger>> hashFunctionParameters)
		{
			List<Func<BigInteger, int>> results = new List<Func<BigInteger, int>>();

			foreach (var parameterTuple in hashFunctionParameters)
			{
				BigInteger p = parameterTuple.Item1;
				BigInteger g = parameterTuple.Item2;
				results.Add(new Func<BigInteger, int>(n => (int)PowerMod(g, n, p)));
			}
			return results;
		}

		private static BigInteger PowerMod(BigInteger value, BigInteger exponent, BigInteger modulus)
		{
			BigInteger result = BigInteger.One;
			while (exponent > 0)
			{
				if (exponent % 2 != 0) // If exponent is odd
				{
					result = (result * value).Mod(modulus);
				}

				value = (value * value).Mod(modulus);
				exponent >>= 1; // exponent = exponent >> 1
			}
			return result.Mod(modulus);
		}

		private static BigInteger FindPrimitiveRoot(BigInteger p)
		{
			// Check if p is prime or not 
			if (!Factorization.IsProbablePrime(p)) { throw new ArgumentException($"Parameter {nameof(p)} must be a prime number.", nameof(p)); }

			BigInteger phi = p - 1; // The Euler Totient function phi of a prime number p is p-1
			IEnumerable<BigInteger> primeFactors = Factorization.GetDistinctPrimeFactors(phi, BigInteger.Pow(10, 7));
			List<BigInteger> powersToTry = primeFactors.Select(factor => phi / factor).ToList();

			for (int r = 2; r <= phi; r++) // Check for every number from 2 to phi 
			{
				if (powersToTry.All(n => BigInteger.ModPow(r, n, p) != 1)) // If there was no n such that r^n ≡ 1 (mod p) 
				{
					return r; // We found our primitive root
				}
			}

			throw new Exception($"No primitive root found for prime {p}!"); // If no primitive root found 
		}

		#endregion

		private int[] GetIndicesToSet(string element)
		{
			if (string.IsNullOrEmpty(element)) { throw new ArgumentNullException("element"); }

			BigInteger elementValue = CalculateValue(element);

			List<int> result = new List<int>();
			foreach (Func<BigInteger, int> hashFunction in HashFunctions)
			{
				result.Add(hashFunction.Invoke(elementValue));
			}

			return result.ToArray();
		}

		private static BigInteger CalculateValue(string input)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(input);
			Array.Reverse(bytes);

			int counter = 0;
			BigInteger placeValue = new BigInteger(0);
			BigInteger result = new BigInteger(0);
			foreach (byte octet in bytes)
			{
				placeValue = BigInteger.Pow(_byteMax, counter);
				placeValue *= octet;
				result += placeValue;
				counter++;
			}
			return result;
		}
		private static BigInteger _byteMax = new BigInteger(256);

		#endregion

		public override string ToString()
		{
			return $"Bloom filter is {FilterSizeInBytes:0.00} bytes long and has used {GetUtilizationPercentage():0.00}% of its {MaxElementsToHash} element capacity.";
		}
	}
}
