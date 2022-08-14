using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BloomFilterCore
{
	public interface IFilter
	{
		bool this[int index] { get; set; }
		int Length { get; }
		int BitsSet { get; }
		bool[] GetArray();
	}
}
