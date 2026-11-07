using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Soso.Serialization.Logging;
using Soso.Serialization.Reflection.Extensions;

namespace Soso.Serialization.Reflection
{
    public class MemberMap
    {
        public static BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public;
        
        public static readonly List<Type> IgnoredAttributes = new List<Type>()
        {
            typeof(NonSerializedAttribute), typeof(IgnoreDataMemberAttribute), typeof(IndexerNameAttribute)
        };
        public static readonly List<Type> IgnoredTypes = new List<Type>()
        {
            typeof(Action), typeof(Action<>)
        };
        
        private readonly Type _dataType;
        private readonly MemberInfo[] _infos;

        public MemberMap(Type type)
        {
            _dataType = type;
            
            FieldInfo[] fields = _dataType.GetFields(Flags);
            PropertyInfo[] props = _dataType.GetProperties(Flags);

            List<MemberInfo> _serializableFields = new List<MemberInfo>();
            foreach (var field in fields)
            {
                if (IsSerializable(field))
                {
                    _serializableFields.Add(field);
                }
                else
                {
                    Log.Debug($"Ignoring field: {field.Name} in {type.Name}");
                }
            }
            foreach (var prop in props)
            {
                if (IsSerializable(prop) 
                    && prop.SetMethod != null
                    && prop.IsSpecialName == false
                    && prop.CanRead
                    && prop.CanWrite
                    && prop.GetIndexParameters()?.Length <= 0 // Indexer
                    )
                {
                    _serializableFields.Add(prop);
                }
                else
                {
                    Log.Debug($"Ignoring property: {prop.Name} in {type.Name}");
                }
            }

            _infos = _serializableFields.ToArray();
        }

        public IEnumerable<MemberInfo> GetMembers()
        {
            return _infos;
        }

        public IEnumerable<Type> GetMemberTypes()
        {
            foreach (MemberInfo info in _infos)
            {
                yield return info.GetMemberType();
            }
        }

        private bool IsSerializable(MemberInfo field)
        {
            Type type = field.GetMemberType();
            if (IgnoredTypes.Contains(type))
            {
                return false;
            }

            foreach (Type ignoredAttribute in IgnoredAttributes)
            {
                if (type.GetCustomAttribute(ignoredAttribute) != null)
                {
                    return false;
                }
            }
            return true;
        }
    }
}