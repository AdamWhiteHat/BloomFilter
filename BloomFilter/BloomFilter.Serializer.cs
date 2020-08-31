using System;
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
using System.Numerics;
using System.Runtime.Serialization;
using System.Xml;

namespace BloomFilterCore.Serialization
{
	public class BloomFilterSerializer
	{
		public static void Save(BloomFilter filter, string filename, bool compressed = false)
		{
			if (string.IsNullOrWhiteSpace(filename)) { throw new ArgumentException(nameof(filename)); }
			if (filter == null) { throw new ArgumentNullException(nameof(filter)); }

			byte[] fileBytes = BinarySerializer.Serialize<BloomFilter>(filter);

			if (compressed)
			{
				WriteCompressedFile(filename, fileBytes);
			}
			else
			{
				File.WriteAllBytes(filename, fileBytes);
			}
		}

		public static BloomFilter Load(string filename, bool compressed = false)
		{
			if (string.IsNullOrWhiteSpace(filename)) { throw new ArgumentNullException(nameof(filename)); }
			if (!File.Exists(filename)) { throw new ArgumentException(nameof(filename)); }

			byte[] fileBytes = new byte[0];

			if (compressed)
			{
				fileBytes = ReadCompressedFile(filename);
			}
			else
			{
				fileBytes = File.ReadAllBytes(filename);
			}

			BloomFilter result = BinarySerializer.Deserialize<BloomFilter>(fileBytes);

			result.BuildHashFunctions();

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

		private static class BinarySerializer
		{
			public static byte[] Serialize<T>(T obj)
			{
				var serializer = new DataContractSerializer(typeof(T));
				var stream = new MemoryStream();
				using (var writer = XmlDictionaryWriter.CreateBinaryWriter(stream))
				{
					serializer.WriteObject(writer, obj);
				}
				return stream.ToArray();
			}

			public static T Deserialize<T>(byte[] data)
			{
				var serializer = new DataContractSerializer(typeof(T));
				using (var stream = new MemoryStream(data))
				using (var reader = XmlDictionaryReader.CreateBinaryReader(stream, XmlDictionaryReaderQuotas.Max))
				{
					return (T)serializer.ReadObject(reader);
				}
			}
		}

	}
}