using Soso.Serialization.Binary;

namespace Soso.Serialization.Serializers.Defaults
{
    public class ULongSerializer : ISerializer<ulong>
    {
        public void Serialize(ref ByteWriter writer, object value, SerializationConfig config)
        {
            Serialize(ref writer, (ulong)value, config);
        }

        object ISerializer.Deserialize(ref ByteReader reader, SerializationConfig config)
        {
            return Deserialize(ref reader, config);
        }

        public ulong Deserialize(ref ByteReader reader, SerializationConfig config)
        {
            return reader.ReadULong();
        }
        public void Serialize(ref ByteWriter writer, ulong value, SerializationConfig config)
        {
            writer.Write(value);
        }
    }
}