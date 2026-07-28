using System;
using System.Collections.Generic;
using Soso.Serialization.Logging;

namespace Soso.Serialization.Reflection
{
    public class InterfaceMap
    {
        private readonly Dictionary<Type, Type> _interfaceMap = new Dictionary<Type, Type>();

        public bool TryGetMapping(Type interfaceType, out Type type)
        {
            return _interfaceMap.TryGetValue(interfaceType, out type);
        }
        
        public void AddMapping<TI, T>()
        {
            Type interfaceType = typeof(TI);
            Type targetType = typeof(T);
            if (_interfaceMap.ContainsKey(interfaceType))
            {
                Log.Warn($"Mapping already exists for interface {interfaceType.FullName} with type {_interfaceMap[interfaceType].FullName}. " +
                         $"Overriding with {targetType.FullName}");
            }
            _interfaceMap[interfaceType] = targetType;
        }
    }
}