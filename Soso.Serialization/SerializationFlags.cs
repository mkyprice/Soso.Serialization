using System;

namespace Soso.Serialization
{
    [Flags]
    public enum SerializationFlags
    {
        None = 0,
        EmbedType = 1,
    }
}