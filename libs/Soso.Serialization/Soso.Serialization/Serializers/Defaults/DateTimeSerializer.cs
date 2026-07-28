using System;
using Soso.Serialization.Binary;

namespace Soso.Serialization.Serializers.Defaults
{
    public class DateTimeSerializer : ISerializer<DateTime>
    {
        public void Serialize(ref ByteWriter writer, object value, SerializationConfig config)
        {
            Serialize(ref writer, (DateTime)value, config);
        }

        public DateTime Deserialize(ref ByteReader reader, SerializationConfig config)
        {
            long binary = reader.ReadLong();
            return DateTime.FromBinary(binary);
        }

        public void Serialize(ref ByteWriter writer, DateTime value, SerializationConfig config)
        {
            long binary = value.ToBinary();
            writer.Write(binary);
        }

        object ISerializer.Deserialize(ref ByteReader reader, SerializationConfig config)
        {
            return Deserialize(ref reader, config);
        }
    }
}