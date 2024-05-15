using System.IO;
using System.Threading.Tasks;

public static class StreamExtensions
{
    public static async Task<byte[]> ToByteArrayAsync(this Stream stream)
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            await stream.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}