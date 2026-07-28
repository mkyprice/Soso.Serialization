using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Soso.Serialization.Binary;
using Soso.Serialization.Binary.Extensions;
using Soso.Serialization.Reflection;

namespace Soso.Serialization.Serializers.Defaults
{
    public class ListSerializer : ISerializer
    {
        public readonly Type ListType;

        public ListSerializer(Type listType)
        {
            ListType = listType;
        }
        
        public void Serialize(ref ByteWriter writer, object value, SerializationConfig config)
        {
            IList ilist = value as IList;
            if (ilist == null)
            {
                throw new Exception("List was null");
            }
            Type[] tK = value.GetType().GetGenericArguments();

            writer.Write((SosoTypeCode)tK[0]);
            writer.Write(ilist.Count);
            foreach (object item in ilist)
            {
                SosoSerializer.Serialize(ref writer, item, SerializationFlags.None, config);
            }
        }

        public object Deserialize(ref ByteReader reader, SerializationConfig config)
        {
            Type eT = reader.ReadType();
            Type listType = ListType.MakeGenericType(eT);
            IList list = config.GetFactory().CreateInstance<IList>(listType);

            int count = reader.ReadInt();
            for (int i = 0; i < count; i++)
            {
                var item = SosoSerializer.Deserialize(ref reader, eT, SerializationFlags.None, config);
                list.Add(item);
            }
            return list;
        }
    }
}