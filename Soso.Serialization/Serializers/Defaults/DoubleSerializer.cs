using Soso.Serialization.Binary;

namespace Soso.Serialization.Serializers.Defaults
{
    public class DoubleSerializer : ISerializer<double>
    {
        public void Serialize(ref ByteWriter writer, object value, SerializationConfig config)
        {
            Serialize(ref writer, (double)value, config);
        }

        object ISerializer.Deserialize(ref ByteReader reader, SerializationConfig config)
        {
            return Deserialize(ref reader, config);
        }

        public double Deserialize(ref ByteReader reader, SerializationConfig config)
        {
            return reader.ReadDouble();
        }
        public void Serialize(ref ByteWriter writer, double value, SerializationConfig config)
        {
            writer.Write(value);
        }
    }
}