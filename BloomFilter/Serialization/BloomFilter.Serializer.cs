using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomFilterCore.Serialization
{
	public class BloomFilterSerializer
	{
		public static void Save(BloomFilter filter, string filename)
		{
			if (string.IsNullOrWhiteSpace(filename)) { return; }

			try
			{
				List<byte> output = new List<byte>();
				output.AddRange(BitConverter.GetBytes(filter.MaxElements));
				output.AddRange(BitConverter.GetBytes(filter.HashesPerToken));
				output.AddRange(BitConverter.GetBytes(filter.ElementsHashed));
				if (Settings.Output_Compress)
				{
					output.AddRange(HuffmanCompression.Encode(filter.filterArray.OfType<bool>()));
				}
				else
				{
					output.AddRange(ByteBits.GetBytes(filter.filterArray.OfType<bool>().ToArray()));
				}
				File.WriteAllBytes(filename, output.ToArray());
			}
			catch (Exception ex)
			{
				string message = ex.Message;
				string stackTrace = ex.StackTrace;
				string targetSite = ex.TargetSite.Name;
			}
		}

		public static BloomFilter Load(string filename)
		{
			if (string.IsNullOrWhiteSpace(filename) || !File.Exists(filename)) { throw new ArgumentException(); }

			try
			{
				byte[] input = File.ReadAllBytes(filename);
				byte[] header = input.Take(8).ToArray();

				Int32 maxElements = BitConverter.ToInt32(header, 0);
				Int32 hashesPerToken = BitConverter.ToInt32(header, 4);
				Int32 elementsHashed = BitConverter.ToInt32(header, 4);

				bool[] body = new bool[] { };
				if (Settings.Output_Compress)
				{
					body = HuffmanCompression.Decode(input.Skip(8));
				}
				else
				{
					body = ByteBits.GetBools(input.Skip(8).ToArray());
				}
			
				BloomFilter result = new BloomFilter(maxElements, hashesPerToken, elementsHashed, new BitArray(body));
				return result;		
			}
			catch (Exception ex)
			{
				string message = ex.Message;
				string stackTrace = ex.StackTrace;
				string targetSite = ex.TargetSite.Name;
				return null;
			}
		}
	}
}