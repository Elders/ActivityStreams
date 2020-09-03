using System.IO;

namespace ActivityStreams
{
    public interface ISerializer
    {
        object Deserialize(System.IO.Stream str);
        void Serialize<T>(System.IO.Stream str, T message);
    }

    public static class ISerializerExtensions
    {
        public static object DeserializeFromBytes(this ISerializer self, byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            {
                return self.Deserialize(stream);
            }
        }

        public static byte[] SerializeToBytes<T>(this ISerializer self, T message)
        {
            using (var stream = new MemoryStream())
            {
                self.Serialize(stream, message);
                stream.Position = 0;
                return stream.ToArray();
            }
        }
    }
}
