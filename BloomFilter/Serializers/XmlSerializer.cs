﻿using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace BloomFilterCore.Serializers
{
	public static class XmlSerializer
	{
		public static void Save(BloomFilter filter, string filename, bool compressed = false)
		{
			if (string.IsNullOrWhiteSpace(filename)) { throw new ArgumentException(nameof(filename)); }
			if (filter == null) { throw new ArgumentNullException(nameof(filter)); }

			byte[] fileBytes = Serializer.Serialize<BloomFilter>(filter);

			if (compressed)
			{
				CompressedStream.Write(filename, fileBytes);
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
				fileBytes = CompressedStream.Read(filename);
			}
			else
			{
				fileBytes = File.ReadAllBytes(filename);
			}

			BloomFilter result = Serializer.Deserialize<BloomFilter>(fileBytes);
			result.InitializeHashProvider();
			return result;
		}

		private static class Serializer
		{
			public static byte[] Serialize<T>(T obj)
			{
				var serializer = new DataContractSerializer(typeof(T));
				var stream = new MemoryStream();
				using (var writer = XmlDictionaryWriter.CreateTextWriter(stream))
				{
					serializer.WriteObject(writer, obj);
				}
				return stream.ToArray();
			}

			public static T Deserialize<T>(byte[] data)
			{
				var serializer = new DataContractSerializer(typeof(T));
				using (var stream = new MemoryStream(data))
				using (var reader = XmlDictionaryReader.CreateTextReader(stream, XmlDictionaryReaderQuotas.Max))
				{
					return (T)serializer.ReadObject(reader);
				}
			}
		}
	}
}
