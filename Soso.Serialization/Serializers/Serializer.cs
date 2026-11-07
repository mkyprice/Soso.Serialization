using Soso.Serialization.Binary;

namespace Soso.Serialization.Serializers
{
	public class Serializer<T> : ISerializer<T>
	{
		public delegate void SerializeDelegate(ref ByteWriter writer, T value, SerializationConfig config);
		public delegate T DeserializeDelegate(ref ByteReader reader, SerializationConfig config);

		private readonly SerializeDelegate _serializer;
		private readonly DeserializeDelegate _deserializer;
            
		public Serializer(SerializeDelegate serializer, DeserializeDelegate deserializer)
		{
			_serializer = serializer;
			_deserializer = deserializer;
		}

		public void Serialize(ref ByteWriter writer, object value, SerializationConfig config)
		{
			_serializer(ref writer, (T)value, config);
		}
		T ISerializer<T>.Deserialize(ref ByteReader reader, SerializationConfig config)
		{
			return _deserializer(ref reader, config);
		}
		public void Serialize(ref ByteWriter writer, T value, SerializationConfig config)
		{
			_serializer(ref writer, value, config);
		}
		public object Deserialize(ref ByteReader reader, SerializationConfig config)
		{
			return _deserializer(ref reader, config);
		}
	}
}
