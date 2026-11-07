using System;
using System.Reflection;

namespace Soso.Serialization.Reflection.Extensions
{
    public static class MemberInfoExtensions
    {
        public static Type GetMemberType(this MemberInfo info)
        {
            switch (info)
            {
                case PropertyInfo prop:
                    return prop.PropertyType;
                case FieldInfo field:
                    return field.FieldType;
                default:
                    throw new Exception($"MemberInfo {info} was unrecognized type");
            }
        }
        
        public static object GetMemberValue(this MemberInfo info, object target)
        {
            switch (info)
            {
                case PropertyInfo prop:
                    return prop.GetValue(target);
                case FieldInfo field:
                    return field.GetValue(target);
                default:
                    throw new Exception($"MemberInfo {info} was unrecognized type");
            }
        }

        public static void SetMemberValue(this MemberInfo info, object target, object value)
        {
            switch (info)
            {
                case PropertyInfo prop:
                    prop.SetValue(target, value);
                    break;
                case FieldInfo field:
                    field.SetValue(target, value);
                    break;
                default:
                    throw new Exception($"MemberInfo {info} was unrecognized type");
            }
        }
    }
}