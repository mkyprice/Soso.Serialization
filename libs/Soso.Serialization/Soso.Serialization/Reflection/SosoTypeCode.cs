using System;
using System.Collections.Generic;

namespace Soso.Serialization.Reflection
{
    public readonly struct SosoTypeCode
    {
        public readonly int Code;
        public Type Type
        {
            get
            {
                if (_codeToType.TryGetValue(Code, out Type type) == false)
                {
                    throw new ArgumentException($"No type for {nameof(SosoTypeCode)} with code: {Code}");
                }
                return type;
            }
        }

        public SosoTypeCode(Type type)
        {
            if (_typeToCodes.TryGetValue(type, out SosoTypeCode code) == false)
            {
                throw new ArgumentException($"No {nameof(SosoTypeCode)} for type {type.FullName}");
            }
            Code = code.Code;
        }

        public SosoTypeCode(int code)
        {
            Code = code;
        }

        public static implicit operator Type(SosoTypeCode typeCode) => typeCode.Type;
        public static implicit operator SosoTypeCode(Type type) => new SosoTypeCode(type);
        public static implicit operator int(SosoTypeCode typeCode) => typeCode.Code;
        public static implicit operator SosoTypeCode(int code) => new SosoTypeCode(code);

        private static readonly Dictionary<Type, SosoTypeCode> _typeToCodes = new Dictionary<Type, SosoTypeCode>();
        private static readonly Dictionary<SosoTypeCode, Type> _codeToType = new Dictionary<SosoTypeCode, Type>();
        public static void AddStreamingType(Type type)
        {
            var codeNum = (int)(type.FullName?.GetHashCode() ?? type.Name.GetHashCode());
            SosoTypeCode code = new SosoTypeCode(codeNum);
            if (_codeToType.TryGetValue(code, out var existingType))
            {
                if (existingType != type)
                {
                    throw new Exception($"Failed to {nameof(AddStreamingType)} for type " +
                                        $"{type.FullName} as code {code} already exists for " +
                                        $"type {existingType.FullName}");
                }
            }

            _typeToCodes[type] = code;
            _codeToType[code] = type;
        }
    }
}