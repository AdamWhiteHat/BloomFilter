﻿using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Net.Cache;

namespace BloomFilterCore.Serialization
{
	public class BloomFilterSerializer
	{
		public static void Save(BloomFilter filter, string filename)
		{
			if (string.IsNullOrWhiteSpace(filename)) { return; }

				// Header
				List<byte> header = new List<byte>();
				header.AddRange(BitConverter.GetBytes(filter.MaxElements));
				header.AddRange(BitConverter.GetBytes(filter.HashesPerElement));
				header.AddRange(BitConverter.GetBytes(filter.ElementsHashed));
				
				// Body
				byte[] body = ByteBits.GetBytes(filter.FilterArray);

				List<byte> fileBytes = new List<byte>();
				fileBytes.AddRange(header);
				fileBytes.AddRange(body);

				if (Settings.Output_Compress)
				{
					WriteCompressedFile(filename, fileBytes.ToArray());
				}
				else
				{
					using (FileStream fileStream = File.Create(filename))
					{
						using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
						{
							binaryWriter.Write(fileBytes.ToArray());
						}
					}
				}			
		}

		public static BloomFilter Load(string filename)
		{
			if (string.IsNullOrWhiteSpace(filename) || !File.Exists(filename)) { throw new ArgumentException(); }

				int bitsPerInt = 4;
				int headerSize = bitsPerInt * 3;
				List<byte> input = new List<byte>();
				if (Settings.Output_Compress)
				{
					input.AddRange(ReadCompressedFile(filename));
				}
				else
				{
					input.AddRange(File.ReadAllBytes(filename));
				}
							
				byte[] header = input.Take(headerSize).ToArray();
				Int32 maxElements = BitConverter.ToInt32(header, bitsPerInt * 0);
				Int32 hashesPerToken = BitConverter.ToInt32(header, bitsPerInt * 1);
				Int32 elementsHashed = BitConverter.ToInt32(header, bitsPerInt * 2);

				byte[] body = input.Skip(headerSize).ToArray();
				
				Array.Reverse(body);
				BitArray bits = new BitArray(body);

				BloomFilter result = new BloomFilter(maxElements, hashesPerToken, elementsHashed, bits);
				return result;		
		}

		private static void WriteCompressedFile(string filename, byte[] data)
		{
			using (FileStream fileStream = File.Create(filename))
			{
				using (DeflateStream compressStream = new DeflateStream(fileStream, CompressionMode.Compress))
				{
					using (BinaryWriter binaryWriter = new BinaryWriter(compressStream))
					{
						binaryWriter.Write(data);
						binaryWriter.Flush();
					}
				}
			}
		}

		private static byte[] ReadCompressedFile(string filename)
		{
			using (FileStream fileStream = File.OpenRead(filename))
			{
				using (DeflateStream deflateStream = new DeflateStream(fileStream, CompressionMode.Decompress))
				{
					using (BinaryReader binaryReader = new BinaryReader(deflateStream))
					{
						List<byte> result = new List<byte>();
						byte[] buffer;

						do
						{
							buffer = binaryReader.ReadBytes(1024);
							if (buffer.Length > 0)
							{
								result.AddRange(buffer);
							}
						}
						while (buffer.Length > 0);

						return result.ToArray();
					}
				}
			}
		}
	}
}