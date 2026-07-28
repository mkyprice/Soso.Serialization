using Soso.Serialization.Binary;
using Soso.Serialization.Serializers;
using System.Numerics;

namespace Soso.Serialization.Benchmarks
{
	public static class Serializers
	{
		public static void Serialize(ref ByteWriter writer, Vector2 value, SerializationConfig config)
		{
			writer.Write(value.X);
			writer.Write(value.Y);
		}
		public static void SerializeAsBlittable(ref ByteWriter writer, Vector2 value, SerializationConfig config)
		{
			writer.WriteBlittable(value);
		}
		
		public static Vector2 Deserialize(ref ByteReader reader, SerializationConfig config)
		{
			Vector2 vector = new Vector2(
				reader.ReadFloat(), 
				reader.ReadFloat()
				);
			return vector;
		}
		
		public static Vector2 DeserializeAsBlittable(ref ByteReader reader, SerializationConfig config)
		{
			Vector2 vector = reader.ReadBlittable<Vector2>();
			return vector;
		}
	}
}
