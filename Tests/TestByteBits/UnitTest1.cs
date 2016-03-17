using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BloomFilterCore;

namespace TestByteBits
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestMethod1()
		{
			byte test = 13;

			bool[] testBools = ByteBits.GetBools(new byte[] { test });

			byte result = ByteBits.GetBytes(testBools).Single();

			Assert.AreEqual(test, result);
		}
	}
}
