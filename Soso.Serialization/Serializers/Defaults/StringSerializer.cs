using System.Text;
using Soso.Serialization.Binary;

namespace Soso.Serialization.Serializers.Defaults
{
    public class StringSerializer : ISerializer<string>
    {
        public void Serialize(ref ByteWriter writer, object value, SerializationConfig config)
        {
            Serialize(ref writer, (string)value, config);
        }

        object ISerializer.Deserialize(ref ByteReader reader, SerializationConfig config)
        {
            return Deserialize(ref reader, config);
        }

        public string Deserialize(ref ByteReader reader, SerializationConfig config)
        {
            return reader.ReadString();
        }
        public void Serialize(ref ByteWriter writer, string value, SerializationConfig config)
        {
            writer.Write(value);
        }
    }
}