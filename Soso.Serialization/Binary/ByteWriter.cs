using System;

namespace Soso.Serialization.Binary
{
	public ref struct ByteWriter(Span<byte> buffer)
	{
		public int Position
		{
			get => _position;
			set
			{
				_position = value;
				if (Position > Count)
				{
					_count = _position;
				}
			}
		}
		public int Count
		{
			get => _count;
			set
			{
				_count = value;
				if (Position > Count)
				{
					Position = _count;
				}
			}
		}

		private int _position = 0;
		private int _count = 0;
		private readonly Span<byte> _buffer = buffer;
		
		public Span<byte> ToSpan()
		{
			return _buffer.Slice(0, Count);
		}

		public void Seek(int position)
		{
			Position = position;
		}

		public void Skip(int count)
		{
			Position += count;
		}

		public unsafe void WriteBlittable<T>(T value)
			where T : unmanaged
		{
			int size = sizeof(T);

			if (Position + size > _buffer.Length)
			{
				throw new IndexOutOfRangeException($"{nameof(WriteBlittable)}<{typeof(T)}> Buffer is too small. Tried to write at {Position + size}. Size was {_buffer.Length}");
			}

			fixed (byte* ptr = &_buffer[_position])
			{
				*(T*)ptr = value;
			}
			Position += size;
		}

		public void Write(byte data)
		{
			if (Position + 1 > _buffer.Length)
			{
				throw new IndexOutOfRangeException($"Buffer is too small. Tried to write at {Position + 1}. Size was {_buffer.Length}");
			}
			_buffer[Position] = data;
			Position++;
		}
		
		public void Write(ReadOnlySpan<byte> bytes)
		{
			if (Position + bytes.Length > _buffer.Length)
			{
				throw new IndexOutOfRangeException($"Buffer is too small. Tried to write at {Position + bytes.Length}. Size was {_buffer.Length}");
			}

			Span<byte> slice = _buffer.Slice(Position, bytes.Length);
			bytes.CopyTo(slice);
			Position += bytes.Length;
		}

		public void Write(sbyte value)
		{
			Write((byte)value);
		}

		public void Write(char value)
		{
			Write((byte)value);
		}

		public void Write(bool value)
		{
			Write((byte)(value ? 1 : 0));
		}

		public void Write(short value)
		{
			Write((byte)value);
			Write((byte)(value >> 8));
		}

		public void Write(ushort value)
		{
			Write((byte)value);
			Write((byte)(value >> 8));
		}

		public void Write(int value)
		{
			Write((byte)value);
			Write((byte)(value >> 8));
			Write((byte)(value >> 16));
			Write((byte)(value >> 24));
		}

		public void Write(uint value)
		{
			Write((byte)value);
			Write((byte)(value >> 8));
			Write((byte)(value >> 16));
			Write((byte)(value >> 24));
		}

		public void Write(long value)
		{
			Write((byte)value);
			Write((byte)(value >> 8));
			Write((byte)(value >> 16));
			Write((byte)(value >> 24));
			Write((byte)(value >> 32));
			Write((byte)(value >> 40));
			Write((byte)(value >> 48));
			Write((byte)(value >> 56));
		}

		public void Write(ulong value)
		{
			Write((byte)value);
			Write((byte)(value >> 8));
			Write((byte)(value >> 16));
			Write((byte)(value >> 24));
			Write((byte)(value >> 32));
			Write((byte)(value >> 40));
			Write((byte)(value >> 48));
			Write((byte)(value >> 56));
		}

		public unsafe void Write(float value)
		{
			uint v = *(uint*)&value;
			Write(v);
		}

		public unsafe void Write(double value)
		{
			ulong v = *(ulong*)&value;
			Write(v);
		}

		public void Write(decimal value)
		{
			var bytes = decimal.GetBits(value);
			Write(bytes[0]);
			Write(bytes[1]);
			Write(bytes[2]);
			Write(bytes[3]);
		}

		public void Write(string value)
		{
			byte[] bytes;
			if (string.IsNullOrEmpty(value))
			{
				bytes = Array.Empty<byte>();
			}
			else
			{
				bytes = SosoSerializer.DefaultEncoding.GetBytes(value);
			}
			int length = bytes.Length;
			Write7BitEncodedInt(length);
			Write(bytes);
		}

		public void Write7BitEncodedInt(int value)
		{
			uint count = (uint)value;
			while (count >= 0x80)
			{
				Write((byte)(count | 0x80));
				count >>= 7;
			}
			Write((byte)count);
		}
	}
}
