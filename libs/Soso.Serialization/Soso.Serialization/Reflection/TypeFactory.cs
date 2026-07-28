using System;
using System.Collections.Generic;

namespace Soso.Serialization.Reflection
{
    public class TypeFactory
    {
        private readonly Dictionary<Type, Func<object[], object>> _factory = new Dictionary<Type, Func<object[], object>>();
        
        public void SetFactory<T>(Func<object[], object> creation)
        {
            _factory[typeof(T)] = creation;
        }

        public object CreateInstance(Type type, params object[] args)
        {
            if (_factory.TryGetValue(type, out Func<object[], object> createFunc))
            {
                return createFunc(args);
            }
            return Activator.CreateInstance(type, args);
        }

        public T CreateInstance<T>(Type type, params object[] args)
        {
            return (T)CreateInstance(type, args);
        }

        public T CreateInstance<T>(params object[] args)
        {
            return (T)CreateInstance(typeof(T), args);
        }
    }
}