using System;
using System.Collections.Generic;

namespace Soso.Serialization.Reflection
{
    public static class TypeMemberCache
    {
        private static readonly Dictionary<Type, MemberMap> _memberMaps = new Dictionary<Type, MemberMap>();

        public static MemberMap Cache(Type type)
        {
            if (_memberMaps.TryGetValue(type, out MemberMap map) == false)
            {
                map = new MemberMap(type);
                _memberMaps[type] = map;
            }
            return map;
        }

        public static MemberMap GetCache(Type type)
        {
            if (_memberMaps.TryGetValue(type, out MemberMap map) == false)
            {
                map = Cache(type);
            }
            return map;
        }
    }
}