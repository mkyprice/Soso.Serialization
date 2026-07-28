using System.Diagnostics;
using System.Numerics;
using Soso.Serialization.Binary;
using Soso.Serialization.Logging;

namespace Soso.Serialization.Benchmarks;

class Program
{
	static void Main(string[] args)
	{
		Log.Level = LOG_LEVEL.Debug;
		// RunStreamerBenchmarks(1_000_000);
		// RunSerializerBenchmarks(1_000_000);
		RunVector2Benchmarks(1_000_000);
	}

	private static void RunVector2Benchmarks(int count)
	{
		var config = SerializationConfig.Default
			.AddSerializer(Serializers.SerializeAsBlittable, Serializers.DeserializeAsBlittable);
		
		Stopwatch sw = new Stopwatch();

		Vector2[] vs = new Vector2[count];
		for (int i = 0; i < count; i++)
		{
			vs[i] = new Vector2(Random.Shared.NextSingle(), Random.Shared.NextSingle());
		}

		byte[] buffer = new byte[count * sizeof(float) * 2 + count * 128];
		int position = 0;
		sw.Start();
		for (int i = 0; i < count; i++)
		{
			position += SosoSerializer.Serialize(buffer, position, vs[i], SerializationFlags.EmbedType, config);
		}
		sw.Stop();
		
		Console.WriteLine($"Write took {sw.ElapsedMilliseconds}ms for {count} items. Total size: {position} bytes");

		
		sw.Restart();

		ByteReader reader = new ByteReader(buffer);
		for (int i = 0; i < count; i++)
		{
			Vector2 v = SosoSerializer.Deserialize<Vector2>(ref reader, SerializationFlags.EmbedType, config);
		}
		
		sw.Stop();
		Console.WriteLine($"Read took {sw.ElapsedMilliseconds}ms for {count} items");
	}

	private static void RunStreamerBenchmarks(int count)
	{
		var config = SerializationConfig.Default
			.SetFactory<Vector2>(args => new Vector2())
			.AddStreamingType<Vector2>();

		Stopwatch sw = new Stopwatch();

		Vector2[] vs = new Vector2[count];
		string[] ss = new string[count];
		for (int i = 0; i < count; i++)
		{
			vs[i] = new Vector2(Random.Shared.NextSingle(), Random.Shared.NextSingle());
			ss[i] = $"Test: {i}";
		}

		byte[] buffer = new byte[count * sizeof(float) * 2 + count * 128];
		int position = 0;
		sw.Start();
		for (int i = 0; i < count; i++)
		{
			position += SosoSerializer.Serialize(buffer, position, vs[i], SerializationFlags.EmbedType, config);
			position += SosoSerializer.Serialize(buffer, position, ss[i], SerializationFlags.EmbedType, config);
		}
		sw.Stop();
		
		Console.WriteLine($"Write took {sw.ElapsedMilliseconds}ms for {count} items. Total size: {position} bytes");

		
		sw.Restart();

		ByteReader reader = new ByteReader(buffer);
		for (int i = 0; i < count; i++)
		{
			Vector2 v = SosoSerializer.Deserialize<Vector2>(ref reader, SerializationFlags.EmbedType, config);
			string s = SosoSerializer.Deserialize<string>(ref reader, SerializationFlags.EmbedType, config);
			Debug.Assert(s == ss[i], $"String serialization failed. Should be {ss[i]} was {s}");
			Debug.Assert(v == vs[i], $"Vector serialization failed. Should be {vs[i]} was {v}");
		}
		
		sw.Stop();
		Console.WriteLine($"Read took {sw.ElapsedMilliseconds}ms for {count} items");
	}

	private static void RunSerializerBenchmarks(int count)
	{
		var config = SerializationConfig.Default
			.AddSerializer(Serializers.Serialize, Serializers.Deserialize);

		Stopwatch sw = new Stopwatch();

		Vector2[] vs = new Vector2[count];
		string[] ss = new string[count];
		for (int i = 0; i < count; i++)
		{
			vs[i] = new Vector2(Random.Shared.NextSingle(), Random.Shared.NextSingle());
			ss[i] = $"Test: {i}";
		}

		byte[] buffer = new byte[count * sizeof(float) * 2 + count * 128];
		int position = 0;
		sw.Start();
		for (int i = 0; i < count; i++)
		{
			position += SosoSerializer.Serialize(buffer, position, vs[i], SerializationFlags.EmbedType, config);
			position += SosoSerializer.Serialize(buffer, position, ss[i], SerializationFlags.EmbedType, config);
		}
		sw.Stop();
		
		Console.WriteLine($"Write took {sw.ElapsedMilliseconds}ms for {count} items. Total size: {position} bytes");

		
		sw.Restart();

		ByteReader reader = new ByteReader(buffer);
		for (int i = 0; i < count; i++)
		{
			Vector2 v = SosoSerializer.Deserialize<Vector2>(ref reader, SerializationFlags.EmbedType, config);
			string s = SosoSerializer.Deserialize<string>(ref reader, SerializationFlags.EmbedType, config);
			Debug.Assert(s == ss[i], $"String serialization failed. Should be {ss[i]} was {s}");
			Debug.Assert(v == vs[i], $"Vector serialization failed. Should be {vs[i]} was {v}");
		}
		
		sw.Stop();
		Console.WriteLine($"Read took {sw.ElapsedMilliseconds}ms for {count} items");
	}
}
