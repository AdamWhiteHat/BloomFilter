using System;
using System.Text;
using System.Linq;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BloomFilterCore.HashProviders
{
	[DataContract]
	public class MultiplicativeGroupHashProvider : IHashProvider
	{
		[DataMember]
		private int _filterSize;
		[DataMember]
		private int _hashesPerElement;
		[DataMember]
		private List<Tuple<BigInteger, BigInteger>> _multiplicativeGroupParameters { get; set; }

		[IgnoreDataMember]
		private List<Func<BigInteger, int>> _hashFunctions;
		[IgnoreDataMember]
		private bool _isDisposed;
		[IgnoreDataMember]
		private static BigInteger _byteMax = new BigInteger(256);

		public MultiplicativeGroupHashProvider()
		{
			_isDisposed = false;
		}

		#region Public Methods

		public void SetParameters(int hashesPerElement, int filterSize)
		{
			_filterSize = filterSize;
			_hashesPerElement = hashesPerElement;
			_multiplicativeGroupParameters = new List<Tuple<BigInteger, BigInteger>>();
			BuildMultiplicativeGroupParameters();
		}

		public void Initialize()
		{
			BuildHashFunctionsFromGroupParameters();
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				_isDisposed = true;

				if (_hashFunctions != null)
				{
					_hashFunctions.Clear();
					_hashFunctions = null;
				}
			}
		}

		public int[] HashElement(string element)
		{
			if (string.IsNullOrEmpty(element)) { throw new ArgumentNullException("element"); }

			BigInteger elementValue = CalculateNumericValue(element);

			List<int> result = new List<int>();
			foreach (Func<BigInteger, int> hashFunction in _hashFunctions)
			{
				result.Add(hashFunction.Invoke(elementValue));
			}

			return result.ToArray();
		}

		#endregion

		#region Private Methods

		private void BuildMultiplicativeGroupParameters()
		{
			BigInteger prime = _filterSize + 1;

			int counter = 0;
			while (counter < _hashesPerElement)
			{
				prime = PrimeHelper.GetPreviousPrime(prime);
				BigInteger generator = CalculatePrimitiveRoot(prime);

				_multiplicativeGroupParameters.Add(new Tuple<BigInteger, BigInteger>(prime, generator));

				counter++;
			}
		}

		internal void BuildHashFunctionsFromGroupParameters()
		{
			if (_multiplicativeGroupParameters != null && _multiplicativeGroupParameters.Any())
			{
				_hashFunctions = new List<Func<BigInteger, int>>();
				foreach (var tuple in _multiplicativeGroupParameters)
				{
					BigInteger prime = tuple.Item1;
					BigInteger generator = tuple.Item2;
					_hashFunctions.Add(new Func<BigInteger, int>(n => (int)PowerMod(generator, n % (prime - 1), prime)));
				}
			}
		}

		private static BigInteger CalculateNumericValue(string input)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(input);
			Array.Reverse(bytes);

			int counter = 0;
			BigInteger result = new BigInteger(0);
			foreach (byte octet in bytes)
			{
				result += (BigInteger.Pow(_byteMax, counter) * octet);
				counter++;
			}
			return result;
		}

		private static BigInteger PowerMod(BigInteger value, BigInteger exponent, BigInteger modulus)
		{
			BigInteger result = BigInteger.One;
			while (exponent > 0)
			{
				if (exponent % 2 == 1) // If exponent is odd
				{
					result = (result * value).Mod(modulus);
					exponent -= 1;
					if (exponent == 0) { break; }
				}

				value = (value * value).Mod(modulus);
				exponent >>= 1; // exponent /= 2;
			}
			return result.Mod(modulus);
		}

		private static BigInteger CalculatePrimitiveRoot(BigInteger p)
		{
			// Check if p is prime or not 
			if (!PrimeHelper.IsProbablePrime(p)) { throw new ArgumentException($"Parameter {nameof(p)} must be a prime number.", nameof(p)); }

			BigInteger phi = p - 1; // The Euler Totient function phi of a prime number p is p-1
			IEnumerable<BigInteger> primeFactors = PrimeHelper.GetPrimeFactorization(phi).Distinct();
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

	}
}
