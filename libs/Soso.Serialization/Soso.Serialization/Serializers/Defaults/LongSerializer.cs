using Soso.Serialization.Binary;

namespace Soso.Serialization.Serializers.Defaults
{
    public class LongSerializer : ISerializer<long>
    {
        public void Serialize(ref ByteWriter writer, object value, SerializationConfig config)
        {
            Serialize(ref writer, (long)value, config);
        }

        object ISerializer.Deserialize(ref ByteReader reader, SerializationConfig config)
        {
            return Deserialize(ref reader, config);
        }

        public long Deserialize(ref ByteReader reader, SerializationConfig config)
        {
            return reader.ReadLong();
        }
        public void Serialize(ref ByteWriter writer, long value, SerializationConfig config)
        {
            writer.Write(value);
        }
    }
}