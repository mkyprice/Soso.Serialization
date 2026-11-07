using Soso.Serialization.Binary;

namespace Soso.Serialization.Serializers.Defaults
{
    public class IntSerializer : ISerializer<int>
    {
        public void Serialize(ref ByteWriter writer, object value, SerializationConfig config)
        {
            Serialize(ref writer, (int)value, config);
        }

        object ISerializer.Deserialize(ref ByteReader reader, SerializationConfig config)
        {
            return Deserialize(ref reader, config);
        }

        public int Deserialize(ref ByteReader reader, SerializationConfig config)
        {
            return reader.ReadInt();
        }
        public void Serialize(ref ByteWriter writer, int value, SerializationConfig config)
        {
            writer.Write(value);
        }
    }
}