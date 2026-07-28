using Soso.Serialization.Binary;

namespace Soso.Serialization.Serializers.Defaults
{
    public class ShortSerializer : ISerializer<short>
    {
        public void Serialize(ref ByteWriter writer, object value, SerializationConfig config)
        {
            Serialize(ref writer, (short)value, config);
        }

        object ISerializer.Deserialize(ref ByteReader reader, SerializationConfig config)
        {
            return Deserialize(ref reader, config);
        }

        public short Deserialize(ref ByteReader reader, SerializationConfig config)
        {
            return reader.ReadShort();
        }
        public void Serialize(ref ByteWriter writer, short value, SerializationConfig config)
        {
            writer.Write(value);
        }
    }
}