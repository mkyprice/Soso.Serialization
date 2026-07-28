using Soso.Serialization.Binary;

namespace Soso.Serialization.Serializers.Defaults
{
    public class UIntSerializer : ISerializer<uint>
    {
        public void Serialize(ref ByteWriter writer, object value, SerializationConfig config)
        {
            Serialize(ref writer, (uint)value, config);
        }

        object ISerializer.Deserialize(ref ByteReader reader, SerializationConfig config)
        {
            return Deserialize(ref reader, config);
        }

        public uint Deserialize(ref ByteReader reader, SerializationConfig config)
        {
            return reader.ReadUInt();
        }
        public void Serialize(ref ByteWriter writer, uint value, SerializationConfig config)
        {
            writer.Write(value);
        }
    }
}