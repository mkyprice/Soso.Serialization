using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Soso.Serialization.Binary;
using Soso.Serialization.Binary.Buffers.Writing;
using Soso.Serialization.Reflection;
using Soso.Serialization.Serializers.RW;

namespace Soso.Serialization
{
    public static class SosoSerializer
    {
        public static Encoding DefaultEncoding = Encoding.ASCII;
        public static SerializationConfig Config
        {
            get => _config;
            set => _config = value;
        }
        private static SerializationConfig _config = SerializationConfig.Default;
        
        public static List<Type> IgnoredAttributes => MemberMap.IgnoredAttributes;
        public static List<Type> IgnoredTypes => MemberMap.IgnoredTypes;
        public static BindingFlags MemberFlags
        {
            get => MemberMap.Flags;
            set => MemberMap.Flags = value;
        }

        #region Serialization

        public static int Serialize<T>(byte[] buffer, int offset, T value, SerializationFlags flags = SerializationFlags.None, SerializationConfig config = null)
        {
            ByteWriter writer = new ByteWriter(buffer);
            writer.Seek(offset);
            return Serialize<T>(ref writer, value, flags, config);
        }

        public static int Serialize<T>(Span<byte> buffer, T value, SerializationFlags flags = SerializationFlags.None, SerializationConfig config = null)
        {
            ByteWriter writer = new ByteWriter(buffer);
            return Serialize<T>(ref writer, value, flags, config);
        }

        private static readonly byte[] _serializationBuffer = new byte[IWriteBuffer<byte>.DEFAULT_CAPACITY];
        public static byte[] Serialize<T>(T value, SerializationFlags flags = SerializationFlags.None, SerializationConfig config = null)
        {
            lock (_serializationBuffer)
            {
                ByteWriter writer = new ByteWriter(_serializationBuffer);
                Serialize<T>(ref writer, value, flags, config);
                return writer.ToSpan().ToArray();
            }
        }

        public static int Serialize<T>(ref ByteWriter writer, T value, SerializationConfig config = null, SerializationFlags flags = SerializationFlags.None)
            => Serialize<T>(ref writer, value, flags, config);
        
        public static int Serialize<T>(ref ByteWriter writer, T value, SerializationFlags flags = SerializationFlags.None, SerializationConfig config = null)
        {
            long start = writer.Position;
            config ??= Config;

            Type type = typeof(T);
            if ((typeof(T) == typeof(object) || type.IsInterface) && value != null)
            {
                type = value.GetType();
            }

            ObjectWriter serializer = new ObjectWriter(config, config.GetSerializers());
            serializer.Serialize(ref writer, value, type, flags);

            return (int)(writer.Position - start);
        }

        #endregion


        #region Deserialization

        public static object Deserialize(Span<byte> bytes, SerializationConfig config = null)
        {
            config ??= Config;
            ByteReader reader = new ByteReader(bytes);
            return Deserialize(ref reader, config);
        }

        public static object Deserialize(ref ByteReader reader, SerializationConfig config = null)
        {
            config ??= Config;
            ObjectReader serializer = new ObjectReader(config, config.GetSerializers());
            return serializer.Deserialize(ref reader);
        }

        public static T Deserialize<T>(Span<byte> bytes, SerializationFlags flags = SerializationFlags.None, SerializationConfig config = null)
        {
            return (T)Deserialize(bytes, typeof(T), flags, config);
        }

        public static object Deserialize(Span<byte> bytes, Type type, SerializationFlags flags = SerializationFlags.None, SerializationConfig config = null)
        {
            ByteReader reader = new ByteReader(bytes);
            return Deserialize(ref reader, type, flags, config);
        }

        public static T Deserialize<T>(ref ByteReader reader, SerializationFlags flags = SerializationFlags.None, SerializationConfig config = null)
        {
            return (T)Deserialize(ref reader, typeof(T), flags, config);
        }

        public static object Deserialize(ref ByteReader reader, Type type, SerializationFlags flags = SerializationFlags.None, SerializationConfig config = null)
        {
            config ??= Config;
            
            ObjectReader serializer = new ObjectReader(config, config.GetSerializers());

            if (flags == SerializationFlags.EmbedType)
            {
                return serializer.Deserialize(ref reader);
            }
            return serializer.Deserialize(ref reader, type);
        }

        #endregion
    }
}