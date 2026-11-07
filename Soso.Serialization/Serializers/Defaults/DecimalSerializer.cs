using Soso.Serialization.Binary;

namespace Soso.Serialization.Serializers.Defaults
{
    public class DecimalSerializer : ISerializer<decimal>
    {
        public void Serialize(ref ByteWriter writer, object value, SerializationConfig config)
        {
            Serialize(ref writer, (decimal)value, config);
        }

        object ISerializer.Deserialize(ref ByteReader reader, SerializationConfig config)
        {
            return Deserialize(ref reader, config);
        }

        public decimal Deserialize(ref ByteReader reader, SerializationConfig config)
        {
            return reader.ReadDecimal();
        }
        public void Serialize(ref ByteWriter writer, decimal value, SerializationConfig config)
        {
            writer.Write(value);
        }
    }
}