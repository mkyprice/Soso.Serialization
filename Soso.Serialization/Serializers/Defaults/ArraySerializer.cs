using System;
using Soso.Serialization.Binary;
using Soso.Serialization.Binary.Extensions;
using Soso.Serialization.Reflection;

namespace Soso.Serialization.Serializers.Defaults
{
    public class ArraySerializer : ISerializer<Array>
    {
        public void Serialize(ref ByteWriter writer, object value, SerializationConfig config)
        {
            Serialize(ref writer, (Array)value, config);
        }

        public Array Deserialize(ref ByteReader reader, SerializationConfig config)
        {
            Type eT = reader.ReadType();
            int count = reader.ReadInt();
            
            Array list = Array.CreateInstance(eT, count);
            for (int i = 0; i < count; i++)
            {
                var item = SosoSerializer.Deserialize(ref reader, eT, SerializationFlags.None, config);
                list.SetValue(item, i);
            }
            return list;
        }

        public void Serialize(ref ByteWriter writer, Array value, SerializationConfig config)
        {
            SosoTypeCode eT = value.GetType().GetElementType();

            writer.Write(eT);
            writer.Write((int)value.Length);
            foreach (var item in value)
            {
                SosoSerializer.Serialize(ref writer, item, SerializationFlags.None, config);
            }
        }

        object ISerializer.Deserialize(ref ByteReader reader, SerializationConfig config)
        {
            return Deserialize(ref reader, config);
        }
    }
}