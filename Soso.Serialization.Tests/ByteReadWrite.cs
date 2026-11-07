using System.Text;
using Soso.Serialization.Binary;
using Soso.Serialization.Binary.Extensions;
using Soso.Serialization.Tests.Models;

namespace Soso.Serialization.Tests
{
    public class ByteReadWrite
    {
        [Test]
        [TestCase((byte)55)]
        [TestCase((sbyte)-55)]
        [TestCase((short)-33)]
        [TestCase((ushort)33)]
        [TestCase((int)-213123)]
        [TestCase((uint)321321)]
        [TestCase((long)-4324234332)]
        [TestCase((ulong)238912389893)]
        [TestCase((float)231.2311f)]
        [TestCase((double)2321.43243243)]
        [TestCase((bool)true)]
        [TestCase((char)'h')]
        public void UnmanagedRead<T>(T value)
            where T : unmanaged
        {
            byte[] buffer = new byte[sizeof(double)];
            ByteWriter writer = new ByteWriter(buffer);
            writer.Write(value);

            ByteReader reader = new ByteReader(writer.ToSpan());
            T result = reader.Read<T>();
            Assert.That(value, Is.EqualTo(result), $"{typeof(T).Name} failed");
        }

        [Test]
        [TestCase((byte)55)]
        [TestCase((sbyte)-55)]
        [TestCase((short)-33)]
        [TestCase((ushort)33)]
        [TestCase((int)-213123)]
        [TestCase((uint)321321)]
        [TestCase((long)-4324234332)]
        [TestCase((ulong)238912389893)]
        [TestCase((float)231.2311f)]
        [TestCase((double)2321.43243243)]
        [TestCase((bool)true)]
        [TestCase((char)'h')]
        public void UnmanagedPeek<T>(T value)
            where T : unmanaged
        {
            byte[] buffer = new byte[sizeof(double)];
            ByteWriter writer = new ByteWriter(buffer);
            writer.Write(value);

            ByteReader reader = new ByteReader(writer.ToSpan());
            T result = reader.Peek<T>();
            Assert.That(value, Is.EqualTo(result), $"{typeof(T).Name} failed");
            Assert.That(reader.Position, Is.EqualTo(0), $"Read position was {reader.Position}");
        }
        
	
        [Test]
        [TestCase("Testeroonie")]
        [TestCase("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.")]
        public void StringSerialization(string value)
        {
            byte[] buffer = new byte[value.Length * sizeof(char)];
            ByteWriter writer = new ByteWriter(buffer);
            writer.Write(value);

            ByteReader reader = new ByteReader(writer.ToSpan());
            string result = reader.ReadString();
            Assert.That(value, Is.EqualTo(result), $"{nameof(StringSerialization)} failed");
        }

        [Test]
        public void BlittableTestRW()
        {
            byte[] buffer = new byte[1024];
            BlittableTest blittable = new BlittableTest(12, 34);
            
            ByteWriter writer = new ByteWriter(buffer);
            writer.WriteBlittable(blittable);
            
            ByteReader reader = new ByteReader(writer.ToSpan());
            BlittableTest result = reader.ReadBlittable<BlittableTest>();
            
            Assert.That(result, Is.EqualTo(blittable));
        }
    }
}