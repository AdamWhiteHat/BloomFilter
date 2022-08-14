using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;

namespace BloomFilterCore.Serializers
{
	public static class CompressedStream
	{
		public static void Write(string filename, byte[] data)
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

		public static byte[] Read(string filename)
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
