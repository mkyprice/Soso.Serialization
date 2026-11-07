using System;
using Soso.Serialization.Logging;

namespace Soso.Serialization.Binary.Buffers.Reading
{
	public class BasicReadBuffer<T> : IReadBuffer<T>
	{
		public static int DEFAULT_CAPACITY = 1024; // 1MB
		public long Position { get; set; }
		public long Count { get => _count; }

		private long _count = 0;
		private T[] _buffer;

		public BasicReadBuffer()
		{
			_buffer = new T[DEFAULT_CAPACITY];
		}

		public BasicReadBuffer(int capacity)
		{
			_buffer = new T[capacity];
		}
		public void Append(Span<T> bytes)
		{
			while (Position + bytes.Length > _buffer.Length)
			{
				Resize(_buffer.Length * 2);
			}
			var span = new Span<T>(_buffer, (int)Position, bytes.Length);
			bytes.CopyTo(span);
			_count += bytes.Length;
		}
		public T Peek(int offset)
		{
			if (offset + Position > Count)
			{
				throw new IndexOutOfRangeException("Tried to peek outside of buffer");
			}
			return _buffer[Position + offset];
		}
		public T[] ReadRange(int count)
		{
			var result = new Span<T>(_buffer, (int)Position, count).ToArray();
			Position += count;
			return result;
		}
		public void Reset()
		{
			Position = 0;
			_count = 0;
		}

		public void Resize(int size)
		{
			var tmp = new T[size];
			int maxCopySize = Math.Min(tmp.Length, _buffer.Length);
			Array.Copy(_buffer, 0, tmp, 0, maxCopySize);
			_buffer = tmp;
			_count = Math.Min(_count, size);
			Position = Math.Min(_count, Position);
			Log.Info($"Resized buffer to {size}");
		}
	}
}
