using System;
using System.IO;

namespace Soso.Serialization.Binary
{
	public ref struct ByteReader(ReadOnlySpan<byte> data)
	{
		public int Count => _data.Length;
		public int Position
		{
			get => _position;
			set => _position = value;
		}
		private readonly ReadOnlySpan<byte> _data = data;
		private int _position = 0;
		
		public void Skip(int count)
		{
			_position += count;
		}

		#region Out Reads
		
		public void Read(out byte value) => value = ReadByte();
		public void Read(out sbyte value) => value = ReadSByte();
		public void Read(out short value) => value = ReadShort();
		public void Read(out ushort value) => value = ReadUShort();
		public void Read(out int value) => value = ReadInt();
		public void Read(out uint value) => value = ReadUInt();
		public void Read(out long value) => value = ReadLong();
		public void Read(out ulong value) => value = ReadULong();
		public void Read(out float value) => value = ReadFloat();
		public void Read(out decimal value) => value = ReadDecimal();
		public void Read(out double value) => value = ReadDouble();
		public void Read(out bool value) => value = ReadBool();
		public void Read(out char value) => value = ReadChar();
		public void Read(out string value) => value = ReadString();

		#endregion
		
		#region Peek

		public byte PeekByte(int offset = 0)
		{
			VerifySize(offset + 1);
			byte v = _data[_position + offset];
			return v;
		}

		public sbyte PeekSByte(int offset = 0)
		{
			return (sbyte)PeekByte(offset);
		}

		public short PeekShort(int offset = 0)
		{
			return (short)(PeekByte(offset + 0) |
			               PeekByte(offset + 1) << 8);
		}

		public ushort PeekUShort(int offset = 0)
		{
			return (ushort)(PeekShort(offset));
		}

		public int PeekInt(int offset = 0)
		{
			return (int)(PeekByte(offset + 0) |
			             PeekByte(offset + 1) << 8 |
			             PeekByte(offset + 2) << 16 |
			             PeekByte(offset + 3) << 24);
		}
		public uint PeekUInt(int offset = 0)
		{
			return (uint)(PeekByte(offset + 0) |
			              PeekByte(offset + 1) << 8 |
			              PeekByte(offset + 2) << 16 |
			              PeekByte(offset + 3) << 24
				);
		}

		public long PeekLong(int offset = 0)
		{
			uint low = PeekUInt(offset);
			uint high = PeekUInt(offset + sizeof(uint));
			return (long)((ulong)high << 32 | low);
		}

		public ulong PeekULong(int offset = 0)
		{
			uint low = PeekUInt(offset);
			uint high = PeekUInt(offset + sizeof(uint));
			return (ulong)((ulong)high << 32 | low);
		}

		public unsafe float PeekFloat(int offset = 0)
		{
			uint iv = PeekUInt(offset);
			return *((float*)&iv);
		}

		public decimal PeekDecimal(int offset = 0)
		{
			int[] buffer = new int[4];
			buffer[0] = PeekInt(offset);
			buffer[1] = PeekInt(offset + sizeof(int));
			buffer[2] = PeekInt(offset + sizeof(int) * 2);
			buffer[3] = PeekInt(offset + sizeof(int) * 3);
			return new decimal(buffer);
		}

		public unsafe double PeekDouble(int offset = 0)
		{
			ulong lv = PeekULong(offset);
			return *((double*)&lv);
		}

		public bool PeekBool(int offset = 0)
		{
			return PeekByte(offset) == 1;
		}

		public char PeekChar(int offset = 0)
		{
			return (char)PeekByte(offset);
		}

		#endregion
		
		#region Read

		public unsafe T ReadBlittable<T>()
			where T : unmanaged
		{
			int size = sizeof(T);
			if (_position + size > _data.Length)
			{
				throw new EndOfStreamException($"{nameof(ReadBlittable)}<{typeof(T).Name}> is out of range. Tried to read at {_position + size} but length was {_data.Length}.");
			}

			T value;
			fixed (byte* ptr = &_data[_position])
			{
				value = *(T*)ptr;
			}
			Skip(size);
			return value;
		}
		
		public byte ReadByte()
		{
			byte v = PeekByte();
			Skip(sizeof(byte));
			return v;
		}

		public sbyte ReadSByte()
		{
			return (sbyte)ReadByte();
		}

		public short ReadShort()
		{
			var value = PeekShort();
			Skip(sizeof(short));
			return value;
		}

		public ushort ReadUShort()
		{
			var value = PeekUShort();
			Skip(sizeof(short));
			return value;
		}

		public int ReadInt()
		{
			var value = PeekInt();
			Skip(sizeof(int));
			return value;
		}

		public uint ReadUInt()
		{
			var value = PeekUInt();
			Skip(sizeof(int));
			return value;
		}

		public long ReadLong()
		{
			var value = PeekLong();
			Skip(sizeof(long));
			return value;
		}

		public ulong ReadULong()
		{
			var value = PeekULong();
			Skip(sizeof(ulong));
			return value;
		}

		public float ReadFloat()
		{
			var value = PeekFloat();
			Skip(sizeof(float));
			return value;
		}

		public decimal ReadDecimal()
		{
			var value = PeekDecimal();
			Skip(sizeof(decimal));
			return value;
		}

		public double ReadDouble()
		{
			var value = PeekDouble();
			Skip(sizeof(double));
			return value;
		}

		public bool ReadBool()
		{
			return ReadByte() == 1;
		}

		public char ReadChar()
		{
			return (char)ReadByte();
		}
		public byte[] ReadBytes(int count)
		{
			VerifySize(count);
			byte[] bytes = new byte[count];
			_data.Slice(_position, count).CopyTo(bytes);
			_position += count;
			return bytes;
		}

		public ReadOnlySpan<byte> ReadSpan(int count)
		{
			VerifySize(count);
			ReadOnlySpan<byte> bytes = _data.Slice(_position, count);
			_position += count;
			return bytes;
		}

		private int Read7BitEncodedInt()
		{
			int count = 0;
			int shift = 0;
			byte b;
			do
			{
				if (shift == 5 * 7) throw new FormatException("Failed to read 7 bit encoding from stream");

				b = (byte)ReadByte();
				count |= (b & 0x7f) << shift;
				shift += 7;
			} while ((b & 0x80) != 0);
			return count;
		}

		public string ReadString()
		{
			int length = Read7BitEncodedInt();
			if (length == 0) return string.Empty;

			var bytes = ReadSpan(length);
			return SosoSerializer.DefaultEncoding.GetString(bytes);
		}

		#endregion


		private void VerifySize(int count)
		{
			if (_position + count > Count)
			{
				throw new IndexOutOfRangeException($"ByteReader - Tried to access {_position + count}. Size was {Count}");
			}
		}
	}
}
