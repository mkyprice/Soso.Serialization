using Soso.Serialization.Binary;

namespace Soso.Serialization.Serializers.Defaults
{
    public class SByteSerializer : ISerializer<sbyte>
    {
        public void Serialize(ref ByteWriter writer, object value, SerializationConfig config)
        {
            Serialize(ref writer, (sbyte)value, config);
        }

        object ISerializer.Deserialize(ref ByteReader reader, SerializationConfig config)
        {
            return Deserialize(ref reader, config);
        }

        public sbyte Deserialize(ref ByteReader reader, SerializationConfig config)
        {
            return reader.ReadSByte();
        }
        public void Serialize(ref ByteWriter writer, sbyte value, SerializationConfig config)
        {
            writer.Write(value);
        }
    }
}