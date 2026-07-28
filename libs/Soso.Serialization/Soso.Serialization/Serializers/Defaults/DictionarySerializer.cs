using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Soso.Serialization.Binary;
using Soso.Serialization.Binary.Extensions;
using Soso.Serialization.Reflection;

namespace Soso.Serialization.Serializers.Defaults
{
    public class DictionarySerializer : ISerializer
    {
        private static readonly Encoding Encoding = Encoding.ASCII;
        public object Deserialize(ref ByteReader reader, SerializationConfig config)
        {
            Type tK = reader.ReadType();
            Type tV = reader.ReadType();
            Type dictType = typeof(Dictionary<,>).MakeGenericType(tK, tV);
            IDictionary dict = (IDictionary)Activator.CreateInstance(dictType);
            int count = reader.ReadInt();
            for (int i = 0; i < count; i++)
            {
                var key = SosoSerializer.Deserialize(ref reader, tK, SerializationFlags.None, config);
                var value = SosoSerializer.Deserialize(ref reader, tV, SerializationFlags.None, config);
                dict.Add(key, value);
            }
            return dict;
        }
        public void Serialize(ref ByteWriter writer, object value, SerializationConfig config)
        {
            IDictionary dict = value as IDictionary;
            
            Type[] tK = dict.Keys.GetType().GetGenericArguments();
            writer.Write((SosoTypeCode)tK[0]);
            writer.Write((SosoTypeCode)tK[1]);
            int count = dict.Count;
            writer.Write(count);
            foreach (object key in dict.Keys)
            {
                SosoSerializer.Serialize(ref writer, key, SerializationFlags.None, config);
                SosoSerializer.Serialize(ref writer, dict[key], SerializationFlags.None, config);
            }
        }
    }
}