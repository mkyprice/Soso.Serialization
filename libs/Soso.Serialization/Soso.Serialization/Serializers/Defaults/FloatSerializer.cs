using Soso.Serialization.Binary;

namespace Soso.Serialization.Serializers.Defaults
{
    public class FloatSerializer : ISerializer<float>
    {
        public void Serialize(ref ByteWriter writer, object value, SerializationConfig config)
        {
            Serialize(ref writer, (float)value, config);
        }

        object ISerializer.Deserialize(ref ByteReader reader, SerializationConfig config)
        {
            return Deserialize(ref reader, config);
        }

        public float Deserialize(ref ByteReader reader, SerializationConfig config)
        {
            return reader.ReadFloat();
        }
        public void Serialize(ref ByteWriter writer, float value, SerializationConfig config)
        {
            writer.Write(value);
        }
    }
}