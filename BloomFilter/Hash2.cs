using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomFilterCore
{
	public class Hash2
	{
		public int Size { get; private set; }

		private List<Hash> internalArray;

		public Hash2()
		{
			Size = 256;

			List<Hash> hashArray = new List<Hash>();

			Coprimes cprime = new Coprimes(Size, Size * Size, (Size * Size) * 10);
			IEnumerable<int> coPrimes = cprime.GetCoprimes();
			List<int> primes = coPrimes.Take(Size).ToList();

			foreach (int prime in primes)
			{
				hashArray.Add(new Hash(prime));
			}

			internalArray = hashArray;
		}

		public Hash this[int index]
		{
			get
			{
				return internalArray[index];
			}
		}
	}
}
