using Soso.Serialization.Binary;

namespace Soso.Serialization.Serializers.Defaults
{
    public class BoolSerializer : ISerializer<bool>
    {
        public void Serialize(ref ByteWriter writer, object value, SerializationConfig config)
        {
            Serialize(ref writer, (bool)value, config);
        }

        object ISerializer.Deserialize(ref ByteReader reader, SerializationConfig config)
        {
            return Deserialize(ref reader, config);
        }

        public bool Deserialize(ref ByteReader reader, SerializationConfig config)
        {
            return reader.ReadBool();
        }
        public void Serialize(ref ByteWriter writer, bool value, SerializationConfig config)
        {
            writer.Write(value);
        }
    }
}