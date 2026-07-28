using System;
using System.Collections.Generic;

namespace Soso.Serialization.Serializers
{
    public class SerializerMap
    {
        private readonly Dictionary<Type, ISerializer> _customSerializers = new Dictionary<Type, ISerializer>();

        public void AddSerializer<T>(ISerializer<T> serializer)
        {
            _customSerializers[typeof(T)] = serializer;
        }

        public void AddSerializer<T>(Serializer<T>.SerializeDelegate serialize, Serializer<T>.DeserializeDelegate deserialize, params Type[] additionalTypes)
        {
            Serializer<T> serializer = new Serializer<T>(serialize, deserialize);
            AddSerializer(serializer);
            foreach (Type extra in additionalTypes)
            {
                AddSerializer(serializer, extra);
            }
        }

        public void AddSerializer(ISerializer serializer, Type type)
        {
            _customSerializers[type] = serializer;
        }

        public void AddSerializer(ISerializer serializer, Type type, params Type[] types)
        {
            AddSerializer(serializer, type);
            foreach (Type extra in types)
            {
                AddSerializer(serializer, extra);
            }
        }

        public bool TryGetSerializer(Type type, out ISerializer serializer)
        {
            if (type.IsGenericType)
            {
                type = type.GetGenericTypeDefinition();
            }
            return _customSerializers.TryGetValue(type, out serializer);
        }
    }
}