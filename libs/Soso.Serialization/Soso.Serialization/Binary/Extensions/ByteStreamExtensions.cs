using System;
using Soso.Serialization.Reflection;

namespace Soso.Serialization.Binary.Extensions
{
	public static class ByteStreamExtensions
	{
		public static void Write(this ref ByteWriter writer, SosoTypeCode type)
		{
			writer.Write(type.Code);
		}
		
		public static void Write<T>(this ref ByteWriter writer, T value)
			where T : unmanaged
		{
			switch (value)
			{
				case byte v: writer.Write(v); break;
				case sbyte v: writer.Write(v); break;
				case short v: writer.Write(v); break;
				case ushort v: writer.Write(v); break;
				case int v: writer.Write(v); break;
				case uint v: writer.Write(v); break;
				case long v: writer.Write(v); break;
				case ulong v: writer.Write(v); break;
				case float v: writer.Write(v); break;
				case double v: writer.Write(v); break;
				case decimal v: writer.Write(v); break;
				case bool v: writer.Write(v); break;
				case char v: writer.Write(v); break;
				default:
					throw new Exception($"Type {typeof(T).Name} cannot be written by {nameof(ByteWriter)}");
			}
		}
		
		
		public static Type ReadType(this ref ByteReader br)
		{
			SosoTypeCode typeCode = br.ReadInt();
			return typeCode;
		}
		
		public static T Read<T>(this ref ByteReader reader)
			where T : unmanaged
		{
			object result;
			switch (typeof(T).Name)
			{
				case nameof(Byte): result = reader.ReadByte(); break;
				case nameof(SByte): result = reader.ReadSByte(); break;
				case nameof(Int16): result = reader.ReadShort(); break;
				case nameof(UInt16): result = reader.ReadUShort(); break;
				case nameof(Int32): result = reader.ReadInt(); break;
				case nameof(UInt32): result = reader.ReadUInt(); break;
				case nameof(Int64): result = reader.ReadLong(); break;
				case nameof(UInt64): result = reader.ReadULong(); break;
				case nameof(Single): result = reader.ReadFloat(); break;
				case nameof(Double): result = reader.ReadDouble(); break;
				case nameof(Decimal): result = reader.ReadDecimal(); break;
				case nameof(Boolean): result = reader.ReadBool(); break;
				case nameof(Char): result = reader.ReadChar(); break;
				default:
					throw new Exception($"Type {typeof(T).Name} cannot be read by {nameof(ByteReader)}");
			}
			return (T)result;
		}
		public static T Peek<T>(this ref ByteReader reader)
			where T : unmanaged
		{
			object result;
			switch (typeof(T).Name)
			{
				case nameof(Byte): result = reader.PeekByte(); break;
				case nameof(SByte): result = reader.PeekSByte(); break;
				case nameof(Int16): result = reader.PeekShort(); break;
				case nameof(UInt16): result = reader.PeekUShort(); break;
				case nameof(Int32): result = reader.PeekInt(); break;
				case nameof(UInt32): result = reader.PeekUInt(); break;
				case nameof(Int64): result = reader.PeekLong(); break;
				case nameof(UInt64): result = reader.PeekULong(); break;
				case nameof(Single): result = reader.PeekFloat(); break;
				case nameof(Double): result = reader.PeekDouble(); break;
				case nameof(Decimal): result = reader.PeekDecimal(); break;
				case nameof(Boolean): result = reader.PeekBool(); break;
				case nameof(Char): result = reader.PeekChar(); break;
				default:
					throw new Exception($"Type {typeof(T).Name} cannot be read by {nameof(ByteReader)}");
			}
			return (T)result;
		}
	}
}
