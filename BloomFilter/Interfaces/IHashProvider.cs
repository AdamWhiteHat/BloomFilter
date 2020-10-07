using System;

namespace BloomFilterCore
{
	public interface IHashProvider : IDisposable
	{
		void SetParameters(int hashesPerElement, int filterSize);
		void Initialize();
		int[] HashElement(string element);
	}
}
