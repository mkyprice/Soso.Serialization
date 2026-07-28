using Soso.Serialization.Binary;

namespace Soso.Serialization.Serializers.Defaults
{
	public class UShortSerializer : ISerializer<ushort>
	{
		public void Serialize(ref ByteWriter writer, object value, SerializationConfig config)
		{
			Serialize(ref writer, (ushort)value, config);
		}

		object ISerializer.Deserialize(ref ByteReader reader, SerializationConfig config)
		{
			return Deserialize(ref reader, config);
		}

		public ushort Deserialize(ref ByteReader reader, SerializationConfig config)
		{
			return reader.ReadUShort();
		}
		public void Serialize(ref ByteWriter writer, ushort value, SerializationConfig config)
		{
			writer.Write(value);
		}
	}
}
