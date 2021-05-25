using System.IO;
using System.IO.Compression;
 
namespace Serialize
{
    public static class Zip
    {
        public static byte[] Compress(byte[] src)
        {
            using (var input = new MemoryStream(src))
            {
                using (var output = new MemoryStream())
                {
                    using (var compressor = new GZipStream(output, CompressionMode.Compress))
                    {
                        input.CopyTo(compressor);
                    }
                    return output.ToArray();
                }
            }
        }
 
        public static byte[] Decompress(byte[] src)
        {
            using (var input = new MemoryStream(src))
            {
                using (var decompressor = new GZipStream(input, CompressionMode.Decompress))
                {
                    using (var output = new MemoryStream())
                    {
                        decompressor.CopyTo(output);
 
                        return output.ToArray();
                    }
                }
            }
        }
    }
}