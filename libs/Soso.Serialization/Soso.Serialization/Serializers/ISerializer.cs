using System;
using Soso.Serialization.Binary;

namespace Soso.Serialization.Serializers
{
	public interface ISerializer
	{
		public void Serialize(ref ByteWriter writer, object value, SerializationConfig config);
		public object Deserialize(ref ByteReader reader, SerializationConfig config);
	}
	public interface ISerializer<T> : ISerializer
	{
		public void Serialize(ref ByteWriter writer, T value, SerializationConfig config);
		public new T Deserialize(ref ByteReader reader, SerializationConfig config);
	}
}
