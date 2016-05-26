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
				List<Int32> header = new List<Int32>();
				List<byte> body = new List<byte>();
				header.Add(filter.MaxElements);
				header.Add(filter.HashesPerToken);
				header.Add(filter.ElementsHashed);
				if (Settings.Output_Compress)
				{
					body.AddRange(HuffmanCompression.Encode(filter.filterArray.OfType<bool>()));
				}
				else
				{
					body.AddRange(ByteBits.GetBytes(filter.filterArray.OfType<bool>().ToArray()));
				}
				
				using (FileStream fStream = File.Open(filename, FileMode.Create))
				{
					using (BinaryWriter bWriter = new BinaryWriter(fStream, Encoding.UTF8, false))
					{
						foreach (Int32 integer in header)
						{
							bWriter.Write(integer);
						}
						bWriter.Write(body.ToArray());
						bWriter.Flush();
					}
				}
				
				//File.WriteAllBytes(filename, header.ToArray());
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
				byte[] header = input.Take(24).ToArray();
				byte[] body = input.Skip(24).ToArray();

				Int32 maxElements = BitConverter.ToInt32(header, 0);
				Int32 hashesPerToken = BitConverter.ToInt32(header, 8);
				Int32 elementsHashed = BitConverter.ToInt32(header, 16);

				// TODO: Ensure that (body.Count % 8 == 0)
				// TODO: Ensure body.Count == (input.Count - header.Count)
				BitArray bits = null;
				if (Settings.Output_Compress)
				{
					bits = HuffmanCompression.Decode(body);
				}
				else
				{
					bits = ByteBits.GetBitArray(body);
				}
			
				BloomFilter result = new BloomFilter(maxElements, hashesPerToken, elementsHashed, bits);
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