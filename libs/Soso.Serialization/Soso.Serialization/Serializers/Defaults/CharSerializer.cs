using Soso.Serialization.Binary;

namespace Soso.Serialization.Serializers.Defaults
{
    public class CharSerializer : ISerializer<char>
    {
        public void Serialize(ref ByteWriter writer, object value, SerializationConfig config)
        {
            Serialize(ref writer, (char)value, config);
        }

        object ISerializer.Deserialize(ref ByteReader reader, SerializationConfig config)
        {
            return Deserialize(ref reader, config);
        }

        public char Deserialize(ref ByteReader reader, SerializationConfig config)
        {
            return reader.ReadChar();
        }
        public void Serialize(ref ByteWriter writer, char value, SerializationConfig config)
        {
            writer.Write(value);
        }
    }
}