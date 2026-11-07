using Soso.Serialization.Binary;

namespace Soso.Serialization.Serializers.Defaults
{
    public class ByteSerializer : ISerializer<byte>
    {
        public void Serialize(ref ByteWriter writer, object value, SerializationConfig config)
        {
            Serialize(ref writer, (byte)value, config);
        }

        object ISerializer.Deserialize(ref ByteReader reader, SerializationConfig config)
        {
            return Deserialize(ref reader, config);
        }

        public byte Deserialize(ref ByteReader reader, SerializationConfig config)
        {
            return reader.ReadByte();
        }
        public void Serialize(ref ByteWriter writer, byte value, SerializationConfig config)
        {
            writer.Write(value);
        }
    }
}