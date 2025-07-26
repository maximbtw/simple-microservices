using System.IO.Compression;
using Microsoft.IO;

namespace Utilities;

public static class CompressionHelper
{
    private const int DeflateStreamDecompressBufferSize = 27360;

    private static readonly RecyclableMemoryStreamManager RecyclableMemoryStreamManager = new();
        
    public static byte[] Compress(byte[] data)
    {
        using MemoryStream output = CompressToStream(data);
            
        return output.ToArray();
    }

    public static byte[] Decompress(byte[] data)
    {
        using MemoryStream decompressedStream = DecompressToStream(data);
        return decompressedStream.ToArray();
    }
        
    static public MemoryStream DecompressToStream(byte[] bytes)
    {
        using MemoryStream input = RecyclableMemoryStreamManager.GetStream(bytes);

        return DecompressToStream(input);
    }

    private static MemoryStream CompressToStream(byte[] data)
    {
        MemoryStream output = RecyclableMemoryStreamManager.GetStream();

        using var deflate = new DeflateStream(output, CompressionMode.Compress, leaveOpen: true);
        using var writer = new BinaryWriter(deflate);
            
        writer.Write(data);
        deflate.Flush();

        return output;
    }

    private static MemoryStream DecompressToStream(MemoryStream input)
    {
        using var deflate = new DeflateStream(input, CompressionMode.Decompress);

        byte[] temp = new byte[DeflateStreamDecompressBufferSize];
        int read;

        MemoryStream decompressedOutputStream = RecyclableMemoryStreamManager.GetStream();
        while ((read = deflate.Read(temp, offset: 0, DeflateStreamDecompressBufferSize)) > 0)
        {
            decompressedOutputStream.Write(temp, offset: 0, read);
        }

        return decompressedOutputStream;
    }
}