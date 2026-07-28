using System.Numerics;
using Soso.Serialization.Tests.Models;

namespace Soso.Serialization.Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void VectorTest()
    {
        Vector2 value = new Vector2(123.45f, 54.321f);

        byte[] bytes = SosoSerializer.Serialize(value);

        Vector2 result = SosoSerializer.Deserialize<Vector2>(bytes);

        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void DictionaryTest()
    {
        Dictionary<string, string> value = new Dictionary<string, string>();
        value["Hello"] = "world";
        value["How's"] = "it going?";

        byte[] bytes = SosoSerializer.Serialize(value);

        var result = SosoSerializer.Deserialize<Dictionary<string, string>>(bytes);

        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void InterfaceTest()
    {
        IDemo value = new Demo()
        {
            Test = "Hello there",
            TestF = 123.456f,
        };

        var config = SerializationConfig.Default.AddMapping<IDemo, Demo>();
        
        byte[] bytes = SosoSerializer.Serialize(value, SerializationFlags.EmbedType, config);

        IDemo result = SosoSerializer.Deserialize<IDemo>(bytes, SerializationFlags.EmbedType);

        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void ListTest()
    {
        List<string> value = new List<string>();
        value.Add("Hello");
        value.Add("there");

        var config = SerializationConfig.Default;

        byte[] bytes = SosoSerializer.Serialize(value, SerializationFlags.None, config);

        var result = SosoSerializer.Deserialize<List<string>>(bytes, SerializationFlags.None, config);

        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void ArrayTest()
    {
        string[] value = new string[]
        {
            "Hello",
            "there"
        };

        byte[] bytes = SosoSerializer.Serialize(value);

        var result = SosoSerializer.Deserialize<string[]>(bytes);

        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void EmbededTypeTest()
    {
        string[] value = new string[]
        {
            "Hello",
            "there"
        };

        byte[] bytes = SosoSerializer.Serialize(value, SerializationFlags.EmbedType);

        var result = SosoSerializer.Deserialize(bytes);

        Assert.That(result, Is.EqualTo(value));
    }
}