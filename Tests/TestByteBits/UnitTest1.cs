using System;
using System.Linq;
using System.Text;
using BloomFilterCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BloomFilterCore;
using BloomFilterCore.Serialization;
using System.Collections;
using System.Collections.Generic;

namespace TestByteBits
{
	[TestClass]
	public class UnitTest1
	{
		private static readonly List<byte> SampleByteArray = Enumerable.Range(0, 100).Select(b => BitConverter.GetBytes(b).First()).ToList();

		//[TestMethod]
		//public void TestMethod1()
		//{
		//	byte[] input = SampleByteArray.ToArray();
		//	Assert.AreEqual(test, result);
		//}

		[TestMethod]
		public void TestByteEndian()
		{
			byte[] input = SampleByteArray.ToArray();
			
			if (BitConverter.IsLittleEndian) 
			{
				input.Reverse();
			}

			byte[] output  = ByteBits.GetBytes(ByteBits.GetBools(input));

			Assert.AreEqual(input, output);
		}

		[TestMethod]
		public void TestHuffmanCompression()
		{
			byte[] input = SampleByteArray.ToArray();

			bool[] bits = ByteBits.GetBools(input);

			byte[] encoded = HuffmanCompression.Encode(bits);

			BitArray decoded = HuffmanCompression.Decode(encoded);

			byte[] output = ByteBits.GetBytes(decoded);

			Assert.AreEqual(input, output);
		}

		//[TestMethod]
		//public void TestSerialization()
		//{
			

		//	//Assert.AreEqual(test, result);
		//}
	}
}
