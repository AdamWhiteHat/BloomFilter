using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BloomFilterCore;
using BloomFilterCore.Serialization;

namespace UnitTestBloomFilter
{
	[TestClass]
	public class UnitTestConversion
	{
		public TestContext TestContext { get { return m_testContext; } set { m_testContext = value; } }
		private TestContext m_testContext;

		private static readonly List<byte> SampleByteArray = Enumerable.Range(0, 100).Select(b => BitConverter.GetBytes(b).First()).ToList();

		[TestMethod]
		public void TestBytesToBits()
		{
			byte[] input = SampleByteArray.ToArray();
			bool[] bytes = ByteBits.GetBits(input);
			byte[] output = ByteBits.GetBytes(bytes);

			CompareBytes(input, output);
		}

		private void CompareBytes(byte[] input, byte[] output)
		{
			byte counter = 0;
			foreach (byte bite in input)
			{
				byte b = output[counter];
				Assert.AreEqual(bite, b);
				counter++;
			}
		}
	}
}
