using Soso.Serialization.Binary;
using Soso.Serialization.Serializers;

namespace Soso.Serialization.Tests.Models
{
	public class DemoSerializer : ISerializer<Demo>
	{
		public void Serialize(ref ByteWriter writer, Demo value, SerializationConfig config)
		{
			writer.Write(value.Test);
			writer.Write(value.TestF);
		}
		public Demo Deserialize(ref ByteReader reader, SerializationConfig config)
		{
			Demo value = new Demo();
			value.Test = reader.ReadString();
			value.TestF = reader.ReadFloat();
			return value;
		}
	}
}
