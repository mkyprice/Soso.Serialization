using Soso.Serialization.Binary;
using Soso.Serialization.Reflection;
using Soso.Serialization.Reflection.Extensions;
using System;
using System.Reflection;
using Soso.Serialization.Logging;

namespace Soso.Serialization.Serializers.RW
{
	public readonly struct ObjectWriter(SerializationConfig config, SerializerMap map)
    {
        public void Serialize(ref ByteWriter writer, object value, Type type, SerializationFlags flags)
        {
            if (type == typeof(object))
            {
                Log.Warn($"Type of object {value} was type object");
            }

            // if (config.GetInterfaceMapping().TryGetMapping(type, out Type mappedType))
            // {
            //     type = mappedType;
            // }
            
            if (flags == SerializationFlags.EmbedType)
            {
                EmbedType(ref writer, type);
            }
            
            // Special cases
            if (type.IsArray)
            {
                WriteArray(ref writer, (Array)value);
                return;
            }
            
            // Should catch all primitives and basic types
            if (map.TryGetSerializer(type, out ISerializer serializer))
            {
                serializer.Serialize(ref writer, value, config);
                return;
            }
            
            if (type.BaseType == typeof(Action) ||
                type.BaseType == typeof(Delegate))
            {
                return;
            }

            WriteObject(ref writer, value, type);
        }

        private void EmbedType(ref ByteWriter writer, Type type)
        {
            SosoTypeCode code;
            if (type.IsArray)
            {
                code = (SosoTypeCode)typeof(Array);
            }
            else
            {
                code = (SosoTypeCode)type;
            }
            writer.Write(code);
        }
        
        private void WriteObject(ref ByteWriter writer, object value, Type type)
        {
            if (value == null)
            {
                return;
            }
            foreach (MemberInfo member in TypeMemberCache.GetCache(type).GetMembers())
            {
                object memberValue = member.GetMemberValue(value);
                if (ReferenceEquals(memberValue, null))
                {
                    continue;
                }
                Type memberType = memberValue.GetType();
                Serialize(ref writer, memberValue, memberType, SerializationFlags.None);
            }
        }
        
        private void WriteArray(ref ByteWriter writer, Array value)
        {
            SosoTypeCode eT = value.GetType().GetElementType();

            writer.Write(eT);
            writer.Write(value.Length);
            foreach (object item in value)
            {
                Serialize(ref writer, item, eT, SerializationFlags.None);
            }
        }
	}
}
