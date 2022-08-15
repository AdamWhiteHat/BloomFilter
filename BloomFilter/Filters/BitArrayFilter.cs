using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BloomFilterCore.Filters
{
	[DataContract]
	public class BitArrayFilter : IFilter
	{
		[DataMember(Name = "FilterArray")]
		private BitArray _filterArray;

		[IgnoreDataMember]
		public bool this[int index]
		{
			get { return _filterArray[index]; }
			set { _filterArray[index] = value; }
		}

		[IgnoreDataMember]
		public int Length { get { return _filterArray.Length; } }

		[IgnoreDataMember]
		public int SetBitCount { get { return _filterArray.OfType<bool>().Count(b => b == true); } }


		public BitArrayFilter()
		{
			_filterArray = new BitArray(0);
		}

		public BitArrayFilter(int filterSizeInBits)
		{
			_filterArray = new BitArray(filterSizeInBits, false);
		}

		public bool[] GetArray()
		{
			return _filterArray.OfType<bool>().ToArray();
		}
	}
}
