using ProtoBuf;

namespace Vehicles.Infrastructure.Serialisation
{
    public static class ProtobufHelper
    {
        public static byte[] SerialiseToProtobuf<T>(T response)
        {
            using var stream = new MemoryStream();
            Serializer.Serialize(stream, response);
            return stream.ToArray();
        }

        public static T DeserialiseFromProtobuf<T>(byte[] data)
        {
            using var stream = new MemoryStream(data);
            return Serializer.Deserialize<T>(stream);
        }
    }
}
