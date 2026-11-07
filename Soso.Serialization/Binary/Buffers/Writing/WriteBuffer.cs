using Soso.Serialization.Logging;
using System;

namespace Soso.Serialization.Binary.Buffers.Writing
{
	public struct WriteBuffer<T>(T[] buffer) : IWriteBuffer<T>
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

		public void Write(T data)
		{
			if (Position + 1 > buffer.Length)
			{
				throw new IndexOutOfRangeException($"Buffer is too small. Tried to write at {Position + 1}. Size was {buffer.Length}");
			}
			buffer[Position] = data;
			Position++;
		}
		public void Write(Span<T> bytes)
		{
			if (Position + bytes.Length > buffer.Length)
			{
				throw new IndexOutOfRangeException($"Buffer is too small. Tried to write at {Position + bytes.Length}. Size was {buffer.Length}");
			}

			var dest = new Span<T>(buffer, (int)Position, bytes.Length);
			bytes.CopyTo(dest);
			Position += bytes.Length;
		}
		
		public Span<T> ToSpan()
		{
			return new Span<T>(buffer, 0, Count);
		}
	}
}
