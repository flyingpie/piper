using System.IO.Compression;
using System.Threading;
using Piper.Core.Data;

namespace Piper.Core.Functions;

[AttributeUsage(AttributeTargets.Class)]
public sealed class PpFuncAttribute(string name) : Attribute
{
	public string Name { get; } = Guard.Against.NullOrWhiteSpace(name);
}

public interface IPpFunc<TType>
{
	Task<TType> ExecuteAsync(TType value, CancellationToken ct = default);
}

public class PpCompressFunc : IPpFunc<string>
{
	public async Task<string> ExecuteAsync(string value, CancellationToken ct = default)
	{
		byte[] buffer = Encoding.UTF8.GetBytes(value);
		var memoryStream = new MemoryStream();
		using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
		{
			gZipStream.Write(buffer, 0, buffer.Length);
		}

		memoryStream.Position = 0;

		var compressedData = new byte[memoryStream.Length];
		memoryStream.Read(compressedData, 0, compressedData.Length);

		var gZipBuffer = new byte[compressedData.Length + 4];
		Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
		Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);
		return Convert.ToBase64String(gZipBuffer);
	}

	// /// <summary>
	// /// Decompresses the string.
	// /// </summary>
	// /// <param name="compressedText">The compressed text.</param>
	// /// <returns></returns>
	// public static string DecompressString(string compressedText)
	// {
	// 	byte[] gZipBuffer = Convert.FromBase64String(compressedText);
	// 	using (var memoryStream = new MemoryStream())
	// 	{
	// 		int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
	// 		memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);
	//
	// 		var buffer = new byte[dataLength];
	//
	// 		memoryStream.Position = 0;
	// 		using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
	// 		{
	// 			gZipStream.Read(buffer, 0, buffer.Length);
	// 		}
	//
	// 		return Encoding.UTF8.GetString(buffer);
	// 	}
	// }
}

[PpFunc("reverse")]
public class PpReverseFunc : IPpFunc<string>
{
	public Task<string> ExecuteAsync(string value, CancellationToken ct = default)
	{
		return Task.FromResult(new string(value.Reverse().ToArray()));
	}
}
