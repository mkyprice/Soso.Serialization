using Soso.Serialization.Binary;
using Soso.Serialization.Binary.Extensions;
using Soso.Serialization.Reflection;
using Soso.Serialization.Reflection.Extensions;
using System;

namespace Soso.Serialization.Serializers.RW
{
	public readonly struct ObjectReader(SerializationConfig config, SerializerMap map)
    {
        public object Deserialize(ref ByteReader reader)
        {
            if (reader.Position >= reader.Count)
            {
                return default;
            }
            
            // Read type
            Type type = reader.ReadType();

            return Deserialize(ref reader, type);
        }
        
        public object Deserialize(ref ByteReader reader, Type type)
        {
            if (config.GetInterfaceMapping().TryGetMapping(type, out Type mappedType))
            {
                type = mappedType;
            }

            // Special cases
            if (type.IsArray)
            {
                return ReadArray(ref reader);
            }
            
            // Should catch all primitives and basic types
            if (map.TryGetSerializer(type, out ISerializer serializer))
            {
                return serializer.Deserialize(ref reader, config);
            }

            if (type.BaseType == typeof(Action) ||
                type.BaseType == typeof(Delegate))
            {
                return default;
            }

            return ReadObject(ref reader, type);
        }

        private object ReadObject(ref ByteReader reader, Type type)
        {
            object obj = config.GetFactory().CreateInstance(type);
            foreach (var member in TypeMemberCache.GetCache(type).GetMembers())
            {
                Type memberType = member.GetMemberType();
                object memberValue = Deserialize(ref reader, memberType);
                member.SetMemberValue(obj, memberValue);
            }
            return obj;
        }

        private Array ReadArray(ref ByteReader reader)
        {
            Type eT = reader.ReadType();
            int count = reader.ReadInt();
            
            Array list = Array.CreateInstance(eT, count);
            for (int i = 0; i < count; i++)
            {
                var item = Deserialize(ref reader, eT);
                list.SetValue(item, i);
            }
            return list;
        }
	}
}
